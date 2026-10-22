using System;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.Combat;
using WeijiaMod.Powers;

namespace WeijiaMod.UI
{
    /// <summary>
    /// 比特计数器 UI。
    /// 阴阳圆形比特图标 + 中间数字，位于能量计数器的右下角。
    /// 使用 EnergyCounterContainer 定位（能量计数器 128x128，比特放在右下角外侧）。
    /// 注意：不使用 ShouldAlwaysShowStarCounter / 真辉星机制，避免显示游戏默认辉星。
    /// </summary>
    [HarmonyPatch]
    public static class BitCounterUi
    {
        private static Control _root;
        private static Label _label;
        private static Player _player;

        [HarmonyPatch(typeof(NCombatUi), nameof(NCombatUi.Activate))]
        [HarmonyPostfix]
        private static void OnCombatUiActivate(NCombatUi __instance, CombatState state)
        {
            try
            {
                _player = LocalContext.GetMe(state);
                if (_player == null) return;

                var container = __instance.EnergyCounterContainer;
                if (container == null) return;

                RemoveOld();

                // 根容器：56x56，位于能量计数器（128x128）的右下角
                _root = new Control
                {
                    Name = "BitCounter",
                    MouseFilter = Control.MouseFilterEnum.Ignore
                };
                _root.Position = new Vector2(76f, 76f); // 能量计数器右下角偏移
                _root.Size = new Vector2(56f, 56f);

                // 阴阳圆形图标（56x56 铺满）
                var icon = new TextureRect
                {
                    Name = "BitIcon",
                    Texture = ResourceLoader.Load<Texture2D>("res://images/powers/bit_icon.png", null, ResourceLoader.CacheMode.Reuse),
                    ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                    StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                    MouseFilter = Control.MouseFilterEnum.Ignore
                };
                icon.SetAnchorsPreset(Control.LayoutPreset.FullRect);
                _root.AddChild(icon);

                // 数字标签：居中在图标上
                _label = new Label
                {
                    Text = "0",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    MouseFilter = Control.MouseFilterEnum.Ignore
                };
                _label.SetAnchorsPreset(Control.LayoutPreset.FullRect);
                _label.AddThemeColorOverride("font_color", Colors.White);
                _label.AddThemeColorOverride("font_outline_color", new Color(0f, 0f, 0f, 0.8f));
                _label.AddThemeConstantOverride("outline_size", 8);
                _label.AddThemeFontSizeOverride("font_size", 22);
                _root.AddChild(_label);

                container.AddChild(_root);
                Refresh();
            }
            catch (Exception ex)
            {
                MegaCrit.Sts2.Core.Logging.Log.Debug("BitCounterUi error: " + ex.Message, 1);
            }
        }

        [HarmonyPatch(typeof(NCombatUi), nameof(NCombatUi.Deactivate))]
        [HarmonyPostfix]
        private static void OnCombatUiDeactivate()
        {
            WeijiaBits.ClearAll();
            RemoveOld();
            _player = null;
        }

        private static void RemoveOld()
        {
            if (_root != null && GodotObject.IsInstanceValid(_root))
            {
                _root.QueueFree();
            }
            _root = null;
            _label = null;
        }

        static BitCounterUi()
        {
            WeijiaBits.BitsChanged += (player, oldVal, newVal) => Refresh();
        }

        private static void Refresh()
        {
            if (_label == null || !GodotObject.IsInstanceValid(_label)) return;
            if (_player == null) return;

            int bits = WeijiaBits.Get(_player);
            _label.Text = bits.ToString();
            _label.Modulate = bits > 0 ? Colors.White : new Color(0.6f, 0.6f, 0.6f);
        }
    }
}
