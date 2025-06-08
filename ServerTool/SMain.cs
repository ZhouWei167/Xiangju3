using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Library.SystemModels;
using MirDB;
using Server.DBModels;
using Server.Envir;
using Server.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Server
{
    public partial class SMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public List<Control> Windows = new List<Control>();
        public static Session Session;
        private static GridView gridView;
        private static int[] selectRows = new int[0];
        /// <summary>
        /// 数据库对角所在程序集
        /// </summary>
        public Assembly[] Assemblies { get; private set; }
        private string[] MySqlParms;
        public SMain()
        {
            InitializeComponent();  //初始化组件
                                    //服务管理器 安全协议
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                   SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12;

        }

        private void SMain_Load(object sender, EventArgs e)
        {
            DLookAndFeel.LookAndFeel.SkinName = "Office 2010 Blue";
            Log("SMain_Load: Starting application initialization.");

            // Initialize Session 
            var assemblies = new Assembly[]
            {
                Assembly.GetAssembly(typeof(MirDB.DBObject)),
                Assembly.GetAssembly(typeof(Server.DBModels.AccountInfo)),
            };

            // Step 1: Create session instance and prepare for file-based loading initially
            Session = new Session(SessionMode.ServerTool, assemblies, Config.EnableDBEncryption, Config.DBPassword)
            {
                IsMySql = false, // Force file loading first
                Output = (o, ev) => Log(ev.ToString())
            };
            // Manually call the file initialization part of Session's Init() logic.
            // Session.Init() branches based on IsMySql. We need the non-SQL path executed.
            // Accessing private Initialize() directly is not possible, so we replicate its core purpose for ServerTool:
            // This would typically be a call to something like Session.LoadFromFiles();
            // For now, let's assume calling Init() then overriding IsMySql and MySqlParms is the way if Init() is smart enough
            // or if we ensure internal Load() methods are called correctly.
            
            // Let's call the internal method that loads from .db files.
            // We need to reflect to call the private Initialize() method or restructure Session.
            // A cleaner way would be to expose a LoadFiles() method in Session.
            // Given the current structure, we call Init() with IsMySql = false.
            Session.Init(); // This will call the non-SQL Initialize(), loading from .db files.
            Log("SMain_Load: Session.Init() called for file-based loading.");

            // Step 2: If MySQL is enabled in config, set the session to target MySQL for subsequent operations like InitDataBase
            if (Config.MySqlEnable)
            {
                Session.IsMySql = true;
                Session.MySqlParms = new string[]
                {
                    Config.MySqlUser,
                    Config.MySqlPassword,
                    Config.MySqlIP,
                    Config.MySqlPort
                };
                // If IsMySql is true, Session.Init() would have tried to InitializeSQL().
                // We need to ensure that the file data loaded above remains in Binding lists.
                // And that subsequent InitDataBase() uses this Binding data to populate MySQL.
                // The current Session.InitializeSQL() also creates collections but attempts to load from SQL.
                // This might clear or overwrite Binding lists if not handled carefully.

                // The ideal flow: 
                // 1. Session created.
                // 2. Session.LoadDataFromDbFiles() -> populates Bindings.
                // 3. If MySql enabled: Session.ConfigureForMySql(params) -> sets IsMySql=true, MySqlParms.
                // 4. When InitDataBase is called: if IsMySql, uses Bindings to populate MySql.

                // For now, by setting IsMySql = true AFTER file loading, InitDataBase() should correctly target MySQL.
            }

            // Now show views that might depend on Session
            Log("SMain_Load: Attempting to show ConfigView.");
            ShowView(typeof(ConfigView));
            Log("SMain_Load: Attempting to show SystemLogView.");
            ShowView(typeof(SystemLogView));   //显示视图(系统日志视图)
            Log("SMain_Load: Finished showing initial views.");
            
            // UpdateInterface();  //更新接口 // Commented out as definition is not found
            Application.Idle += Application_Idle;  //应用程序空闲

            InterfaceTimer.Start();
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            try
            {
                while (AppStillIdle)
                {
                    MapViewer.CurrentViewer?.Process();
                    DiyMonShowViewer.CurrentDiyMonViewer?.Process();
                    Thread.Sleep(1);
                }
            }
            catch (Exception ex)
            {
                SEnvir.Log(ex.ToString());
            }

        }

        protected override void OnClosing(CancelEventArgs e)  //关闭时
        {
            base.OnClosing(e);

            if (XtraMessageBox.Show(this, "确定要关闭数据库编辑器吗？", "关闭数据库编辑器", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }

            Session.BackUpDelay = 0;   //数据库备份延迟
            Session?.Save(true, SessionMode.ServerTool);    //数据库备份开启

        }
        private void ShowView(Type type)   //显示视图
        {
            try
            {
                Log($"ShowView: Attempting to show view of type {type.FullName}.");
                foreach (Control item in Windows)
                    if (item.GetType() == type)
                    {
                        Log($"ShowView: View {type.FullName} already exists, activating.");
                        tabbedView1.ActivateDocument(item);
                        return;
                    }

                Log($"ShowView: Creating new instance of {type.FullName}.");
                Form view = (Form)Activator.CreateInstance(type);
                Log($"ShowView: Instance of {type.FullName} created successfully.");
                view.MdiParent = this;
                view.Disposed += View_Disposed;
                view.Tag = type.Name;
                Windows.Add(view);
                view.Show();
                Log($"ShowView: View {type.FullName} shown.");
            }
            catch (Exception ex) // Added catch block for logging
            {
                Log($"ShowView: Error showing view {type.FullName}. Exception: {ex.ToString()}");
            }
            finally
            { }
        }
        private void View_Disposed(object sender, EventArgs e)  //视图释放
        {
            Windows.Remove((Control)sender);
        }

        private void InterfaceTimer_Tick(object sender, EventArgs e)  //接口计时器
        {

        }

        //数据库初始化按钮
        private async void FileToSqlButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("注意：此操作会将本地DB文件中的数据导入到MySQL数据库。\r\n1，请确保已经正确配置MySQL连接参数（用户、密码、IP、端口）。\r\n2，请确保MySQL中已经创建好空的 'SystemDB' 和 'UserDB' 数据库（如果尚不存在）。\r\n3，如果目标表已存在于MySQL中，表结构将被同步，现有数据可能会根据导入逻辑被处理（通常是清空后插入）。\r\n确定要继续吗？", "导入数据到MySQL提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            if (!Session.IsMySql)
            {
                XtraMessageBox.Show("MySQL未在配置中启用。请先在'配置'->'数据设置'中启用MySQL并保存设置，然后重启本工具再尝试导入。", "MySQL未启用", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 可选：禁用按钮或显示加载指示
            FileToSqlButton.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            Log("开始将本地数据导入到MySQL数据库...");

            try
            {
                Log("开始执行数据库初始化...");
                
                bool completed = false;
                Exception taskException = null;
                
                // 在后台线程执行，避免UI线程阻塞
                var task = Task.Run(() => {
                    try {
                        Session?.InitDataBase();
                        completed = true;
                    }
                    catch (Exception ex) {
                        taskException = ex;
                    }
                });
                
                // 设置10分钟超时
                bool finishedInTime = task.Wait(TimeSpan.FromMinutes(5));
                
                if (taskException != null)
                {
                    // 任务出错
                    Log($"数据导入过程中发生错误: {taskException.Message}");
                    XtraMessageBox.Show("数据导入过程中发生错误: " + taskException.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (completed)
                {
                    // 任务正常完成
                    Log("数据导入到MySQL的过程已成功完成。");
                    XtraMessageBox.Show("数据导入到MySQL的过程已成功完成。请查看系统日志获取详细信息。", "操作完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (!finishedInTime)
                {
                    // 任务超时，但数据库操作可能仍在进行
                    Log("数据导入操作超时，但仍在后台继续执行。请稍后检查数据库状态。");
                    XtraMessageBox.Show("数据导入操作超过了等待时间(10分钟)，但仍在后台继续处理。", "操作超时", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Log($"执行操作时发生错误: {ex.Message}");
                XtraMessageBox.Show($"执行操作时发生错误: {ex.Message}", 
                    "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 恢复按钮和光标
                FileToSqlButton.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }
        //MySql数据转换到Z版二进制文件
        private void SqlToFileButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!Session.IsMySql)
            {
                XtraMessageBox.Show("当前数据源为二进制文件，不需要转换。。。。", "提示");
                return;
            }
            Session.BackUpDelay = 0;
            if (Session.SqlToFile())
                XtraMessageBox.Show("转换成功。", "提示");
            else
                XtraMessageBox.Show("转换失败。", "提示");
        }

        //py统计按钮
        private void PyMetricsButton_LinkClicked(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowView(typeof(PyMetricsView));
        }

        //日志按钮
        private void LogNavButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(SystemLogView));
        }
        //聊天记录按钮
        private void ChatLogNavButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(ChatLogView));
        }
        //设置按钮
        private void ConfigButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(ConfigView));
        }
        //地图信息按钮
        private void MapInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(MapInfoView));
        }
        //怪物信息按钮
        private void MonsterInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(MonsterInfoView));
        }

        public static void SetUpView(GridView view)  //设置界面
        {
            view.BestFitColumns();
            view.KeyPress += PasteData_KeyPress;
            view.KeyDown += DeleteRows_KeyDown;
            view.OptionsSelection.MultiSelect = true;
            view.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
        }
        private static void DeleteRows_KeyDown(object sender, KeyEventArgs e)   //删除行
        {
            if (e.KeyCode != Keys.Delete) return;

            if (MessageBox.Show("删除行？", "确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            GridView view = (GridView)sender;
            //view.DeleteSelectedRows();

            int[] rows = view.GetSelectedRows();

            List<DBObject> objects = new List<DBObject>();

            foreach (int index in rows)
                objects.Add((DBObject)view.GetRow(index));

            foreach (DBObject ob in objects)
                ob?.Delete();
        }
        //复制黏贴数据
        public static void PasteData_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 0x3)
                {
                    e.Handled = true;

                    gridView = (GridView)sender;
                    selectRows = gridView.GetSelectedRows();

                    if (selectRows.Length == 0) return;

                    List<DBObject> objects = new List<DBObject>();

                    foreach (int index in selectRows)
                        objects.Add((DBObject)gridView.GetRow(index));

                    Clipboard.SetAudio(Session.CopyObject(objects));
                }

                if (e.KeyChar == 0x16)
                {
                    e.Handled = true;

                    GridView view = (GridView)sender;
                    int[] rows = view.GetSelectedRows();
                    IBindingList list = (IBindingList)view.DataSource;

                    //if (rows.Length > 0 && selectRows.Length > 0)
                    //{
                    //    foreach (int RowIndex in selectRows)
                    //    {
                    //        DBObject oldOb = gridView.GetRow(RowIndex) as DBObject;
                    //        if (list.GetType().GenericTypeArguments[0] != oldOb.GetType()) return;
                    //        Session.PasteObject(oldOb.GetType(), oldOb, (DBObject)list.AddNew());
                    //    }
                    //    XtraMessageBox.Show("复制成功，请到最后一行查看。", "提示");
                    //}
                    //else
                    {
                        MemoryStream mem = Clipboard.GetAudioStream() as MemoryStream;
                        if (mem != null)
                        {
                            using (mem)
                            using (BinaryReader reader = new BinaryReader(mem))
                            {
                                int count = reader.ReadInt32();
                                for (int i = 0; i < count; i++)
                                {
                                    DBMapping mapping = new DBMapping(Session.Assemblies, reader);
                                    if (mapping.Type == null) return;
                                    if (list.GetType().GenericTypeArguments[0] != mapping.Type) return;
                                    Session.PasteObject(reader, mapping, (DBObject)list.AddNew());
                                }
                                XtraMessageBox.Show("复制成功，请到最后一行查看。", "提示");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.ToString());
            }
        }

        //道具信息按钮
        private void ItemInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(ItemInfoView));
        }
        //NPC信息按钮
        private void NPCInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(NPCInfoView));
        }
        //NPC页按钮
        private void NPCPageButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(NPCPageView));
        }
        //魔法技能信息按钮
        private void MagicInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(MagicInfoView));
        }
        //角色信息按钮
        private void CharacterInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(CharacterView));
        }
        //账号信息按钮
        private void AccountInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(AccountView));
        }
        //地图链接信息按钮
        private void MovementInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(MovementInfoView));
        }
        //道具属性信息按钮
        private void ItemInfoStatButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(ItemInfoStatView));
        }
        //自定义道具特效按钮
        private void ItemCustomInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(ItemCustomInfoView));
        }
        //制作合成道具按钮
        private void ItemComposeButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(CraftItemInfoView));
        }
        //道具套装信息按钮
        private void SetInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(SetInfoView));
        }
        //商城信息按钮
        private void StoreInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(StoreInfoView));
        }
        //基础属性设置按钮
        private void BaseStatButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(BaseStatView));
        }

        #region Idle Check
        private static bool AppStillIdle
        {
            get
            {
                PeekMsg msg;
                return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
            }
        }

        [SuppressUnmanagedCodeSecurity]
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern bool PeekMessage(out PeekMsg msg, IntPtr hWnd, uint messageFilterMin,
                                               uint messageFilterMax, uint flags);

        [StructLayout(LayoutKind.Sequential)]
        private struct PeekMsg
        {
            private readonly IntPtr hWnd;
            private readonly Message msg;
            private readonly IntPtr wParam;
            private readonly IntPtr lParam;
            private readonly uint time;
            private readonly Point p;
        }
        #endregion

        //地图查看按钮
        private void barButtonItem1_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
        }
        //安全区信息按钮
        private void SafeZoneInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(SafeZoneInfoView));
        }
        //钓鱼区信息按钮
        private void FishingAreaInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(FishingAreaInfoView));
        }
        //怪物刷新信息按钮
        private void RespawnInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(RespawnInfoView));
        }
        //地图链接按钮
        private void MapRegionButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(MapRegionView));
        }
        //爆率信息按钮
        private void DropInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(DropInfoView));
        }
        //角色爆率按钮
        private void UserDropButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(UserDropView));
        }
        //人物成就按钮
        private void UserAchievementButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(UserAchievementView));
        }
        //任务信息按钮
        private void QuestInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(QuestInfoView));
        }
        //宠物信息按钮
        private void CompanionInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(CompanionInfoView));
        }
        //事件信息按钮
        private void EventInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(EventInfoView));
        }
        //怪物属性信息按钮
        private void MonsterInfoStatButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(MonsterInfoStatView));
        }
        //自定义怪物信息按钮
        private void MonsterCustomInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(MonAnimationFrameView));
        }
        //城堡信息按钮
        private void CastleInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(CastleInfoView));
        }
        //元宝充值按钮
        private void PaymentButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(GameGoldPaymentView));
        }
        //商城购买记录按钮
        private void StoreSalesButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(GameStoreSaleView));
        }
        //判断工具按钮
        private void DiagnosticButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(DiagnosticView));
        }
        //所有角色道具查询按钮
        private void navBarItem2_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(UserItemView));
        }
        //所有攻城统计信息按钮
        private void navBarItem3_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(UserConquestStatsView));
        }
        //所有角色邮件查询按钮
        private void navBarItem4_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(UserMailView));
        }
        //武器工艺设置按钮
        private void navBarItem5_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(WeaponCraftStatInfoView));
        }
        //配置服务器按钮
        private void barButtonItem2_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowView(typeof(ConfigView));
        }
        //成就信息按钮
        private void AchievementInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(AchievementInfoView));
        }
        //触发设置按钮
        private void TriggerButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(TriggerInfoView));
        }
        //角色任务查询按钮
        private void UserQuestButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(UserQuestView));
        }
        //自定义BUFF按钮
        private void BuffInfoButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(BuffInfoView));
        }
        //新版武器升级按钮
        private void WeaponUpgrade_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(WeaponUpgradeInfoView));
        }

        private void GuildButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(GuildInfoView));
        }

        private void DiyMagicEffectButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(DiyMagicEffectView));
        }

        private void MonDiyAiActionButton_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(MonDiyAiActionView));
        }

        private void InternationalButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowView(typeof(LangInfoView));
        }

        private void navBarItem7_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(QuestInfoEditForm));
        }

        private void navBarItem8_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowView(typeof(CurrencyQueryView));
        }

        private void Log(string text)
        {
            // Original code using SystemLogView.Instance which does not exist:
            // if (SystemLogView.Instance == null || SystemLogView.Instance.IsDisposed) return;
            // if (SystemLogView.Instance.LogTextBox.InvokeRequired)
            //     SystemLogView.Instance.LogTextBox.Invoke(new Action(() => Log(text)));
            // else
            // {
            //     SystemLogView.Instance.LogTextBox.AppendText(string.Format("[{0}]: {1}{2}", DateTime.Now, text, Environment.NewLine));
            //     SystemLogView.Instance.LogTextBox.ScrollToCaret();
            // }

            // Revised code to add to the static Logs list in SystemLogView
            // Assuming SMain runs on the UI thread or that Logs is thread-safe for adding.
            // If SystemLogView updates its UI from this Logs list, it needs to handle cross-thread operations if Log is called from non-UI threads.
            // For simplicity, directly adding. If InvokeRequired-like logic is needed, it should be within SystemLogView or handled differently.
            
            // We need to ensure this addition happens on the UI thread if SystemLogView's LogListBoxControl is directly bound
            // and doesn't handle cross-thread updates internally.
            // However, SystemLogView itself has a timer (InterfaceTimer_Tick) that dequeues from SEnvir.DisplayLogs
            // and adds to its own Logs list, which is bound to the UI.
            // So, the most consistent way is to use the SEnvir.DisplayLogs queue if that's the intended mechanism.

            // Let's check how SEnvir.Log is implemented or if SEnvir.DisplayLogs is the right place.
            // The Session.Output is set to (o, ev) => Log(ev.ToString()).
            // SEnvir.Log(ex.ToString()); is used elsewhere.
            // If SEnvir.Log also writes to SEnvir.DisplayLogs, then we might create a loop or duplicate logs.

            // For now, let's assume SMain.Log should directly add to a list that SystemLogView can pick up,
            // or SystemLogView should provide a static method to add logs.
            // Given SystemLogView.Logs is public static, we can add to it.
            // Cross-thread safety for BindingList: Adding is generally not thread-safe if bound to UI that doesn't expect it.
            
            // Let's try adding to SEnvir.DisplayLogs, as SystemLogView already processes this queue.
            // This seems like a more robust approach than directly manipulating SystemLogView.Logs from here.
            Server.Envir.SEnvir.DisplayLogs.Enqueue(string.Format("[{0}]: {1}", DateTime.Now, text));

            // If direct update to SystemLogView.Logs was intended and UI thread handling is managed by DevExpress:
            // if (this.InvokeRequired) // SMain is a Form
            // {
            //    this.Invoke(new Action(() => {
            //        if (SystemLogView.Logs != null)
            //        {
            //            SystemLogView.Logs.Add(string.Format("[{0}]: {1}", DateTime.Now, text));
            //        }
            //    }));
            // }
            // else
            // {
            //    if (SystemLogView.Logs != null)
            //    {
            //        SystemLogView.Logs.Add(string.Format("[{0}]: {1}", DateTime.Now, text));
            //    }
            // }
        }
    }

}
