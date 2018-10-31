using SFML.Audio;

namespace Avalon.Sounds
{
	public class SoundEngine
	{
		private static string folder = @"res\Sounds\";
		public static Sound explosionSound;
		public static Sound missleSound;
		public static void Init()
		{
			var explosionBuffer = new SoundBuffer(folder+"explosion1.wav");
			explosionSound = new Sound(explosionBuffer);
			var missleBuffer = new SoundBuffer(folder +"missle.wav");
			missleSound = new Sound(missleBuffer);
		}
	}
}
