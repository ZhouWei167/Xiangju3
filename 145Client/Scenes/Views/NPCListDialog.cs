using Client.Controls;
using Client.Envir;
using Client.UserModels;
using Library;
using Library.SystemModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using C = Library.Network.ClientPackets;
using System.Threading.Tasks;

namespace Client.Scenes.Views
{
    /// <summary>
    /// NPC列表面板
    /// </summary>
    public sealed class NPCListDialog : DXWindow
    {
        #region Properties

        public DXListBox NPCListBox;

        public override WindowType Type => WindowType.None;
        public override bool CustomSize => false;
        public override bool AutomaticVisibility => false;

        #endregion

        /// <summary>
        /// NPC列表面板
        /// </summary>
        public NPCListDialog()
        {
            HasTitle = false;
            HasFooter = false;
            HasTopBorder = false;
            Movable = true;
            Opacity = 0F;

            Size = new Size(340, 420);
            Location = new Point(GameScene.Game.Size.Width - Size.Width - 10, 10);

            NPCListBox = new DXListBox
            {
                Parent = this,
                Location = new Point(ClientArea.X + 10, ClientArea.Y + 20),
                Size = new Size(ClientArea.Width - 20, ClientArea.Height - 20),
                BackColour = Color.FromArgb(32, 32, 32),
                Border = true,
                ScrollBar = { Border = true }  // 启用滚动条边框
            };

            Parent = GameScene.Game;

            UpdateNPCList();
        }

        public void UpdateNPCList()
        {
            // 清空列表
            foreach (DXControl control in NPCListBox.Controls.ToList())
            {
                if (control is DXListBoxItem)
                {
                    control.Dispose();
                }
            }

            // 获取当前地图的NPC列表
            var allNPCs = Globals.NPCInfoList.Binding.ToList();
            var mapNPCs = allNPCs.Where(x => x.Region?.Map == GameScene.Game.MapControl.MapInfo).ToList();

            if (!mapNPCs.Any())
            {
                DXListBoxItem item = new DXListBoxItem
                {
                    Parent = NPCListBox,
                    Label = { Text = "当前地图没有NPC" }
                };
                return;
            }

            // 获取当前地图的连接点列表
            var currentMap = GameScene.Game.MapControl.MapInfo;
            var movementCount = Globals.MovementInfoList?.Binding?.Count ?? 0;
            var mapMovements = Globals.MovementInfoList?.Binding?.Where(x => x.SourceRegion?.Map?.Index == currentMap?.Index).ToList();

            // 显示当前地图的所有连接点
            if (mapMovements != null)
            {
                // 按目标地图分组
                var groupedMovements = mapMovements
                    .Where(x => x.DestinationRegion?.Map != null)
                    .GroupBy(x => x.DestinationRegion.Map.Description)
                    .ToList();

                foreach (var group in groupedMovements)
                {
                    // 收集该目标地图的所有坐标点
                    var points = group.SelectMany(x => x.SourceRegion?.PointList ?? new List<Point>())
                        .Distinct()
                        .OrderBy(p => p.X)
                        .ThenBy(p => p.Y)
                        .ToList();

                    if (points.Count > 0)
                    {
                        // 创建合并后的连接点项
                        DXListBoxItem movementItem = new DXListBoxItem
                        {
                            Parent = NPCListBox,
                            Label = { Text = $"{currentMap?.Description ?? "未知"} -> {group.Key}" },
                            Item = new { Points = points, IsMovement = true }
                        };

                        movementItem.MouseEnter += (o, e) => movementItem.Label.ForeColour = Color.Yellow;
                        movementItem.MouseLeave += (o, e) => movementItem.Label.ForeColour = Color.White;
                        movementItem.MouseClick += (o, e) =>
                        {
                            // 找到最近的可移动点
                            var nearestPoint = points
                                .Where(p => !GameScene.Game.MapControl.Cells[p.X, p.Y].Flag)
                                .OrderBy(p => Math.Abs(p.X - GameScene.Game.MapControl.MapLocation.X) +
                                            Math.Abs(p.Y - GameScene.Game.MapControl.MapLocation.Y))
                                .FirstOrDefault();

                            if (nearestPoint == Point.Empty)
                                return;

                            // 开始寻路
                            if (!GameScene.Game.MapControl.PathFinder.bSearching)
                            {
                                GameScene.Game.MapControl.PathFinder.bSearching = true;
                                Task.Run(() =>
                                {
                                    GameScene.Game.MapControl.InitCurrentPath(nearestPoint.X, nearestPoint.Y);
                                    GameScene.Game.MapControl.PathFinder.bSearching = false;
                                });

                                Visible = false;
                            }
                        };
                    }
                }
            }

            // 添加NPC到列表
            foreach (NPCInfo npc in mapNPCs)
            {
                if (npc == null || string.IsNullOrEmpty(npc.NPCName) || npc.Region == null)
                    continue;

                // 创建列表项
                DXListBoxItem item = new DXListBoxItem
                {
                    Parent = NPCListBox,
                    Label = { Text = npc.NPCName + (npc.Display ? "" : " (隐藏)") +
                           $" -> {currentMap?.Description ?? "未知"}" +
                           (npc.Region?.PointList?.Count > 0 ?
                           $" [{npc.Region.PointList[0].X}, {npc.Region.PointList[0].Y}]" : " [未知坐标]") },
                    Item = npc
                };

                // 添加地图连接点信息
                if (npc.Region?.PointList?.Count > 0 && Globals.MovementInfoList?.Binding != null)
                {
                    var point = npc.Region.PointList[0];
                    var movements = mapMovements?.Where(x =>
                        x.SourceRegion?.PointList?.Any(p => p.X == point.X && p.Y == point.Y) == true).ToList();

                    if (movements != null && movements.Count > 0)
                    {
                        foreach (var movement in movements)
                        {
                            if (movement.DestinationRegion?.Map != null)
                            {
                                item.Label.Text = item.Label.Text.Replace($" -> {currentMap?.Description ?? "未知"}",
                                    $" -> {movement.DestinationRegion.Map.Description}");
                            }
                        }
                    }
                }

                item.MouseEnter += (o, e) => item.Label.ForeColour = npc.Display ? Color.Yellow : Color.White;
                item.MouseLeave += (o, e) => item.Label.ForeColour = npc.Display ? Color.LightYellow : Color.LightGray;
                item.MouseClick += (o, e) =>
                {
                    if (npc.Region == null || npc.Region.PointList == null || npc.Region.PointList.Count == 0)
                        return;

                    Point target = npc.Region.PointList[0];

                    // 如果目标是不可移动区域
                    if (GameScene.Game.MapControl.Cells[target.X, target.Y].Flag)
                        return;

                    // 开始寻路
                    if (!GameScene.Game.MapControl.PathFinder.bSearching)
                    {
                        GameScene.Game.MapControl.PathFinder.bSearching = true;
                        Task.Run(() =>
                        {
                            GameScene.Game.MapControl.InitCurrentPath(target.X, target.Y);
                            GameScene.Game.MapControl.PathFinder.bSearching = false;
                        });

                        Visible = false;
                    }
                };
            }

            // 重置滚动位置
            NPCListBox.ScrollBar.Value = 0;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (NPCListBox != null)
                {
                    if (!NPCListBox.IsDisposed)
                        NPCListBox.Dispose();
                    NPCListBox = null;
                }
            }
        }
    }
}
