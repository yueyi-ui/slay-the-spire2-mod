using System;
using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using HermitMod.Extensions;

namespace HermitMod.Powers
{
	// Token: 0x02000023 RID: 35
	
	public abstract class HermitPower : CustomPowerModel
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00004172 File Offset: 0x00002372
		public override string CustomPackedIconPath
		{
			get
			{
				return (base.Id.Entry.RemovePrefix().ToLowerInvariant() + ".png").PowerImagePath();
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00004198 File Offset: 0x00002398
		public override string CustomBigIconPath
		{
			get
			{
				return (base.Id.Entry.RemovePrefix().ToLowerInvariant() + ".png").BigPowerImagePath();
			}
		}
	}
}
