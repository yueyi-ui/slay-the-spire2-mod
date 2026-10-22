using System;
using System.Runtime.CompilerServices;

namespace WeijiaMod.Utility
{
	
	public static class WeijiaSfx
	{
		public static void PlayGun1(float volumeDb = 5f, float pitchVariation = 0.1f)
		{
			ModAudio.PlaySfx("hermit_gun", volumeDb, pitchVariation);
		}

		public static void PlayGun2(float volumeDb = 5f, float pitchVariation = 0.1f)
		{
			ModAudio.PlaySfx("hermit_gun2", volumeDb, pitchVariation);
		}

		public static void PlayGun3(float volumeDb = 5f, float pitchVariation = 0.1f)
		{
			ModAudio.PlaySfx("hermit_gun3", volumeDb, pitchVariation);
		}

		public static void PlaySpin(float volumeDb = 5f, float pitchVariation = 0.15f)
		{
			ModAudio.PlaySfx("hermit_spin", volumeDb, pitchVariation);
		}

		public static void PlayReload(float volumeDb = 5f, float pitchVariation = 0.1f)
		{
			ModAudio.PlaySfx("hermit_reload", volumeDb, pitchVariation);
		}

		public const string Gun1 = "hermit_gun";

		public const string Gun2 = "hermit_gun2";

		public const string Gun3 = "hermit_gun3";

		public const string Spin = "hermit_spin";

		public const string Reload = "hermit_reload";

		public const float DefaultDb = 5f;

		public const float SpinPitchVariation = 0.15f;

		public const float GunPitchVariation = 0.1f;
	}
}




