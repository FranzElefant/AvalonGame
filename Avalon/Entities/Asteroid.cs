using Avalon.Textures;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using System.Diagnostics;
using static Avalon.Game;

namespace Avalon.Entities
{
	class Asteroid : Entity
	{
		private static long asteroidsCount = 0;

		private float radius;
		private float baseSpeed;
		private float minRadiusForBreakApart;
		private int textureN;

		public float Radius { get => radius; }

		public Asteroid(Vector2f p, Vector2f v, int r, int textureNumber)
		{
			#region Constants
			baseSpeed = Constants.Asteroid.baseSpeed;
			minRadiusForBreakApart = Constants.Asteroid.minRadiusForBreakApart;
			#endregion

			this.Id = "A" + asteroidsCount.ToString();
			asteroidsCount++;

			radius = r;

			speed = v / radius + v / v.AbsoluteValue() * baseSpeed;

			Vector2f o = new Vector2f(radius, radius);
			shape = new CircleShape(radius)
			{
				Origin = o,
				Position = p
			};
			textureN = textureNumber;
		}

		public override void Draw(RenderWindow window, bool textures)
		{
			UpdateTextures(textures, TextureEngine.asteroidTexture[textureN]);
			Edge curEdge = CheckBound(window, radius);
			if (curEdge != Edge.NULL) CrossingEdge(curEdge, window, radius);
			window.Draw(shape);
		}

		public override void Update(float dt, Stopwatch sw)
		{
			Kinematics(dt);
		}

		private void Kinematics(float dt)
		{
			shape.Position += speed;
		}

		public Vector2f GetCenterVertex()
		{
			return shape.Position;
		}
		/// <summary>
		/// Проверка пересечения с кораблем
		/// </summary>
		public bool HasCollided(Ship s)
		{
			List<Vector2f> shipVertices = s.GetVertices(); //Точки корабля
			Vector2f c = shape.Position;
			for (int i = 0; i < shipVertices.Count; i++)
			{
				for (int j = i; j < shipVertices.Count; j++)
				{
					if (i != j)
					{
						if (VectorExtension.Intersect(shipVertices[i], shipVertices[j], c, radius)) return true;
					}
				}
			}
			return false;
		}
		/// <summary>
		/// Проверка уничтожен ли астероид снарядом
		/// </summary>
		public bool ShouldExplode(Weapon proj)
		{
			List<Vector2f> shipVertices = proj.GetVertices(); //Точки снаряда
			Vector2f c = shape.Position;
			for (int i = 0; i < shipVertices.Count; i++)
			{
				for (int j = i; j < shipVertices.Count; j++)
				{
					if (i != j)
					{
						if (VectorExtension.Intersect(shipVertices[i], shipVertices[j], c, radius)) return true;
					}
				}
			}
			return false;
		}
		/// <summary>
		/// Проверка на разлом астероида
		/// </summary>
		public bool WillBreakApart()
		{
			if (radius > minRadiusForBreakApart)
			{
				return true;
			}
			return false;
		}
	}
}
