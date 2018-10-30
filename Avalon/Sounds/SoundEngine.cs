using SFML.Audio;

namespace Avalon.Sounds
{
	public class SoundEngine
	{
		public static Sound explosionSound;
		public static Sound missleSound;
		public static void Init()
		{
			var explosionBuffer = new SoundBuffer(@"Resources\Sounds\explosion1.wav");
			explosionSound = new Sound(explosionBuffer);
			var missleBuffer = new SoundBuffer(@"Resources\Sounds\missle.wav");
			missleSound = new Sound(missleBuffer);
		}
	}
}
