using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace HermitMod.Cards
{
    public class Strike_Hermit : HermitCard
    {
        public Strike_Hermit() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
        {
        }

        public override int MaxUpgradeLevel => this._maxUpgradeLevel;

        public void SetMaxUpgradeLevel(int level)
        {
            this._maxUpgradeLevel = level;
        }

        protected override HashSet<CardTag> CanonicalTags

        {
            get
            {
                return new HashSet< CardTag >

                {
                    CardTag.Strike

                }
                ;
            }
        }

        protected override IEnumerable<DynamicVar> CanonicalVars

        {
            get
            {
                return new DynamicVar[] { new DamageVar(6m, ValueProp.Move) };
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await CommonActions.CardAttack(this, play).Execute(ctx);
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Damage.UpgradeValueBy(3m);
        }

        private const int Damage = 6;
        private const int UpgradeDamage = 3;
        private int _maxUpgradeLevel = 1;
    }
}