using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Linq;

namespace WeijiaMod.Powers
{
    /// <summary>
    /// 专注思维 - 每回合开始抽1张牌。
    /// 若持有专注，改为抽2张。
    /// </summary>
    public sealed class FocusedMindPower : WeijiaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterAutoPrePlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
        {
            int draws = 1;
            var focus = this.Owner.Powers.OfType<FocusPower>().FirstOrDefault();
            if (focus != null && focus.Amount > 0m) draws = 2;
            await CardPileCmd.Draw(choiceContext, draws, player);
        }
    }
}