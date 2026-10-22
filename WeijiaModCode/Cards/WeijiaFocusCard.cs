using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using WeijiaMod.Character;
using WeijiaMod.Powers;

namespace WeijiaMod.Cards
{
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaFocusCard : WeijiaCard
    {
        public WeijiaFocusCard() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            decimal focus = IsUpgraded ? 2m : 1m;
            int draws = IsUpgraded ? 2 : 1;
            await PowerCmd.Apply<WeijiaMod.Powers.FocusPower>(ctx, this.Owner.Creature, focus, this.Owner.Creature, this, false);
            await CardPileCmd.Draw(ctx, draws, this.Owner.Creature.Player);
        }

        protected override void OnUpgrade() { }
    }
}
