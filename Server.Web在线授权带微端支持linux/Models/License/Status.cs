using System;
using System.Text;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.InteropServices;

namespace Server.Web.Models.License
{
    public class Status
    {
        public static string GetHardwareID(bool Board, bool CPU, bool HDD, bool MAC)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder debugInfo = new StringBuilder();
            
            debugInfo.AppendLine("开始获取硬件ID");
            
            // 检查操作系统
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            debugInfo.AppendLine($"当前操作系统: {(isWindows ? "Windows" : "Linux/其他")}");
            
            if (Board)
            {
                try
                {
                    debugInfo.AppendLine("尝试获取主板序列号");
                    if (isWindows)
                    {
                        #if WINDOWS
                        using (var searcher = new System.Management.ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard"))
                        {
                            foreach (System.Management.ManagementObject obj in searcher.Get())
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
                        #endif
                    }
                    else
                    {
                        // Linux下获取主板序列号
                        string serial = ReadLinuxSystemFile("/sys/class/dmi/id/board_serial");
                        if (!string.IsNullOrEmpty(serial))
                        {
                            sb.Append(serial);
                            debugInfo.AppendLine($"获取到主板序列号: {serial}");
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
                    if (isWindows)
                    {
                        #if WINDOWS
                        using (var searcher = new System.Management.ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor"))
                        {
                            foreach (System.Management.ManagementObject obj in searcher.Get())
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
                        #endif
                    }
                    else
                    {
                        // Linux下获取CPU信息
                        string cpuInfo = ReadLinuxSystemFile("/proc/cpuinfo");
                        if (!string.IsNullOrEmpty(cpuInfo))
                        {
                            // 提取处理器ID或序列号
                            string[] lines = cpuInfo.Split('\n');
                            foreach (string line in lines)
                            {
                                if (line.StartsWith("processor") || line.StartsWith("serial") || line.StartsWith("cpu family"))
                                {
                                    sb.Append(line);
                                    debugInfo.AppendLine($"获取到CPU信息: {line}");
                                    break;
                                }
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
                    if (isWindows)
                    {
                        #if WINDOWS
                        using (var searcher = new System.Management.ManagementObjectSearcher("SELECT SerialNumber FROM Win32_DiskDrive"))
                        {
                            foreach (System.Management.ManagementObject obj in searcher.Get())
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
                        #endif
                    }
                    else
                    {
                        // Linux下获取硬盘信息
                        try
                        {
                            // 尝试使用lsblk命令获取
                            string diskInfo = ExecuteCommand("lsblk --nodeps -no serial");
                            if (!string.IsNullOrEmpty(diskInfo))
                            {
                                sb.Append(diskInfo.Trim());
                                debugInfo.AppendLine($"获取到硬盘序列号: {diskInfo.Trim()}");
                            }
                        }
                        catch
                        {
                            // 如果失败，尝试从/dev/disk/by-id读取
                            try
                            {
                                if (Directory.Exists("/dev/disk/by-id"))
                                {
                                    string[] diskIds = Directory.GetFiles("/dev/disk/by-id");
                                    if (diskIds.Length > 0)
                                    {
                                        string diskId = Path.GetFileName(diskIds[0]);
                                        sb.Append(diskId);
                                        debugInfo.AppendLine($"获取到硬盘ID: {diskId}");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                debugInfo.AppendLine($"获取硬盘ID失败: {ex.Message}");
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
        
        // 读取Linux系统文件
        private static string ReadLinuxSystemFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    return File.ReadAllText(path).Trim();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"读取系统文件失败 {path}: {ex.Message}");
            }
            return string.Empty;
        }
        
        // 执行Shell命令并获取输出
        private static string ExecuteCommand(string command)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = "/bin/bash";
                process.StartInfo.Arguments = $"-c \"{command}\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return output;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"执行命令失败 {command}: {ex.Message}");
                return string.Empty;
            }
        }
    }
} 