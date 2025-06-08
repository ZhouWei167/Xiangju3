using System;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Security.Cryptography;

namespace Server.Web.Models.License
{
    public class Status
    {
        public static string GetHardwareID(bool Board, bool CPU, bool HDD, bool MAC)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder debugInfo = new StringBuilder();
            
            debugInfo.AppendLine("开始获取硬件ID");
            
            if (Board)
            {
                try
                {
                    debugInfo.AppendLine("尝试获取主板序列号");
                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard"))
                    {
                        foreach (ManagementObject obj in searcher.Get())
                        {
                            string serial = obj["SerialNumber"]?.ToString() ?? "";
                            if (!string.IsNullOrEmpty(serial))
                            {
                                sb.Append(serial);
                                debugInfo.AppendLine($"获取到主板序列号: {serial}");
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex) 
                {
                    debugInfo.AppendLine($"获取主板序列号失败: {ex.Message}");
                }
            }

            if (CPU)
            {
                try
                {
                    debugInfo.AppendLine("尝试获取CPU ID");
                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor"))
                    {
                        foreach (ManagementObject obj in searcher.Get())
                        {
                            string processorId = obj["ProcessorId"]?.ToString() ?? "";
                            if (!string.IsNullOrEmpty(processorId))
                            {
                                sb.Append(processorId);
                                debugInfo.AppendLine($"获取到CPU ID: {processorId}");
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex) 
                {
                    debugInfo.AppendLine($"获取CPU ID失败: {ex.Message}");
                }
            }

            if (HDD)
            {
                try
                {
                    debugInfo.AppendLine("尝试获取硬盘序列号");
                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_DiskDrive"))
                    {
                        foreach (ManagementObject obj in searcher.Get())
                        {
                            string serial = obj["SerialNumber"]?.ToString() ?? "";
                            if (!string.IsNullOrEmpty(serial))
                            {
                                sb.Append(serial);
                                debugInfo.AppendLine($"获取到硬盘序列号: {serial}");
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex) 
                {
                    debugInfo.AppendLine($"获取硬盘序列号失败: {ex.Message}");
                }
            }

            if (MAC)
            {
                try
                {
                    debugInfo.AppendLine("尝试获取MAC地址");
                    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                    foreach (NetworkInterface adapter in nics)
                    {
                        if (adapter.OperationalStatus == OperationalStatus.Up)
                        {
                            string mac = adapter.GetPhysicalAddress().ToString();
                            if (!string.IsNullOrEmpty(mac))
                            {
                                sb.Append(mac);
                                debugInfo.AppendLine($"获取到MAC地址: {mac}");
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex) 
                {
                    debugInfo.AppendLine($"获取MAC地址失败: {ex.Message}");
                }
            }

            // 生成一个基于硬件信息的唯一ID
            string hardwareInfo = sb.ToString();
            debugInfo.AppendLine($"收集到的原始硬件信息: {hardwareInfo}");
            
            if (string.IsNullOrEmpty(hardwareInfo))
            {
                // 如果无法获取硬件信息，返回默认ID
                string defaultId = "####-####-####-####-####";
                debugInfo.AppendLine($"未能获取硬件信息，返回默认ID: {defaultId}");
                Console.WriteLine(debugInfo.ToString());
                return defaultId;
            }

            debugInfo.AppendLine($"获取到硬件信息: {hardwareInfo}");
            
            // 使用MD5生成固定长度的硬件ID，与原始项目保持一致
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(hardwareInfo);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                
                // 将字节数组转换为十六进制字符串
                StringBuilder result = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    result.Append(hashBytes[i].ToString("X2"));
                }
                
                // 格式化输出为 ####-####-####-####-#### 格式
                string hash = result.ToString();
                string formattedId = $"{hash.Substring(0, 4)}-{hash.Substring(4, 4)}-{hash.Substring(8, 4)}-{hash.Substring(12, 4)}-{hash.Substring(16, 4)}";
                debugInfo.AppendLine($"生成的硬件ID: {formattedId}");
                Console.WriteLine(debugInfo.ToString());
                return formattedId;
            }
        }
    }
} 