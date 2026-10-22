using System;
using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using WeijiaMod.Character;
using WeijiaMod.Extensions;

namespace WeijiaMod.Relics
{
	[Pool(typeof(WeijiaRelicPool))]
	public abstract class WeijiaRelic : CustomRelicModel
	{
		public override string PackedIconPath
		{
			get
			{
				return (base.Id.Entry.RemovePrefix().ToLowerInvariant() + ".png").RelicImagePath();
			}
		}

		protected override string PackedIconOutlinePath
		{
			get
			{
				return (base.Id.Entry.RemovePrefix().ToLowerInvariant() + "_outline.png").RelicImagePath();
			}
		}

		protected override string BigIconPath
		{
			get
			{
				return (base.Id.Entry.RemovePrefix().ToLowerInvariant() + ".png").BigRelicImagePath();
			}
		}

		protected WeijiaRelic() : base(true)
		{
		}
	}
}




