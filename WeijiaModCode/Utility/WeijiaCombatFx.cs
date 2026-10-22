using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace WeijiaMod.Utility
{
	
	public static class WeijiaCombatFx
	{
		public static void GroundFireOnTarget(Creature target)
		{
			NGroundFireVfx ngroundFireVfx = NGroundFireVfx.Create(target, VfxColor.Red);
			bool flag = ngroundFireVfx != null;
			if (flag)
			{
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance != null)
				{
					instance.CombatVfxContainer.AddChildSafely(ngroundFireVfx);
				}
			}
			SfxCmd.Play("event:/sfx/characters/attack_fire", 1f);
		}

		public static AttackCommand WithHermitGunHitFx(this AttackCommand cmd)
		{
			return cmd.WithHitFx("vfx/vfx_attack_blunt", null, null);
		}

		public static AttackCommand WithHermitFireHitFx(this AttackCommand cmd)
		{
			return cmd.WithHitFx("vfx/vfx_fire_burst", "event:/sfx/characters/attack_fire", null);
		}

		public static AttackCommand WithHermitBluntLightHitFx(this AttackCommand cmd)
		{
			return cmd.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3");
		}

		public static AttackCommand WithHermitBluntHeavyHitFx(this AttackCommand cmd)
		{
			return cmd.WithHitFx("vfx/vfx_attack_blunt", null, "heavy_attack.mp3");
		}

		public static AttackCommand WithHermitShortFuseHitFx(this AttackCommand cmd)
		{
			return cmd.WithHitFx("vfx/vfx_coin_explosion_small", null, "heavy_attack.mp3");
		}

		public static AttackCommand WithHermitSlashHitFx(this AttackCommand cmd)
		{
			return cmd.WithHitFx("vfx/vfx_attack_slash", null, "slash_attack.mp3");
		}

		public const string GunHitVfx = "vfx/vfx_attack_blunt";

		public const string FireHitVfx = "vfx/vfx_fire_burst";

		public const string BluntHitVfx = "vfx/vfx_attack_blunt";

		public const string BluntLightHitTmpSfx = "blunt_attack.mp3";

		public const string BluntHeavyHitTmpSfx = "heavy_attack.mp3";

		public const string ShortFuseHitVfx = "vfx/vfx_coin_explosion_small";

		public const string SlashHitVfx = "vfx/vfx_attack_slash";

		public const string SlashHitTmpSfx = "slash_attack.mp3";
	}
}




