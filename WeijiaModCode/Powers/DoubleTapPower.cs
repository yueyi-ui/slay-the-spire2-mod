using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace WeijiaMod.Powers
{
    /// <summary>
    /// 连射 - 下回合开始时获得能量，然后移除。
    /// </summary>
    public sealed class DoubleTapPower : WeijiaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterAutoPrePlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
        {
            decimal energy = this.Amount;
            if (energy > 0m)
            {
                await PlayerCmd.GainEnergy(energy, player);
                await PowerCmd.Apply<DoubleTapPower>(choiceContext, this.Owner, -this.Amount, this.Owner, null, false);
            }
        }
    }
}