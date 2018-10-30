using SFML.Graphics;
using System.Collections.Generic;

namespace Avalon.Textures
{
	public static class TextureEngine
	{

		public static Texture ufoTexture;
		public static Texture shipTexture;
		public static Texture spaceTexture;
		public static Texture flameTexture;
		public static Texture laserTexture;
		public static Texture missleTexture;
		public static List<Texture> asteroidTexture;
		public static List<Texture> lifeBarTexture;

		public static void Init()
		{
			Image image = new Image(@"Resources\ufo.png");
			ufoTexture = new Texture(image);
			image = new Image(@"Resources\ship.png");
			shipTexture = new Texture(image);
			image = new Image(@"Resources\space.jpg");
			spaceTexture = new Texture(image);
			image = new Image(@"Resources\flame.png");
			flameTexture = new Texture(image);
			image = new Image(@"Resources\laser.png");
			laserTexture = new Texture(image);
			image = new Image(@"Resources\missle.png");
			missleTexture = new Texture(image);
			asteroidTexture = new List<Texture>();
			for (int i = 1; i < 6; i++)
			{
				Image asterImage = new Image(@"Resources\Asteroids\" + i.ToString() + ".png");
				asteroidTexture.Add(new Texture(asterImage));
			}
			lifeBarTexture = new List<Texture>();
			for (int i = 0; i <= 100; i+=10)
			{
				Image lifeImage = new Image(@"Resources\Lifebar\" + i.ToString() + ".png");
				lifeBarTexture.Add(new Texture(lifeImage));
			}
		}
	}
}
