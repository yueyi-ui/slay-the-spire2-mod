using System;
using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using Godot;
using WeijiaMod.Extensions;

namespace WeijiaMod.Character
{
	public class WeijiaRelicPool : CustomRelicPoolModel
	{
		public override Color LabOutlineColor
		{
			get
			{
				return Weijia.Color;
			}
		}

		public override string BigEnergyIconPath
		{
			get
			{
				return "charui/big_energy.png".ImagePath();
			}
		}

		public override string TextEnergyIconPath
		{
			get
			{
				return "charui/text_energy.png".ImagePath();
			}
		}
	}
}




