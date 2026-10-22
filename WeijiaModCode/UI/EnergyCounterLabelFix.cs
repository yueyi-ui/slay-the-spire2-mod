using Godot;
using HarmonyLib;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace WeijiaMod.UI
{
    /// <summary>
    /// 修复自定义能量计数器场景：BaseLib 的场景转换对泛型 Instantiate&lt;NEnergyCounter&gt; 不生效，
    /// 导致场景里的 Label 不会被转换成 MegaLabel，基类 _Ready 的 GetNode&lt;MegaLabel&gt; 抛异常。
    /// 此补丁在 _Ready 前把根节点下的 Label 替换为 MegaLabel。
    /// </summary>
    [HarmonyPatch]
    public static class EnergyCounterLabelFix
    {
        [HarmonyPatch(typeof(NEnergyCounter), "_Ready")]
        [HarmonyPrefix]
        private static void OnEnergyCounterReady(NEnergyCounter __instance)
        {
            try
            {
                // 只为为嘉角色修复（场景路径含 weijia/energy_counters）
                var scenePath = __instance.SceneFilePath;
                if (string.IsNullOrEmpty(scenePath) ||
                    (!scenePath.Contains("weijia") && !scenePath.Contains("energy_counters")))
                    return;

                var labelNode = __instance.GetNodeOrNull<Label>("Label");
                if (labelNode == null || labelNode is MegaLabel) return;

                var megaLabel = new MegaLabel
                {
                    Name = labelNode.Name,
                    Text = labelNode.Text,
                    HorizontalAlignment = labelNode.HorizontalAlignment,
                    VerticalAlignment = labelNode.VerticalAlignment,
                    AutowrapMode = labelNode.AutowrapMode,
                    ClipText = labelNode.ClipText,
                    Uppercase = labelNode.Uppercase,
                    LayoutMode = labelNode.LayoutMode,
                    AnchorLeft = labelNode.AnchorLeft,
                    AnchorTop = labelNode.AnchorTop,
                    AnchorRight = labelNode.AnchorRight,
                    AnchorBottom = labelNode.AnchorBottom,
                    OffsetLeft = labelNode.OffsetLeft,
                    OffsetTop = labelNode.OffsetTop,
                    OffsetRight = labelNode.OffsetRight,
                    OffsetBottom = labelNode.OffsetBottom,
                    GrowHorizontal = labelNode.GrowHorizontal,
                    GrowVertical = labelNode.GrowVertical
                };
                megaLabel.AddThemeFontSizeOverride("font_size", labelNode.GetThemeFontSize("font_size"));
                megaLabel.AddThemeColorOverride("font_color", labelNode.GetThemeColor("font_color"));

                var parent = labelNode.GetParent();
                int index = labelNode.GetIndex();
                var oldName = labelNode.Name;
                parent.RemoveChild(labelNode);
                labelNode.Free();
                megaLabel.Name = oldName;
                parent.AddChild(megaLabel, false, Node.InternalMode.Disabled);
                parent.MoveChild(megaLabel, index);
            }
            catch (System.Exception)
            {
                // 如果替换失败，让基类自己处理（可能仍会报错但不致命）
            }
        }
    }
}
