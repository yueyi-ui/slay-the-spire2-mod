using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Patches.Content;
using BaseLib.Utils;
using HermitMod.Character;
using HermitMod.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace HermitMod.Cards
{
    public class HelloWorld : HermitCard
    {
        public HelloWorld() : base(0, CardType.Skill, CardRarity.Basic, TargetType.None)
        {
        }

        protected override IEnumerable<CardKeyword> CustomKeywords
        {
            get
            {
                return new CardKeyword[] { HermitKeywords.Bit };
            }
        }

        protected override IEnumerable<IHoverTip> AdditionalHoverTips
        {
            get => Array.Empty<IHoverTip>();
        }

        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            // 获得1比特（直接Apply，Power的Counter会自动叠加，上限后面再处理）
            await CommonActions.Apply < BitPower > (ctx, this.Owner.Creature, this, 1m);

            // 抽1张牌
            await CommonActions.Draw(this, ctx);
        }
    }
}