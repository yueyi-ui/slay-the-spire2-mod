using System;
using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using HermitMod.Character;
using HermitMod.Extensions;

namespace HermitMod.Relics
{
	// Token: 0x0200000B RID: 11
	[Pool(typeof(HermitRelicPool))]
	public abstract class HermitRelic : CustomRelicModel
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000039 RID: 57 RVA: 0x000031D6 File Offset: 0x000013D6
		public override string PackedIconPath
		{
			get
			{
				return (base.Id.Entry.RemovePrefix().ToLowerInvariant() + ".png").RelicImagePath();
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600003A RID: 58 RVA: 0x000031FC File Offset: 0x000013FC
		protected override string PackedIconOutlinePath
		{
			get
			{
				return (base.Id.Entry.RemovePrefix().ToLowerInvariant() + "_outline.png").RelicImagePath();
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00003222 File Offset: 0x00001422
		protected override string BigIconPath
		{
			get
			{
				return (base.Id.Entry.RemovePrefix().ToLowerInvariant() + ".png").BigRelicImagePath();
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003248 File Offset: 0x00001448
		protected HermitRelic() : base(true)
		{
		}
	}
}
