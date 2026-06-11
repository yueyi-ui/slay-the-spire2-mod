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

namespace HermitMod.Cards
{
	// Token: 0x02000051 RID: 81
	public class Defend_Hermit : HermitCard
	{
		// Token: 0x0600017B RID: 379 RVA: 0x00007484 File Offset: 0x00005684
		public Defend_Hermit() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
		{
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00007498 File Offset: 0x00005698
		public override int MaxUpgradeLevel
		{
			get
			{
				return this._maxUpgradeLevel;
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x000074A0 File Offset: 0x000056A0
		public void SetMaxUpgradeLevel(int level)
		{
			this._maxUpgradeLevel = level;
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600017E RID: 382 RVA: 0x000074A9 File Offset: 0x000056A9
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

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600017F RID: 383 RVA: 0x000074B8 File Offset: 0x000056B8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
                return new DynamicVar[] { new BlockVar(5m, ValueProp.Move) };
			}
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000074CC File Offset: 0x000056CC
		[DebuggerStepThrough]
		protected override Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
		{
            return CommonActions.CardBlock(this, play);
        }

		// Token: 0x06000181 RID: 385 RVA: 0x0000751E File Offset: 0x0000571E
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}

		// Token: 0x0400005B RID: 91
		private const int Block = 5;

		// Token: 0x0400005C RID: 92
		private const int UpgradeBlock = 3;

		// Token: 0x0400005D RID: 93
		private int _maxUpgradeLevel = 1;
	}
}
