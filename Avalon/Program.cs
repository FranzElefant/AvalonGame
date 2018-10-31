using Avalon.Sounds;
using Avalon.Textures;
using SFML.Graphics;


namespace Avalon
{
    class Program
    {
        static void Main(string[] args)
        {
			Avalon asteroidGame = new Avalon(1920, 1500, "Asteroids", Color.Black);
			asteroidGame.Run();
		}
    }
}
