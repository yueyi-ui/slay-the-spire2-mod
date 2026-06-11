using System;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace HermitMod.Powers
{
    public sealed class AdaptPower : HermitPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public async Task AfterAutoPrePlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
        {
            await CreatureCmd.GainBlock(player.Creature, this.Amount * BlockPerStack, ValueProp.Move, null);
        }

        private const int BlockPerStack = 8;
    }
}