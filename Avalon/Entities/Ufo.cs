using Avalon.Textures;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Avalon.Game;

namespace Avalon.Entities
{
	class Ufo : BehavioralEntity
	{
		private static long ufoCount = 0;

		private Shape lifeBar;
		public float Radius { get => size; }

		public Ufo(Vector2f p, Vector2f v)
		{
			movement = new Movement(this);
			reaction = new Reaction();

			#region Constants
			size = Constants.Ufo.baseSize;
			Health = Constants.Ufo.health;
			#endregion

			this.Id = "UFO" + ufoCount.ToString();
			ufoCount++;

			movement.Speed = v;
			Vector2f o = new Vector2f(size, size);
			shape = new CircleShape(size)
			{
				Origin = o,
				Position = p
			};
			// Полоса здоровья
			lifeBar = new RectangleShape(new Vector2f(size * 2, 10))
			{
				Origin = o,
				Position = new Vector2f(p.X, p.Y - size*0.8f),
			};
		}

		public override void UpdateTextures(bool loadTextures, Texture texture)
		{
			if (loadTextures)
			{
				shape.Texture = texture;
				lifeBar.Texture = TextureEngine.lifeBarTexture[(int)(Health / 10)];
			}
			else if (shape.Texture != null && !loadTextures)
			{
				shape.Texture.Dispose();
				lifeBar.Texture.Dispose();
			}
		}

		public override void Draw(RenderWindow window, bool textures)
		{
			UpdateTextures(textures, TextureEngine.ufoTexture);
			window.Draw(shape);
			window.Draw(lifeBar);
		}

		public override void Update(float dt, Stopwatch sw)
		{
			base.Update(dt, sw);
			lifeBar.Position = new Vector2f(shape.Position.X, shape.Position.Y - size);
		}
	}
}
