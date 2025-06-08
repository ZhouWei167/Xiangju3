using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace Server.Web.Models.License
{
    public class LicenseGenerator
    {
        // 使用与Server端DLL相同的密钥
        private const string KEY = "ZirconLicenseKey1234567890123456789012345678"; // 32字节
        private const string IV = "ZirconLicenseIV123";  // 16字节

        public class LicenseInfo
        {
            public string HardwareID { get; set; }
            public DateTime ExpirationDate { get; set; }
            public int NumberOfUses { get; set; }
            public int NumberOfInstances { get; set; }
            public bool EnableEvaluationLock { get; set; }
            public bool EnableExpirationDateLock { get; set; }
            public bool EnableNumberOfUsesLock { get; set; }
            public bool EnableNumberOfInstancesLock { get; set; }
            public bool EnableHardwareLock { get; set; }
            public int EvaluationTime { get; set; }
            public EvaluationType EvaluationType { get; set; }
            public Dictionary<string, string> CustomData { get; set; }
            public string Version { get; set; }
            public string CustomerName { get; set; }
            public string ProductName { get; set; }
            public DateTime IssueDate { get; set; }

            public LicenseInfo()
            {
                CustomData = new Dictionary<string, string>();
            }

            public void Validate()
            {
                if (string.IsNullOrEmpty(HardwareID))
                    throw new Exception("硬件ID不能为空");
                    
                if (EnableExpirationDateLock && ExpirationDate <= DateTime.Now)
                    throw new Exception("过期时间必须大于当前时间");
                    
                if (EnableNumberOfUsesLock && NumberOfUses <= 0)
                    throw new Exception("启用使用次数限制时，使用次数必须大于0");
                    
                if (EnableNumberOfInstancesLock && NumberOfInstances <= 0)
                    throw new Exception("启用实例数量限制时，实例数量必须大于0");
                    
                if (EnableEvaluationLock && EvaluationTime <= 0)
                    throw new Exception("启用试用期限制时，试用时间必须大于0");
            }
        }

        public static void GenerateLicenseFile(LicenseInfo licenseInfo, string outputPath)
        {
            try
            {
                // 验证授权信息
                licenseInfo.Validate();

                // 确保输出路径使用正确的扩展名
                if (!outputPath.EndsWith(".license", StringComparison.OrdinalIgnoreCase))
                {
                    outputPath = Path.ChangeExtension(outputPath, ".license");
                }

                // 将许可证信息转换为JSON格式
                string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(licenseInfo);

                // 加密数据
                byte[] encryptedData = EncryptString(jsonData);

                // 保存到文件
                File.WriteAllBytes(outputPath, encryptedData);
            }
            catch (Exception ex)
            {
                throw new Exception("生成许可证文件失败: " + ex.Message);
            }
        }

        public static LicenseInfo LoadLicenseFile(string filePath)
        {
            try
            {
                // 确保文件扩展名正确
                if (!filePath.EndsWith(".license", StringComparison.OrdinalIgnoreCase))
                {
                    filePath = Path.ChangeExtension(filePath, ".license");
                }

                // 读取加密数据
                byte[] encryptedData = File.ReadAllBytes(filePath);

                // 解密数据
                string jsonData = DecryptBytes(encryptedData);

                // 反序列化JSON数据
                return Newtonsoft.Json.JsonConvert.DeserializeObject<LicenseInfo>(jsonData);
            }
            catch (Exception ex)
            {
                throw new Exception("加载许可证文件失败: " + ex.Message);
            }
        }

        private static byte[] EncryptString(string plainText)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] key = new byte[32];
                    byte[] iv = new byte[16];
                    
                    // 确保密钥和IV长度正确
                    Array.Copy(Encoding.UTF8.GetBytes(KEY), key, Math.Min(KEY.Length, 32));
                    Array.Copy(Encoding.UTF8.GetBytes(IV), iv, Math.Min(IV.Length, 16));
                    
                    aes.Key = key;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }

                        return msEncrypt.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"加密失败: {ex.Message}");
            }
        }

        private static string DecryptBytes(byte[] cipherText)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] key = new byte[32];
                    byte[] iv = new byte[16];
                    
                    // 确保密钥和IV长度正确
                    Array.Copy(Encoding.UTF8.GetBytes(KEY), key, Math.Min(KEY.Length, 32));
                    Array.Copy(Encoding.UTF8.GetBytes(IV), iv, Math.Min(IV.Length, 16));
                    
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
                throw new Exception($"解密失败: {ex.Message}");
            }
        }
    }
} 