using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Server.Envir;
using Reactor = License;
using Newtonsoft.Json;

namespace Server.Helpers
{
    public static class LicenseHelper
    {
        private static readonly string LICENSE_BACKUP = "license.license.bak";
        private static readonly string INVALIDATE_RECORD = "invalidate.record";
        private static readonly string INVALIDATE_ATTEMPTS_FILE = "invalidate_attempts.json";
        private static readonly int MAX_INVALIDATE_ATTEMPTS = 3;
        private static readonly TimeSpan INVALIDATE_COOLDOWN = TimeSpan.FromHours(24);

        private class InvalidateAttempts
        {
            public int Count { get; set; }
            public DateTime LastAttempt { get; set; }
        }

        static LicenseHelper()
        {
            // 注册LoadLicenseEvent事件处理程序
            Reactor.Status.LoadLicenseEvent += (byte[] license) =>
            {
                try
                {
                    // 解密数据
                    string jsonData = DecryptBytes(license);

                    // 反序列化JSON数据
                    var licenseInfo = JsonConvert.DeserializeObject<License.LicenseGenerator.LicenseInfo>(jsonData);

                    // 反射赋值
                    var type = typeof(Reactor.Status);
                    type.GetField("licensed", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, true);
                    type.GetField("eva_lock", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, licenseInfo.EnableEvaluationLock);
                    type.GetField("date_lock", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, licenseInfo.EnableExpirationDateLock);
                    type.GetField("uses_lock", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, licenseInfo.EnableNumberOfUsesLock);
                    type.GetField("instances_lock", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, licenseInfo.EnableNumberOfInstancesLock);
                    type.GetField("hardware_lock", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, licenseInfo.EnableHardwareLock);
                    type.GetField("eva_time", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, licenseInfo.EvaluationTime);
                    type.GetField("eva_type", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, licenseInfo.EvaluationType);
                    type.GetField("lhid", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, licenseInfo.HardwareID);
                    type.GetField("uses", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, licenseInfo.NumberOfUses);
                    type.GetField("instances", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, licenseInfo.NumberOfInstances);
                    type.GetField("license_exp_date", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, licenseInfo.ExpirationDate);
                    type.GetField("lic", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, license);

                    // 更新自定义数据
                    var keyValueList = type.GetField("key_n_value", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null) as System.Collections.SortedList;
                    if (keyValueList != null)
                    {
                        keyValueList.Clear();
                        foreach (var kvp in licenseInfo.CustomData)
                        {
                            keyValueList[kvp.Key] = kvp.Value;
                        }
                    }

                    SEnvir.Log("授权文件解析成功");
                }
                catch (Exception ex)
                {
                    SEnvir.Log($"解析授权文件失败: {ex.Message}");
                    var type = typeof(Reactor.Status);
                    type.GetField("licensed", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, false);
                }
            };

            ReLoadLicense(null);
        }

        private static string DecryptBytes(byte[] cipherText)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] key = new byte[32];
                    byte[] iv = new byte[16];
                    
                    // 使用固定的密钥和IV
                    string keyStr = "ZirconLicenseKey1234567890123456789012345678";
                    string ivStr = "ZirconLicenseIV123";
                    
                    // 确保密钥和IV长度正确
                    Array.Copy(Encoding.UTF8.GetBytes(keyStr), key, Math.Min(keyStr.Length, 32));
                    Array.Copy(Encoding.UTF8.GetBytes(ivStr), iv, Math.Min(ivStr.Length, 16));
                    
                    aes.Key = key;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                SEnvir.Log($"解密失败: {ex.Message}");
                return null;
            }
        }

        public static bool ReLoadLicense(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Config.LicenseFile;
            }

            return LoadLicense();
        }

        public static LicenseState CheckLicense()
        {
            if (!Reactor.Status.Licensed)
            {
                return LicenseState.Invalid;
            }

            if (Reactor.Status.Expired)
            {
                return LicenseState.Expired;
            }

            if (Reactor.Status.Hardware_Lock_Enabled && CurrentHardwareID != LicenseHardwareID)
            {
                return LicenseState.HardwareNotMatched;
            }

            if (Reactor.Status.Evaluation_Lock_Enabled &&
                Reactor.Status.Evaluation_Time_Current > Reactor.Status.Evaluation_Time)
            {
                return LicenseState.EvaluationExpired;
            }

            if (Reactor.Status.Number_Of_Uses_Lock_Enable &&
                Reactor.Status.Number_Of_Uses_Current > Reactor.Status.Number_Of_Uses)
            {
                return LicenseState.UsesExceeded;
            }
#if DEBUG
                return LicenseState.DebugMode;
#else
            return Reactor.Status.Licensed ? LicenseState.Licensed : LicenseState.Invalid; 
            // return LicenseState.Licensed;
#endif 
        }

        public static bool IsLicensed => Reactor.Status.Licensed;
        public static string CurrentHardwareID => Reactor.Status.HardwareID;
        public static string LicenseHardwareID => Reactor.Status.License_HardwareID;
        public static bool IsEvaluation => Reactor.Status.Evaluation_Lock_Enabled;
        public static string EvaluationType =>
            Reactor.Status.Evaluation_Type == Reactor.EvaluationType.Trial_Days ? "按天试用" : "按运行时长试用";
        public static int EvaluationRemainingTime =>
            Reactor.Status.Evaluation_Time - Reactor.Status.Evaluation_Time_Current + 1;

        public static bool HardwareLockEnabled => Reactor.Status.Hardware_Lock_Enabled;
        public static bool HardwareMatch => CurrentHardwareID == LicenseHardwareID;


        public static Dictionary<string, string> GetAdditionalInfo()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            if (!Reactor.Status.Licensed) return result;

            for (int i = 0; i < Reactor.Status.KeyValueList.Count; i++)
            {
                result[Reactor.Status.KeyValueList.GetKey(i).ToString()] =
                    Reactor.Status.KeyValueList.GetByIndex(i).ToString();
            }

            return result;
        }

        public static string InvalidateLicense()
        {
            try
            {
                // 检查是否可以解绑
                if (!CanInvalidate())
                {
                    return null;
                }

                // 生成解绑确认码
                string confirmationCode = GenerateConfirmationCode();
                
                // 备份当前授权文件
                if (File.Exists(Config.LicenseFile))
                {
                    File.Copy(Config.LicenseFile, LICENSE_BACKUP, true);
                }

                // 删除当前授权文件
                if (File.Exists(Config.LicenseFile))
                {
                    File.Delete(Config.LicenseFile);
                }

                // 记录解绑信息
                var invalidateInfo = new
                {
                    HardwareID = Reactor.Status.HardwareID,
                    ConfirmationCode = confirmationCode,
                    InvalidateTime = DateTime.Now,
                    BackupFile = LICENSE_BACKUP
                };
                File.WriteAllText(INVALIDATE_RECORD, JsonConvert.SerializeObject(invalidateInfo));

                // 重置授权状态
                Reactor.Status.UpdateLicenseInfo(new Reactor.LicenseGenerator.LicenseInfo(), null);

                // 记录解绑尝试
                RecordInvalidateAttempt();

                // 记录操作日志
                LogOperation("解绑", $"硬件ID: {Reactor.Status.HardwareID}, 确认码: {confirmationCode}");

                return confirmationCode;
            }
            catch (Exception ex)
            {
                LogOperation("解绑失败", ex.Message);
                return null;
            }
        }

        public static bool CheckConfirmationCode(string confirmation)
        {
            try
            {
                if (!File.Exists(INVALIDATE_RECORD))
                {
                    return false;
                }

                var invalidateInfo = JsonConvert.DeserializeObject<dynamic>(
                    File.ReadAllText(INVALIDATE_RECORD));

                bool isValid = invalidateInfo.ConfirmationCode == confirmation &&
                             invalidateInfo.HardwareID == Reactor.Status.HardwareID;

                LogOperation("确认码验证", $"硬件ID: {Reactor.Status.HardwareID}, 结果: {isValid}");

                return isValid;
            }
            catch (Exception ex)
            {
                LogOperation("确认码验证失败", ex.Message);
                return false;
            }
        }

        public static bool ReactivateLicense(string code)
        {
            try
            {
                // 验证激活码
                if (!ValidateActivationCode(code))
                {
                    return false;
                }

                // 检查解绑记录
                if (!File.Exists(INVALIDATE_RECORD))
                {
                    return false;
                }

                var invalidateInfo = JsonConvert.DeserializeObject<dynamic>(
                    File.ReadAllText(INVALIDATE_RECORD));

                // 更新当前硬件ID
                var type = typeof(Reactor.Status);
                type.GetField("hid", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, Reactor.Status.GetHardwareID(true, true, true, true));

                // 验证硬件ID
                if (invalidateInfo.HardwareID != Reactor.Status.HardwareID)
                {
                    return false;
                }

                // 恢复授权文件
                string backupFilePath = invalidateInfo.BackupFile;
                if (string.IsNullOrEmpty(backupFilePath) || !File.Exists(backupFilePath))
                {
                    return false;
                }

                try
                {
                    File.Copy(backupFilePath, Config.LicenseFile, true);
                    File.Delete(backupFilePath);
                }
                catch (Exception ex)
                {
                    return false;
                }

                // 删除解绑记录
                File.Delete(INVALIDATE_RECORD);

                // 重新加载授权
                bool success = LoadLicense();

                // 记录操作日志
                LogOperation("激活", $"硬件ID: {Reactor.Status.HardwareID}, 结果: {success}");

                return success;
            }
            catch (Exception ex)
            {
                LogOperation("激活失败", ex.Message);
                return false;
            }
        }

        private static string GenerateConfirmationCode()
        {
            // 基于硬件ID和时间戳生成确认码
            string input = $"{Reactor.Status.HardwareID}_{DateTime.Now.Ticks}";
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").Substring(0, 16);
            }
        }

        private static bool ValidateActivationCode(string code)
        {
            // 实现激活码验证逻辑
            return !string.IsNullOrEmpty(code) && code.Length == 16;
        }

        private static bool CanInvalidate()
        {
            try
            {
                if (!File.Exists(INVALIDATE_ATTEMPTS_FILE))
                    return true;

                var attempts = JsonConvert.DeserializeObject<InvalidateAttempts>(
                    File.ReadAllText(INVALIDATE_ATTEMPTS_FILE));

                if (attempts.Count >= MAX_INVALIDATE_ATTEMPTS)
                {
                    if (DateTime.Now - attempts.LastAttempt < INVALIDATE_COOLDOWN)
                    {
                        SEnvir.Log($"解绑操作被限制，请等待 {INVALIDATE_COOLDOWN.TotalHours} 小时后再试");
                        return false;
                    }
                    
                    // 重置计数
                    attempts.Count = 0;
                }

                return true;
            }
            catch
            {
                return true;
            }
        }

        private static void RecordInvalidateAttempt()
        {
            try
            {
                var attempts = new InvalidateAttempts
                {
                    Count = 1,
                    LastAttempt = DateTime.Now
                };

                if (File.Exists(INVALIDATE_ATTEMPTS_FILE))
                {
                    var existing = JsonConvert.DeserializeObject<InvalidateAttempts>(
                        File.ReadAllText(INVALIDATE_ATTEMPTS_FILE));
                    attempts.Count = existing.Count + 1;
                    attempts.LastAttempt = DateTime.Now;
                }

                File.WriteAllText(INVALIDATE_ATTEMPTS_FILE, 
                    JsonConvert.SerializeObject(attempts));
            }
            catch
            {
                // 记录失败不影响主流程
            }
        }

        private static void LogOperation(string operation, string details)
        {
            try
            {
                string logEntry = $"[{DateTime.Now}] {operation}: {details}";
                File.AppendAllText("license_operations.log", logEntry + Environment.NewLine);
            }
            catch
            {
                // 日志记录失败不影响主流程
            }
        }

        public static bool LoadLicense()
        {
            try
            {
                if (!File.Exists(Config.LicenseFile))
                {
                    SEnvir.Log("授权文件不存在");
                    return false;
                }

                byte[] licenseData = File.ReadAllBytes(Config.LicenseFile);
                if (licenseData == null || licenseData.Length == 0)
                {
                    SEnvir.Log("授权文件为空");
                    return false;
                }

                // 解密授权数据
                string decryptedData = DecryptLicenseData(licenseData);
                if (string.IsNullOrEmpty(decryptedData))
                {
                    SEnvir.Log("授权数据解密失败");
                    return false;
                }

                // 解析授权信息
                var licenseInfo = JsonConvert.DeserializeObject<Reactor.LicenseGenerator.LicenseInfo>(decryptedData);
                if (licenseInfo == null)
                {
                    SEnvir.Log("授权信息解析失败");
                    return false;
                }

                // 更新当前硬件ID
                var type = typeof(Reactor.Status);
                type.GetField("hid", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, Reactor.Status.GetHardwareID(true, true, true, true));

                // 更新授权状态
                Reactor.Status.UpdateLicenseInfo(licenseInfo, null);
                SEnvir.Log("授权加载成功");

                return true;
            }
            catch (Exception ex)
            {
                SEnvir.Log($"加载授权失败: {ex.Message}");
                return false;
            }
        }

        private static string DecryptLicenseData(byte[] encryptedData)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] key = new byte[32];
                    byte[] iv = new byte[16];
                    
                    // 使用固定的密钥和IV
                    string keyStr = "ZirconLicenseKey1234567890123456789012345678";
                    string ivStr = "ZirconLicenseIV123";
                    
                    // 确保密钥和IV长度正确
                    Array.Copy(Encoding.UTF8.GetBytes(keyStr), key, Math.Min(keyStr.Length, 32));
                    Array.Copy(Encoding.UTF8.GetBytes(ivStr), iv, Math.Min(ivStr.Length, 16));
                    
                    aes.Key = key;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(encryptedData, 0, encryptedData.Length);
                            cs.FlushFinalBlock();
                        }

                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                SEnvir.Log($"解密授权数据失败: {ex.Message}");
                return null;
            }
        }
    }
}

namespace Server
{

    /// <summary>
    /// 授权状态
    /// </summary>
    public enum LicenseState
    {
        [Description("无效")]
        Invalid = 0,
        [Description("找不到授权文件")]
        Missing,
        [Description("已过期")]
        Expired,
        [Description("硬件码不符")]
        HardwareNotMatched,
        [Description("试用到期")]
        EvaluationExpired,
        [Description("超过使用次数")]
        UsesExceeded,

        [Description("有效")]
        Licensed = 101,
        [Description("Debug模式无需授权")]
        DebugMode = 102
    }
}
