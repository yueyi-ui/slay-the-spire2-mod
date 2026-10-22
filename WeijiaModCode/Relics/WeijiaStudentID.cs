using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using WeijiaMod.Powers;

namespace WeijiaMod.Relics
{
    public sealed class WeijiaStudentID : WeijiaRelic
    {
        public override RelicRarity Rarity => RelicRarity.Starter;

        public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            if (side == CombatSide.Player)
            {
                WeijiaBits.Gain(this.Owner, 1);
            }
        }
    }
}

