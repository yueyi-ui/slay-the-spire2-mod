using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using HermitMod.Character;
using HermitMod.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;

namespace HermitMod.Cards
{
	// Token: 0x02000067 RID: 103
	[Pool(typeof(HermitCardPool))]
	public abstract class HermitCard : CustomCardModel
	{
		// Token: 0x060001F0 RID: 496 RVA: 0x000082CC File Offset: 0x000064CC
		protected HermitCard(int cost, CardType type, CardRarity rarity, TargetType target) : base(cost, type, rarity, target, true, true)
		{
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x000082DC File Offset: 0x000064DC
		public virtual bool HasDeadOn
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x000082DF File Offset: 0x000064DF
		protected virtual IEnumerable<CardKeyword> CustomKeywords
		{
			get
			{
				return Array.Empty<CardKeyword>();
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x000082E6 File Offset: 0x000064E6
		protected virtual IEnumerable<IHoverTip> AdditionalHoverTips
		{
			get
			{
				return Array.Empty<IHoverTip>();
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x000082F0 File Offset: 0x000064F0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
                return AdditionalHoverTips;
            }
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000830F File Offset: 0x0000650F
		private string FallbackArt
		{
			get
			{
				return (this.Rarity == CardRarity.Ancient) ? "no_art_full" : "no_art";
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x00008326 File Offset: 0x00006526
		private string CardFileName
		{
			get
			{
				return base.Id.Entry.RemovePrefix().ToLowerInvariant();
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000833D File Offset: 0x0000653D
		private string ResolvePortrait(string candidate, string fallback)
		{
			return ResourceLoader.Exists("res://" + candidate, "") ? candidate : fallback;
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000835A File Offset: 0x0000655A
		public override string CustomPortraitPath
		{
			get
			{
				return this.ResolvePortrait((this.CardFileName + ".png").BigCardImagePath(), (this.FallbackArt + ".png").BigCardImagePath());
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000838C File Offset: 0x0000658C
		public override string PortraitPath
		{
			get
			{
				return this.ResolvePortrait((this.CardFileName + ".png").CardImagePath(), (this.FallbackArt + ".png").CardImagePath());
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060001FA RID: 506 RVA: 0x000083BE File Offset: 0x000065BE
		public override string BetaPortraitPath
		{
			get
			{
				return this.ResolvePortrait(("beta/" + this.CardFileName + ".png").CardImagePath(), (this.FallbackArt + ".png").CardImagePath());
			}
		}
	}
}
