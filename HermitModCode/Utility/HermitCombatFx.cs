using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace HermitMod.Utility
{
	// Token: 0x02000004 RID: 4
	
	public static class HermitCombatFx
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000022F8 File Offset: 0x000004F8
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

		// Token: 0x0600000D RID: 13 RVA: 0x0000233D File Offset: 0x0000053D
		public static AttackCommand WithHermitGunHitFx(this AttackCommand cmd)
		{
			return cmd.WithHitFx("vfx/vfx_attack_blunt", null, null);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000234C File Offset: 0x0000054C
		public static AttackCommand WithHermitFireHitFx(this AttackCommand cmd)
		{
			return cmd.WithHitFx("vfx/vfx_fire_burst", "event:/sfx/characters/attack_fire", null);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000235F File Offset: 0x0000055F
		public static AttackCommand WithHermitBluntLightHitFx(this AttackCommand cmd)
		{
			return cmd.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3");
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002372 File Offset: 0x00000572
		public static AttackCommand WithHermitBluntHeavyHitFx(this AttackCommand cmd)
		{
			return cmd.WithHitFx("vfx/vfx_attack_blunt", null, "heavy_attack.mp3");
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002385 File Offset: 0x00000585
		public static AttackCommand WithHermitShortFuseHitFx(this AttackCommand cmd)
		{
			return cmd.WithHitFx("vfx/vfx_coin_explosion_small", null, "heavy_attack.mp3");
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002398 File Offset: 0x00000598
		public static AttackCommand WithHermitSlashHitFx(this AttackCommand cmd)
		{
			return cmd.WithHitFx("vfx/vfx_attack_slash", null, "slash_attack.mp3");
		}

		// Token: 0x04000003 RID: 3
		public const string GunHitVfx = "vfx/vfx_attack_blunt";

		// Token: 0x04000004 RID: 4
		public const string FireHitVfx = "vfx/vfx_fire_burst";

		// Token: 0x04000005 RID: 5
		public const string BluntHitVfx = "vfx/vfx_attack_blunt";

		// Token: 0x04000006 RID: 6
		public const string BluntLightHitTmpSfx = "blunt_attack.mp3";

		// Token: 0x04000007 RID: 7
		public const string BluntHeavyHitTmpSfx = "heavy_attack.mp3";

		// Token: 0x04000008 RID: 8
		public const string ShortFuseHitVfx = "vfx/vfx_coin_explosion_small";

		// Token: 0x04000009 RID: 9
		public const string SlashHitVfx = "vfx/vfx_attack_slash";

		// Token: 0x0400000A RID: 10
		public const string SlashHitTmpSfx = "slash_attack.mp3";
	}
}
