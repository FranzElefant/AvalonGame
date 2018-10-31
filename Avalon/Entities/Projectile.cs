using Avalon.Sounds;
using Avalon.Textures;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Avalon.Game;

namespace Avalon.Entities
{
	public class Projectile : Weapon
	{
		private bool isExpired = false;
		private float lifeTimeCounter = 0.0f;
		private float maxLifetime;

		private static long idCount = 0;

		public Projectile(Vector2f p, Vector2f v, float direction)
		{
			#region Constants
			radius = Constants.Projectile.radius;
			baseSpeed = Constants.Projectile.speed;
			maxSpeed = Constants.Projectile.maxSpeed;
			maxLifetime = Constants.Projectile.maxLifetime;
			baseDamage = Constants.Projectile.baseDamage;
			#endregion

			Id = "P" + idCount.ToString();
			idCount++;

			shape = new RectangleShape(new Vector2f(radius, radius * 7))
			{
				Origin = new Vector2f(radius / 2, radius * 2),
				Position = p
			};

			this.direction = direction.ToRadians();
			shape.Rotation = direction;
			Vector2f components = new Vector2f((float)Math.Sin(this.direction), (float)Math.Cos(this.direction) * -1);
			float scale = v.AbsoluteValue() / 100 + baseSpeed;
			if (scale > maxSpeed) scale = maxSpeed;
			speed = components * scale;
			SoundEngine.missleSound.Play();
		}

		public override void Draw(RenderWindow window, bool textures)
		{
			UpdateTextures(textures, TextureEngine.missleTexture);
			if (CheckBound(window, radius) != Edge.NULL) isExpired = true;
			window.Draw(shape);
		}

		public override void Update(float dt, Stopwatch sw)
		{
			if (lifeTimeCounter > maxLifetime) isExpired = true;
			lifeTimeCounter++;
			shape.Position += speed;
		}

		public bool IsExpired
		{
			get
			{
				return isExpired;
			}
		}

		public override float HitDamage()
		{
			return baseDamage;
		}
	}
}
