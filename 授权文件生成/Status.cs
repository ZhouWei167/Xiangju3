using System;
using System.Collections;
using System.IO;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Net.NetworkInformation;

namespace License
{
    public class Status
    {
        public static event InvalidateLicenseEventHandler InvalidateLicenseEvent;
        public static event CheckConfirmationCodeEventHandler CheckConfirmationCodeEvent;
        public static event LoadLicenseEventHandler LoadLicenseEvent;
        public static event ReactivateLicenseEventHandler ReactivateLicenseEvent;

        private static bool licensed; // 是否已授权，由授权文件指定
        private static bool eva_lock; // 试用锁定，由授权文件指定
        private static bool date_lock; // 过期时间锁定，由授权文件指定
        private static bool uses_lock; // 使用次数锁定，由授权文件指定
        private static bool instances_lock; // 实例数锁定，由授权文件指定
        private static bool hardware_lock; // 硬件锁定，由授权文件指定
        private static int eva_time;  // 试用时间限制，由授权文件指定
        private static int eva_time_current;  // 当前试用时间，由授权文件指定
        private static string hid = "####-####-####-####-####";  // 当前硬件ID
        private static string lhid; // 授权文件中的硬件ID，由授权文件指定
        private static int uses;  // 使用次数限制，由授权文件指定
        private static int instances;  // 实例数限制，由授权文件指定
        private static int uses_current;  // 当前使用次数，由授权文件指定
        private static EvaluationType eva_type;  // 试用类型，由授权文件指定
        private static DateTime license_exp_date;  // 授权过期时间，由授权文件指定
        private static SortedList key_n_value = new SortedList();  // 自定义键值对，由授权文件指定
        private static byte[] lic;  // 授权文件内容，由授权文件指定
        private static bool licenseFileExists;  // 授权文件是否存在，由授权文件指定

        public static void UpdateLicenseInfo(LicenseGenerator.LicenseInfo licenseInfo, byte[] licenseData)
        {
            licensed = true;
            eva_lock = licenseInfo.EnableEvaluationLock;
            date_lock = licenseInfo.EnableExpirationDateLock;
            uses_lock = licenseInfo.EnableNumberOfUsesLock;
            instances_lock = licenseInfo.EnableNumberOfInstancesLock;
            hardware_lock = licenseInfo.EnableHardwareLock;
            eva_time = licenseInfo.EvaluationTime;
            eva_type = licenseInfo.EvaluationType;
            lhid = licenseInfo.HardwareID;
            uses = licenseInfo.NumberOfUses;
            instances = licenseInfo.NumberOfInstances;
            license_exp_date = licenseInfo.ExpirationDate;
            lic = licenseData;

            // 更新自定义数据
            key_n_value.Clear();
            foreach (var kvp in licenseInfo.CustomData)
            {
                key_n_value[kvp.Key] = kvp.Value;
            }
        }

        public static string InvalidateLicense()
        {
            if (InvalidateLicenseEvent != null)
            {
                return InvalidateLicenseEvent();
            }
            return "";
        }

        public static bool CheckConfirmationCode(string harwareID, string confirmationCode)
        {
            return CheckConfirmationCodeEvent != null && CheckConfirmationCodeEvent(harwareID, confirmationCode);
        }

        public static void LoadLicense(byte[] license)
        {
            if (LoadLicenseEvent != null)
            {
                LoadLicenseEvent(license);
            }
        }

        public static bool ReactivateLicense(string code)
        {
            return ReactivateLicenseEvent != null && ReactivateLicenseEvent(code);
        }

        public static void LoadLicense(string filename)
        {
            if (Status.LoadLicenseEvent != null)
            {
                // 确保文件扩展名正确
                if (!filename.EndsWith(".license", StringComparison.OrdinalIgnoreCase))
                {
                    filename = Path.ChangeExtension(filename, ".license");
                }

                try
                {
                    licenseFileExists = File.Exists(filename);
                    if (!licenseFileExists)
                    {
                        throw new FileNotFoundException("授权文件不存在", filename);
                    }

                    using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    {
                        byte[] array = new byte[fileStream.Length];
                        fileStream.Read(array, 0, array.Length);
                        Status.LoadLicense(array);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"加载授权文件失败: {ex.Message}");
                }
            }
        }

        public static bool Expired
        {
            get { return DateTime.Now > license_exp_date; }
        }

        public static bool HardwareNotMatched
        {
            get { return hardware_lock && hid != lhid; }
        }

        public static bool Licensed
        {
            get { return licensed; }
        }

        public static string HardwareID
        {
            get { return hid; }
        }

        public static string License_HardwareID
        {
            get { return lhid; }
        }

        public static bool Evaluation_Lock_Enabled
        {
            get { return eva_lock; }
        }

        public static bool Expiration_Date_Lock_Enable
        {
            get { return date_lock; }
        }

        public static bool Number_Of_Uses_Lock_Enable
        {
            get { return uses_lock; }
        }

        public static bool Number_Of_Instances_Lock_Enable
        {
            get { return instances_lock; }
        }

        public static bool Hardware_Lock_Enabled
        {
            get { return hardware_lock; }
        }

        public static int Evaluation_Time
        {
            get { return eva_time; }
        }

        public static int Evaluation_Time_Current
        {
            get { return eva_time_current; }
        }

        public static int Number_Of_Uses_Current
        {
            get { return uses_current; }
        }

        public static int Number_Of_Uses
        {
            get { return uses; }
        }

        public static int Number_Of_Instances
        {
            get { return instances; }
        }

        public static EvaluationType Evaluation_Type
        {
            get { return eva_type; }
        }

        public static byte[] License
        {
            get { return lic; }
        }

        public static DateTime ExpirationDate
        {
            get { return license_exp_date; }
        }

        public static SortedList KeyValueList
        {
            get { return key_n_value; }
        }

        public static bool LicenseFileExists
        {
            get { return licenseFileExists; }
        }

        private Status()
        {
        }

        public static string GetHardwareID(bool Board, bool CPU, bool HDD, bool MAC)
        {
            StringBuilder sb = new StringBuilder();
            
            if (Board)
            {
                try
                {
                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard"))
                    {
                        foreach (ManagementObject obj in searcher.Get())
                        {
                            string serial = obj["SerialNumber"]?.ToString() ?? "";
                            if (!string.IsNullOrEmpty(serial))
                            {
                                sb.Append(serial);
                                break;
                            }
                        }
                    }
                }
                catch { }
            }

            if (CPU)
            {
                try
                {
                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor"))
                    {
                        foreach (ManagementObject obj in searcher.Get())
                        {
                            string processorId = obj["ProcessorId"]?.ToString() ?? "";
                            if (!string.IsNullOrEmpty(processorId))
                            {
                                sb.Append(processorId);
                                break;
                            }
                        }
                    }
                }
                catch { }
            }

            if (HDD)
            {
                try
                {
                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_DiskDrive"))
                    {
                        foreach (ManagementObject obj in searcher.Get())
                        {
                            string serial = obj["SerialNumber"]?.ToString() ?? "";
                            if (!string.IsNullOrEmpty(serial))
                            {
                                sb.Append(serial);
                                break;
                            }
                        }
                    }
                }
                catch { }
            }

            if (MAC)
            {
                try
                {
                    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                    foreach (NetworkInterface adapter in nics)
                    {
                        if (adapter.OperationalStatus == OperationalStatus.Up)
                        {
                            string mac = adapter.GetPhysicalAddress().ToString();
                            if (!string.IsNullOrEmpty(mac))
                            {
                                sb.Append(mac);
                                break;
                            }
                        }
                    }
                }
                catch { }
            }

            if (sb.Length == 0)
            {
                return "####-####-####-####-####";
            }

            // 使用MD5生成固定长度的硬件ID
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(sb.ToString());
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                
                // 将字节数组转换为十六进制字符串
                StringBuilder result = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    result.Append(hashBytes[i].ToString("X2"));
                }
                
                // 格式化输出为 ####-####-####-####-#### 格式
                string hash = result.ToString();
                return $"{hash.Substring(0, 4)}-{hash.Substring(4, 4)}-{hash.Substring(8, 4)}-{hash.Substring(12, 4)}-{hash.Substring(16, 4)}";
            }
        }

        public delegate string InvalidateLicenseEventHandler();
        public delegate bool CheckConfirmationCodeEventHandler(string harwareID, string confirmationCode);
        public delegate void LoadLicenseEventHandler(byte[] license);
        public delegate bool ReactivateLicenseEventHandler(string code);
    }

    public enum EvaluationType
    {
        Trial_Days,
        Trial_Runs,
        Trial_Time
    }
} 