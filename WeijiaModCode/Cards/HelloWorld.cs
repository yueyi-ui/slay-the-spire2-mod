using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using WeijiaMod.Character;

using WeijiaMod.Powers;

namespace WeijiaMod.Cards
{
    public class HelloWorld : WeijiaCard
    {
        public HelloWorld() : base(0, CardType.Skill, CardRarity.Basic, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            // Just gain 1 Bit using PowerCmd (not CommonActions.Apply)
            WeijiaBits.Gain(this.Owner, 1);
            await CardPileCmd.Draw(ctx, 1, this.Owner.Creature.Player);
        }
    }
}
