using System;
using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using Godot;
using HermitMod.Extensions;

namespace HermitMod.Character
{
	// Token: 0x0200003E RID: 62
	public class HermitPotionPool : CustomPotionPoolModel
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000113 RID: 275 RVA: 0x0000518C File Offset: 0x0000338C
		public override Color LabOutlineColor
		{
			get
			{
				return Hermit.Color;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00005193 File Offset: 0x00003393
		public override string BigEnergyIconPath
		{
			get
			{
				return "charui/big_energy.png".ImagePath();
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000115 RID: 277 RVA: 0x0000519F File Offset: 0x0000339F
		public override string TextEnergyIconPath
		{
			get
			{
				return "charui/text_energy.png".ImagePath();
			}
		}
	}
}
