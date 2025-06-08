using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Net.NetworkInformation;

namespace HardwareIDApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("硬件ID获取程序");
            Console.WriteLine("====================");
            
            // 使用与Zircon Server完全相同的方法获取硬件ID
            string zirconHardwareId = GetZirconHardwareID(true, true, true, true);
            Console.WriteLine($"唯一硬件ID: {zirconHardwareId}");
            
            // 将硬件ID保存到剪贴板
            try
            {
                System.Windows.Forms.Clipboard.SetText(zirconHardwareId);
                Console.WriteLine("\n硬件ID已复制到剪贴板！");
            }
            catch
            {
                Console.WriteLine("\n无法复制到剪贴板，请手动复制上面的硬件ID。");
            }
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
        
        // 使用与Zircon Server完全相同的方法获取硬件ID
        public static string GetZirconHardwareID(bool Board, bool CPU, bool HDD, bool MAC)
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
    }
} 