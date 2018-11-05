using Avalon.Textures;
using SFML.Graphics;
using SFML.System;

namespace Avalon.Entities
{
	class Asteroid : BehavioralEntity
	{
		private static long asteroidsCount = 0;
		private float baseSpeed;
		private float minRadiusForBreakApart;
		private int textureN;

		public float Radius { get => size; }

		public Asteroid(Vector2f p, Vector2f v, int r, int textureNumber)
		{
			movement = new Movement(this);
			reaction = new Reaction();
			#region Constants
			baseSpeed = Constants.Asteroid.baseSpeed;
			minRadiusForBreakApart = Constants.Asteroid.minRadiusForBreakApart;
			#endregion

			Id = "A" + asteroidsCount.ToString();
			asteroidsCount++;

			size = r;

			movement.Speed = v / size + v / v.AbsoluteValue() * baseSpeed;

			shape = new CircleShape(size)
			{
				Position = p
			};
			textureN = textureNumber;
		}

		public override void Draw(RenderWindow window, bool textures)
		{
			UpdateTextures(textures, TextureEngine.asteroidTexture[textureN]);
			window.Draw(shape);
		}

		/// <summary>
		/// Проверка на разлом астероида
		/// </summary>
		public bool WillBreakApart()
		{
			if (size > minRadiusForBreakApart)
			{
				return true;
			}
			return false;
		}
	}
}