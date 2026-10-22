using System;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using WeijiaMod.Extensions;

namespace WeijiaMod.Powers
{
	
	public abstract class WeijiaPower : CustomPowerModel
	{
		public override string CustomPackedIconPath
		{
			get
			{
				return (base.Id.Entry.RemovePrefix().ToLowerInvariant() + ".png").PowerImagePath();
			}
		}

		public override string CustomBigIconPath
		{
			get
			{
				return (base.Id.Entry.RemovePrefix().ToLowerInvariant() + ".png").BigPowerImagePath();
			}
		}
	}
}




