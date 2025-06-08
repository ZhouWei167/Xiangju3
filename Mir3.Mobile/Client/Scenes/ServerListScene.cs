using Client.Controls;
using Client.Envir;
using Library;
using MonoGame.Extended;
using MonoGame.Extended.Input;
using Patch;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Client.Scenes
{
    /// <summary>
    /// 启动游戏
    /// </summary>
    public class ServerListScene : DXScene
    {
        private static readonly string KEY = "YourSecretKey123"; // 16字节密钥
        private static readonly string IV = "YourIVKey1234567";  // 16字节IV

        DXImageControl Background;
        DXLabel StatusLabel;
        public DXComboBox LISTComboBox;
        public DXButton ncbutton, okbutton, refreshButton;
        public List<ServerInfo> ServerList = new List<ServerInfo>();
        public bool IsLoading = false;

        /// <summary>
        /// 服务器信息类
        /// </summary>
        public class ServerInfo
        {
            public string Name { get; set; }
            public string IP { get; set; }
            public int Port { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        /// <summary>
        /// 服务器列表数据类
        /// </summary>
        private class ServerListData
        {
            public List<ServerInfo> Servers { get; set; }
        }

        private async Task LISTGET()
        {
            try
            {
                // 创建下拉框
                LISTComboBox = new DXComboBox()
                {
                    Parent = ncbutton,
                    BackColour = Color.Blue,
                    Location = new Point(12, 8),
                    Size = new Size(150, DXComboBox.DefaultNormalHeight),
                    Border = true,
                    BorderColour = Color.FromArgb(198, 166, 99),
                    Visible = true,
                    Enabled = false
                };

                await LoadServerList();
            }
            catch
            {
                // 出现异常时直接使用默认服务器列表
                List<ServerInfo> defaultList = new List<ServerInfo>
                {
                    new ServerInfo { Name = "默认服务器", IP = "127.0.0.1", Port = 7000 }
                };
                LoadServerListToComboBox(defaultList);

                // 更新状态标签
                if (StatusLabel != null)
                {
                    StatusLabel.Text = "加载失败，使用默认服务器";
                    StatusLabel.ForeColour = Color.Red;
                }
            }
        }

        /// <summary>
        /// 加载服务器列表
        /// </summary>
        private async Task LoadServerList()
        {
            // 更新状态标签
            if (StatusLabel != null)
            {
                StatusLabel.Text = "正在加载服务器列表...";
                StatusLabel.ForeColour = Color.Yellow;
            }

            // 尝试从网络获取服务器列表
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(5); // 设置5秒超时
                string serverListUrl = $"{Config.Host}ServerList.json"; // 使用配置中的主机地址

                try
                {
                    // 获取JSON内容
                    string content = await client.GetStringAsync(serverListUrl);

                    // 优先尝试解密后解析
                    try
                    {
                        // 解密JSON内容
                        string decryptedContent = DecryptString(content);
                        var serverListData = JsonConvert.DeserializeObject<ServerListData>(decryptedContent);
                        if (serverListData != null && serverListData.Servers != null)
                        {
                            LoadServerListToComboBox(serverListData.Servers);
                            return;
                        }
                    }
                    catch
                    {
                        // 如果解密失败，尝试直接解析未加密的JSON
                        try
                        {
                            var serverListData = JsonConvert.DeserializeObject<ServerListData>(content);
                            if (serverListData != null && serverListData.Servers != null)
                            {
                                LoadServerListToComboBox(serverListData.Servers);
                                return;
                            }
                        }
                        catch (Exception parseEx)
                        {
                            // 直接解析也失败，抛出异常以进入下一个处理流程
                            throw new Exception("无法解析服务器列表数据", parseEx);
                        }
                    }

                    // 如果解析失败，抛出异常以进入下一个处理流程
                    throw new Exception("服务器列表数据格式不正确");
                }
                catch
                {
                    // 如果网络请求失败，尝试从本地文件加载
                    string serverListPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "ServerList.json");

                    if (File.Exists(serverListPath))
                    {
                        string localJsonContent = await Task.Run(() => File.ReadAllText(serverListPath));
                        var localServerListData = JsonConvert.DeserializeObject<ServerListData>(localJsonContent);
                        LoadServerListToComboBox(localServerListData.Servers);
                    }
                    else
                    {
                        // 如果本地文件也不存在，直接使用默认服务器列表
                        List<ServerInfo> defaultList = new List<ServerInfo>
                        {
                            new ServerInfo { Name = "默认服务器", IP = "127.0.0.1", Port = 7000 }
                        };
                        LoadServerListToComboBox(defaultList);

                        // 更新状态标签
                        if (StatusLabel != null)
                        {
                            StatusLabel.Text = "使用默认服务器列表";
                            StatusLabel.ForeColour = Color.Orange;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 将服务器列表加载到下拉框
        /// </summary>
        private void LoadServerListToComboBox(List<ServerInfo> serverList)
        {
            if (serverList == null || serverList.Count == 0)
            {
                serverList = new List<ServerInfo>
                {
                    new ServerInfo { Name = "默认服务器", IP = "127.0.0.1", Port = 7000 }
                };
            }

            // 清除下拉框内容和事件处理
            LISTComboBox.ListBox.Controls.Clear();
            LISTComboBox.ListBox.selectedItemChanged -= ServerItemChanged;

            foreach (var server in serverList)
            {
                new DXListBoxItem
                {
                    Parent = LISTComboBox.ListBox,
                    BackColour = Color.Black,
                    Label = { Text = server.Name },
                    Size = new Size(150, DXComboBox.DefaultNormalHeight),
                    Item = server,
                };
            }

            // 选择默认服务器
            int defaultIndex = Math.Min(Config.SVID, serverList.Count - 1);
            if (defaultIndex < 0) defaultIndex = 0;

            // 添加事件处理
            LISTComboBox.ListBox.selectedItemChanged += ServerItemChanged;

            if (serverList.Count > 0)
            {
                LISTComboBox.ListBox.SelectItem(serverList[defaultIndex]);
                Config.IPAddress = serverList[defaultIndex].IP;
                Config.Port = serverList[defaultIndex].Port;
                LISTComboBox.Enabled = true;

                // 更新状态标签
                if (StatusLabel != null)
                {
                    StatusLabel.Text = "服务器列表加载成功";
                    StatusLabel.ForeColour = Color.Green;
                }
            }
        }

        /// <summary>
        /// 服务器选项改变事件处理
        /// </summary>
        private void ServerItemChanged(object sender, EventArgs e)
        {
            if (LISTComboBox.SelectedItem is ServerInfo item)
            {
                // 设置配置参数
                Config.IPAddress = item.IP;
                Config.Port = item.Port;

                // 更新选择的索引
                var serverList = new List<ServerInfo>();
                foreach (DXListBoxItem listItem in LISTComboBox.ListBox.Controls)
                {
                    if (listItem.Item is ServerInfo server)
                    {
                        serverList.Add(server);
                    }
                }

                for (int i = 0; i < serverList.Count; i++)
                {
                    if (serverList[i].Name == item.Name && serverList[i].IP == item.IP)
                    {
                        Config.SVID = i;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        private static string DecryptString(string cipherText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(KEY);
                aes.IV = Encoding.UTF8.GetBytes(IV);

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }

        private static async Task<string> DownloadFileWithRetryAsync(string url, int maxRetries = 3, int timeoutMs = 5000)
        {
            using HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(timeoutMs);

            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    return await client.GetStringAsync(url);
                }
                catch (TaskCanceledException) when (i < maxRetries - 1)
                {
                    Console.WriteLine($"请求超时，正在重试 ({i + 1}/{maxRetries})...");
                }
                catch (HttpRequestException ex)
                {
                    if (i == maxRetries - 1)
                        throw new Exception("请求失败（超过最大重试次数）", ex);
                }
            }
            throw new HttpRequestException("请求失败（未知错误）");
        }
        /// <summary>
        /// 启动游戏窗口
        /// </summary>
        /// <param name="size">视窗大小</param>
        public ServerListScene(Size size) : base(size)
        {
            IsControl = false;

            //var offset = Game1.Native.IsPad ? 1 : 0;
            var offset = CEnvir.Random.Next(0, 2);
            Background = new DXImageControl
            {
                Index = 47 + offset,
                LibraryFile = LibraryFile.StartMobileScene,
                IsControl = false,
                Parent = this
            };
            Background.Location = new Point((size.Width - Background.Size.Width) / 2, (size.Height - Background.Size.Height) / 2);

            ncbutton = new DXButton
            {
                Index = 53,
                LibraryFile = LibraryFile.Interface1c,
                Location = new Point(60, 50),
                Parent = this,
                CanBePressed = false,
                //Enabled = true,
            };

            // 创建并设置 okbutton
            okbutton = new DXButton
            {
                LibraryFile = LibraryFile.Interface1c,
                Index = 55,
                Parent = this,
                Location = new Point(360, 50), // 位置调整
                Size = new Size(100, 30),
            };
            okbutton.MouseClick += OkButton_MouseClick; // 使用命名方法

            // 创建刷新按钮
            refreshButton = new DXButton
            {
                LibraryFile = LibraryFile.GameInter,
                Index = 5685, // 使用适当的图标索引
                Parent = this,
                Location = new Point(250, 50), // 位置在服务器列表和确定按钮之间
                Size = new Size(30, 30),
                Hint = "刷新服务器列表"
            };
            refreshButton.MouseClick += RefreshButton_MouseClick; // 使用命名方法

            // 状态标签
            StatusLabel = new DXLabel
            {
                Parent = this,
                Location = new Point(60, 30),
                Size = new Size(300, 20),
                Text = "正在加载服务器列表...",
                ForeColour = Color.Yellow
            };

            _ = LISTGET(); // 不等待任务完成的异步调用
        }

        // 定义命名方法处理按钮点击
        private void OkButton_MouseClick(object sender, MouseEventArgs e)
        {
            NextSecene();
        }

        private async void RefreshButton_MouseClick(object sender, MouseEventArgs e)
        {
            await RefreshServerList();
        }

        /// <summary>
        /// 刷新服务器列表
        /// </summary>
        private async Task RefreshServerList()
        {
            if (IsLoading) return; // 如果正在加载，则忽略请求

            IsLoading = true;
            refreshButton.Enabled = false; // 禁用按钮防止重复点击

            try
            {
                // 重置选择，避免刷新后保持原来的选择
                LISTComboBox.SelectedItem = null;

                // 重置SVID，这样LoadServerListToComboBox会选择第一项
                Config.SVID = 0;

                await LoadServerList();
            }
            finally
            {
                IsLoading = false;
                refreshButton.Enabled = true; // 重新启用按钮
            }
        }

        /// <summary>
        /// 下一个场景选区场景
        /// </summary>
        void NextSecene()
        {
            try
            {
                // 先移除事件处理器，避免事件引用导致的问题
                if (okbutton != null)
                    okbutton.MouseClick -= OkButton_MouseClick;

                if (refreshButton != null)
                    refreshButton.MouseClick -= RefreshButton_MouseClick;

                // 创建新场景
                CEnvir.UpdateScale(true);
                var nextScene = new LoginScene(CEnvir.GameSize);
                ActiveScene = nextScene;

                // 安全释放资源
                Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"切换场景时出错: {ex.Message}");
                // 确保即使出错也能切换场景
                ActiveScene = new LoginScene(CEnvir.GameSize);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    // 移除所有事件处理器
                    if (okbutton != null)
                        okbutton.MouseClick -= OkButton_MouseClick;

                    if (refreshButton != null)
                        refreshButton.MouseClick -= RefreshButton_MouseClick;

                    // 先释放ComboBox中的ListBox，避免空引用
                    if (LISTComboBox != null)
                    {
                        if (LISTComboBox.ListBox != null)
                        {
                            LISTComboBox.ListBox.selectedItemChanged -= ServerItemChanged;
                            LISTComboBox.ListBox.Visible = false;
                        }

                        // 从静态列表中移除
                        DXComboBox.ComboBoxes.Remove(LISTComboBox);
                    }

                    // 释放控件资源
                    StatusLabel?.TryDispose();
                    LISTComboBox?.TryDispose();
                    ncbutton?.TryDispose();
                    okbutton?.TryDispose();
                    refreshButton?.TryDispose();
                    Background?.TryDispose();

                    // 清空引用
                    StatusLabel = null;
                    LISTComboBox = null;
                    ncbutton = null;
                    okbutton = null;
                    refreshButton = null;
                    Background = null;
                    ServerList = null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"释放资源时出错: {ex.Message}");
                }
            }

            // 最后调用基类的Dispose
            base.Dispose(disposing);
        }
    }
}
