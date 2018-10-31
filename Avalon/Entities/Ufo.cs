using Avalon.Textures;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Avalon.Game;

namespace Avalon.Entities
{
	class Ufo : Entity
	{
		private static long ufoCount = 0;

		private int radius;

		private float healthPoints;
		private float baseSpeed;
		private Shape lifeBar;
		public float Radius { get => radius; }

		public Ufo(Vector2f p, Vector2f v)
		{
			#region Constants
			radius = Constants.Ufo.baseSize;
			healthPoints = Constants.Ufo.health;
			#endregion

			this.Id = "UFO" + ufoCount.ToString();
			ufoCount++;

			speed= v;
			baseSpeed = v.AbsoluteValue();
			Vector2f o = new Vector2f(radius, radius);
			shape = new CircleShape(radius)
			{
				Origin = o,
				Position = p
			};
			// Полоса здоровья
			lifeBar = new RectangleShape(new Vector2f(radius * 2, 10))
			{
				//Scale = new Vector2f(1f, 1),
				Origin = o,
				Position = new Vector2f(p.X, p.Y - radius*0.8f),
			};
		}

		public override void UpdateTextures(bool loadTextures, Texture texture)
		{
			if (loadTextures)
			{
				shape.Texture = texture;
				lifeBar.Texture = TextureEngine.lifeBarTexture[(int)(healthPoints / 10)];
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
			Edge curEdge = CheckBound(window, radius / 2);
			if (curEdge != Edge.NULL) CrossingEdge(curEdge, window, radius / 2);
			window.Draw(shape);
			window.Draw(lifeBar);
		}

		public override void Update(float dt, Stopwatch sw)
		{
			Kinematics(dt);
		}

		private void Kinematics(float dt)
		{
			shape.Position += speed;
			lifeBar.Position = new Vector2f(shape.Position.X, shape.Position.Y - radius);
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
						if (VectorExtension.Intersect(shipVertices[i], shipVertices[j], c, radius))
						{
							healthPoints -= proj.HitDamage();
							return true;
						}
					}
				}
			}
			return false;
		}

		public void ChooseShipTarget(Ship s)
		{
			Vector2f shipOrigin = s.GetPosition();
			var absoluteSpeed = speed.AbsoluteValue();
			Vector2f targetSpeed = new Vector2f(shipOrigin.X - shape.Position.X, shipOrigin.Y - shape.Position.Y);
			speed = targetSpeed.Normalize(absoluteSpeed);
		}

		public float GetHealth()
		{
			return healthPoints;
		}
	}
}
