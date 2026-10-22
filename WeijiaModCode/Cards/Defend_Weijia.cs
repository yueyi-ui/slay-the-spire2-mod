using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace WeijiaMod.Cards
{
	public class Defend_Weijia : WeijiaCard
	{
		public Defend_Weijia() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
		{
		}

		public override int MaxUpgradeLevel
		{
			get
			{
				return this._maxUpgradeLevel;
			}
		}

		public void SetMaxUpgradeLevel(int level)
		{
			this._maxUpgradeLevel = level;
		}

		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag>
				{
					CardTag.Defend
				};
			}
		}

		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
                return new DynamicVar[] { new BlockVar(5m, ValueProp.Move) };
			}
		}

		[DebuggerStepThrough]
		protected override Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
		{
            return CommonActions.CardBlock(this, play);
        }

		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}


		private int _maxUpgradeLevel = 1;
	}
}







