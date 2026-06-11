using System;
using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using Godot;
using HermitMod.Extensions;

namespace HermitMod.Character
{
	// Token: 0x0200003F RID: 63
	public class HermitRelicPool : CustomRelicPoolModel
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000117 RID: 279 RVA: 0x000051B4 File Offset: 0x000033B4
		public override Color LabOutlineColor
		{
			get
			{
				return Hermit.Color;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000118 RID: 280 RVA: 0x000051BB File Offset: 0x000033BB
		public override string BigEnergyIconPath
		{
			get
			{
				return "charui/big_energy.png".ImagePath();
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000119 RID: 281 RVA: 0x000051C7 File Offset: 0x000033C7
		public override string TextEnergyIconPath
		{
			get
			{
				return "charui/text_energy.png".ImagePath();
			}
		}
	}
}
