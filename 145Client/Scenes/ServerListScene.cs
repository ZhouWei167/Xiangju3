using Client.Controls;
using Client.Envir;
using Client.Extentions;
using Library;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Client.Scenes
{
    /// <summary>
    /// 服务器列表场景
    /// </summary>
    public class ServerListScene : DXScene
    {
        private static readonly string KEY = "YourSecretKey123"; // 16字节密钥
        private static readonly string IV = "YourIVKey1234567";  // 16字节IV

        #region Properties
        [AutoDispose]
        public DXImageControl Background;
        [AutoDispose]
        public DXComboBox ServerListComboBox;
        [AutoDispose]
        public DXButton ConnectButton;
        [AutoDispose]
        public DXButton RefreshButton;
        [AutoDispose]
        public DXLabel StatusLabel;
        public List<ServerInfo> ServerList = new List<ServerInfo>();
        public bool IsLoading = false;
        #endregion

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
        /// 服务器列表场景
        /// </summary>
        /// <param name="size">视窗大小</param>
        public ServerListScene(Size size) : base(size)
        {
            Background = new DXImageControl
            {
                Index = 7,
                LibraryFile = LibraryFile.Interface1c145,
                Parent = this,
                Location = new Point(0, 60),
            };

            ServerListComboBox = new DXComboBox
            {
                Parent = this,
                Location = new Point((Size.Width - 200) / 2, (Size.Height - 30) / 2),
                Size = new Size(200, 30),
                DropDownHeight = 200,
                Enabled = false
            };

            ConnectButton = new DXButton
            {
                Parent = this,
                Location = new Point((Size.Width - 100) / 2, ServerListComboBox.Location.Y + 40),
                Size = new Size(100, 30),
                LibraryFile = LibraryFile.GameInter1,
                Index = 92,
                Enabled = false,
                Hint = "开始游戏"
            };
            ConnectButton.MouseClick += (o, e) => ConnectToServer();

            RefreshButton = new DXButton
            {
                Parent = this,
                Location = new Point(ServerListComboBox.Location.X + ServerListComboBox.Size.Width + 10, ServerListComboBox.Location.Y),
                Size = new Size(30, 30),
                LibraryFile = LibraryFile.GameInter,
                Index = 767,
                Hint = "刷新服务器列表"
            };
            RefreshButton.MouseClick += (o, e) => LoadServerList();

            StatusLabel = new DXLabel
            {
                Parent = this,
                Location = new Point((Size.Width - 300) / 2, ServerListComboBox.Location.Y - 30),
                Size = new Size(300, 20),
                Text = "正在加载服务器列表...",
                ForeColour = Color.Yellow
            };

            // 加载服务器列表
            LoadServerList();
        }

        /// <summary>
        /// 加载服务器列表
        /// </summary>
        private async void LoadServerList()
        {
            try
            {
                IsLoading = true;
                StatusLabel.Text = "正在加载服务器列表...";
                StatusLabel.ForeColour = Color.Yellow;

                // 尝试从网络获取服务器列表
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(5); // 设置5秒超时
                    string serverListUrl = "http://59.153.167.90:39222/ServerList.json"; // 替换为实际的API地址

                    try
                    {
                        // 获取加密的JSON内容
                        string encryptedContent = await client.GetStringAsync(serverListUrl);

                        // 解密JSON内容
                        string decryptedContent = DecryptString(encryptedContent);

                        // 解析解密后的JSON
                        var serverListData = JsonConvert.DeserializeObject<ServerListData>(decryptedContent);
                        ServerList = serverListData.Servers;

                        StatusLabel.Text = "服务器列表加载成功";
                        StatusLabel.ForeColour = Color.Green;
                    }
                    catch (Exception ex)
                    {
                        // 如果网络请求失败，尝试从本地文件加载
                        string serverListPath = Path.Combine(Application.StartupPath, "Data", "ServerList.json");

                        if (File.Exists(serverListPath))
                        {
                            string localJsonContent = await Task.Run(() => File.ReadAllText(serverListPath));
                            var localServerListData = JsonConvert.DeserializeObject<ServerListData>(localJsonContent);
                            ServerList = localServerListData.Servers;

                            StatusLabel.Text = "使用本地服务器列表";
                            StatusLabel.ForeColour = Color.Orange;
                        }
                        else
                        {
                            // 如果本地文件也不存在，使用默认服务器列表
                            ServerList = new List<ServerInfo>
                            {
                                new ServerInfo
                                {
                                    Name = "本地服务器",
                                    IP = "127.0.0.1",
                                    Port = 7000
                                }
                            };

                            StatusLabel.Text = "使用默认服务器列表";
                            StatusLabel.ForeColour = Color.Orange;
                        }
                    }
                }

                // 添加服务器到下拉列表
                ServerListComboBox.ListBox.Controls.Clear();
                foreach (var server in ServerList)
                {
                    var item = new DXListBoxItem
                    {
                        Item = server,
                        Label = { Text = server.Name },
                        Parent = ServerListComboBox.ListBox
                    };
                }

                if (ServerList.Count > 0)
                {
                    ServerListComboBox.ListBox.SelectItem(ServerList[0]);
                    ServerListComboBox.Enabled = true;
                    ConnectButton.Enabled = true;
                }
                else
                {
                    StatusLabel.Text = "没有可用的服务器";
                    StatusLabel.ForeColour = Color.Red;
                }
            }
            catch (Exception ex)
            {
                StatusLabel.Text = "加载服务器列表失败: " + ex.Message;
                StatusLabel.ForeColour = Color.Red;
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        private string DecryptString(string cipherText)
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

        /// <summary>
        /// 连接到选中的服务器
        /// </summary>
        private void ConnectToServer()
        {
            if (ServerListComboBox.SelectedItem == null)
                return;

            ServerInfo selectedServer = (ServerInfo)ServerListComboBox.SelectedItem;

            // 设置服务器配置
            NetworkConfig.IPAddress = selectedServer.IP;
            NetworkConfig.Port = selectedServer.Port;

            // 切换到登录场景
            ActiveScene = new LoginScene(Config.IntroSceneSize);
            Dispose();
        }

        /// <summary>
        /// 服务器列表数据类
        /// </summary>
        private class ServerListData
        {
            public List<ServerInfo> Servers { get; set; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // 先将Parent设置为null
                Parent = null;

                // 先清空Controls集合
                if (Controls != null)
                {
                    while (Controls.Count > 0)
                    {
                        Controls[0].Dispose();
                    }
                }

                // 然后释放各个控件
                Background?.Dispose();
                ServerListComboBox?.Dispose();
                ConnectButton?.Dispose();
                RefreshButton?.Dispose();
                StatusLabel?.Dispose();

                // 处理静态字段
                if (DXControl.MouseControl == this)
                    DXControl.MouseControl = null;
                if (DXControl.FocusControl == this)
                    DXControl.FocusControl = null;
                if (DXControl.ActiveScene == this)
                    DXControl.ActiveScene = null;
            }

            base.Dispose(disposing);
        }
    }
}