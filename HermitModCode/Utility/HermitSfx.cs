using System;
using System.Runtime.CompilerServices;

namespace HermitMod.Utility
{
	// Token: 0x02000005 RID: 5
	
	public static class HermitSfx
	{
		// Token: 0x06000013 RID: 19 RVA: 0x000023AB File Offset: 0x000005AB
		public static void PlayGun1(float volumeDb = 5f, float pitchVariation = 0.1f)
		{
			ModAudio.PlaySfx("hermit_gun", volumeDb, pitchVariation);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000023BA File Offset: 0x000005BA
		public static void PlayGun2(float volumeDb = 5f, float pitchVariation = 0.1f)
		{
			ModAudio.PlaySfx("hermit_gun2", volumeDb, pitchVariation);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000023C9 File Offset: 0x000005C9
		public static void PlayGun3(float volumeDb = 5f, float pitchVariation = 0.1f)
		{
			ModAudio.PlaySfx("hermit_gun3", volumeDb, pitchVariation);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000023D8 File Offset: 0x000005D8
		public static void PlaySpin(float volumeDb = 5f, float pitchVariation = 0.15f)
		{
			ModAudio.PlaySfx("hermit_spin", volumeDb, pitchVariation);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000023E7 File Offset: 0x000005E7
		public static void PlayReload(float volumeDb = 5f, float pitchVariation = 0.1f)
		{
			ModAudio.PlaySfx("hermit_reload", volumeDb, pitchVariation);
		}

		// Token: 0x0400000B RID: 11
		public const string Gun1 = "hermit_gun";

		// Token: 0x0400000C RID: 12
		public const string Gun2 = "hermit_gun2";

		// Token: 0x0400000D RID: 13
		public const string Gun3 = "hermit_gun3";

		// Token: 0x0400000E RID: 14
		public const string Spin = "hermit_spin";

		// Token: 0x0400000F RID: 15
		public const string Reload = "hermit_reload";

		// Token: 0x04000010 RID: 16
		public const float DefaultDb = 5f;

		// Token: 0x04000011 RID: 17
		public const float SpinPitchVariation = 0.15f;

		// Token: 0x04000012 RID: 18
		public const float GunPitchVariation = 0.1f;
	}
}
