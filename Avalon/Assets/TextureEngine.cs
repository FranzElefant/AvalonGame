using SFML.Graphics;
using System.Collections.Generic;

namespace Avalon.Textures
{
	public static class TextureEngine
	{
		private static string folder = @"res\Textures\";

		#region MemoryImages
		private static Image ufoImage;
		private static Image shipImage;
		private static Image spaceImage;
		private static Image flameImage;
		private static Image laserImage;
		private static Image missleImage;
		private static List<Image> asteroidImage;
		private static List<Image> lifeBarImage;
		#endregion

		#region Textures
		public static Texture ufoTexture;
		public static Texture shipTexture;
		public static Texture spaceTexture;
		public static Texture flameTexture;
		public static Texture laserTexture;
		public static Texture missleTexture;
		public static List<Texture> asteroidTexture;
		public static List<Texture> lifeBarTexture;
		#endregion

		public static void LoadImages()
		{
			ufoImage = new Image(folder + "ufo.png");
			shipImage = new Image(folder + "ship.png");
			spaceImage = new Image(folder + "space.jpg");
			flameImage = new Image(folder + "flame.png");
			laserImage = new Image(folder + "laser.png");
			missleImage = new Image(folder + "missle.png");
			asteroidImage = new List<Image>();
			for (int i = 1; i < 6; i++)
			{
				asteroidImage.Add(new Image(folder + i.ToString() + ".png"));
			}
			lifeBarImage = new List<Image>();
			for (int i = 0; i <= 100; i += 10)
			{
				lifeBarImage.Add(new Image(folder + @"\Lifebar\" + i.ToString() + ".png"));
			}
		}

		public static void Init()
		{
			ufoTexture = new Texture(ufoImage);
			shipTexture = new Texture(shipImage);
			spaceTexture = new Texture(spaceImage);
			flameTexture = new Texture(flameImage);
			laserTexture = new Texture(laserImage);
			missleTexture = new Texture(missleImage);
			asteroidTexture = new List<Texture>();
			for (int i = 0; i < 5; i++)
			{
				asteroidTexture.Add(new Texture(asteroidImage[i]));
			}
			lifeBarTexture = new List<Texture>();
			for (int i = 0; i <= 10; i++)
			{
				lifeBarTexture.Add(new Texture(lifeBarImage[i]));
			}
		}
	}
}
