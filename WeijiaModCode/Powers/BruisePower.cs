using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace WeijiaMod.Powers
{
    public sealed class BruisePower : WeijiaPower
    {
        public override PowerType Type => PowerType.Debuff;
        public override PowerStackType StackType => PowerStackType.Counter;

        /// <summary>
        /// 每层青肿在敌人回合结束时造成 4 点伤害，然后减少 1 层。
        /// 使用 CreatureCmd.Damage 避免 AttackCommand 需要来源的问题。
        /// </summary>
        public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            if (side == CombatSide.Enemy)
            {
                decimal currentAmount = this.Amount;
                if (currentAmount <= 0m) return;

                // 先减层数（避免归零移除时冲突）
                await PowerCmd.Apply<BruisePower>(choiceContext, this.Owner, -1m, this.Owner, null, false);

                // 再造成伤害（使用 CreatureCmd.Damage 无需来源）
                decimal dmg = currentAmount * 4m;
                if (dmg > 0m)
                {
                    await CreatureCmd.Damage(choiceContext, this.Owner, dmg, ValueProp.Move, this.Owner);
                }
            }
        }
    }
}
