	using System;
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;
	using BaseLib.Abstracts;
	using BaseLib.Extensions;
	using BaseLib.Utils;
	using Godot;
	using WeijiaMod.Character;
	using WeijiaMod.Extensions;
	using MegaCrit.Sts2.Core.Entities.Cards;
	using MegaCrit.Sts2.Core.HoverTips;
	using MegaCrit.Sts2.Core.Commands;

	namespace WeijiaMod.Cards
	{
		[Pool(typeof(WeijiaCardPool))]
   
		public abstract class WeijiaCard : CustomCardModel
		{
			protected WeijiaCard(int cost, CardType type, CardRarity rarity, TargetType target) : base(cost, type, rarity, target, true, true)
			{
			}

			public virtual bool HasDeadOn
			{
				get
				{
					return false;
				}
			}

			protected virtual IEnumerable<CardKeyword> CustomKeywords
			{
				get
				{
					return Array.Empty<CardKeyword>();
				}
			}

			public override IEnumerable<CardKeyword> CanonicalKeywords
			{
				get
				{
					return CustomKeywords;
				}
			}
			protected virtual IEnumerable<IHoverTip> AdditionalHoverTips
			{
				get
				{
					return Array.Empty<IHoverTip>();
				}
			}

			protected override IEnumerable<IHoverTip> ExtraHoverTips
			{
				get
				{
					return AdditionalHoverTips;
				}
			}

			private string FallbackArt
			{
				get
				{
					return (this.Rarity == CardRarity.Ancient) ? "no_art_full" : "no_art";
				}
			}

			private string CardFileName
			{
				get
				{
					return base.Id.Entry.RemovePrefix().ToLowerInvariant();
				}
			}

			private string ResolvePortrait(string candidate, string fallback)
			{
				return ResourceLoader.Exists("res://" + candidate, "") ? candidate : fallback;
			}

			public override string CustomPortraitPath
			{
				get
				{
					return this.ResolvePortrait((this.CardFileName + ".png").BigCardImagePath(), (this.FallbackArt + ".png").BigCardImagePath());
				}
			}

			public override string PortraitPath
			{
				get
				{
					return this.ResolvePortrait((this.CardFileName + ".png").CardImagePath(), (this.FallbackArt + ".png").CardImagePath());
				}
			}

			public override string BetaPortraitPath
			{
				get
				{
					return this.ResolvePortrait(("beta/" + this.CardFileName + ".png").CardImagePath(), (this.FallbackArt + ".png").CardImagePath());
					
				}
			}
		}
	}




