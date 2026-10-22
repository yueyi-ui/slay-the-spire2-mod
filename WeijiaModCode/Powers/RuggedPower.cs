using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Commands;

namespace WeijiaMod.Powers
{
    public sealed class RuggedPower : WeijiaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        /// <summary>
        /// 每层坚韧在回合开始时获得 5 点格挡，然后减少 1 层。
        /// </summary>
        public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            if (side == CombatSide.Player)
            {
                await CreatureCmd.GainBlock(this.Owner, this.Amount * 5m, ValueProp.Move, null);
                await PowerCmd.Apply<RuggedPower>(choiceContext, this.Owner, -1m, this.Owner, null, false);
            }
        }
    }
}
