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

		public Projectile(Vector2f p, Vector2f v, float rotation)
		{
			Id = "P" + idCount.ToString();
			idCount++;
			movement = new Movement(this);
			#region Constants
			size = Constants.Projectile.radius;
			baseSpeed = Constants.Projectile.speed;
			maxSpeed = Constants.Projectile.maxSpeed;
			maxLifetime = Constants.Projectile.maxLifetime;
			baseDamage = Constants.Projectile.baseDamage;
			BoundReflection = false;
			#endregion

			shape = new RectangleShape(new Vector2f(size, size * 7))
			{
				Origin = new Vector2f(size / 2, size * 2),
				Position = p
			};

			movement.Rotation = rotation;
			Vector2f components = new Vector2f((float)Math.Sin(movement.Rotation.ToRadians()), (float)Math.Cos(movement.Rotation.ToRadians()) * -1);
			float scale = v.AbsoluteValue() / 100 + baseSpeed;
			if (scale > maxSpeed) 
			scale = maxSpeed;
			movement.Speed = components * scale;
			SoundEngine.missleSound.Play();
		}

		public override void Draw(RenderWindow window, bool textures)
		{
			UpdateTextures(textures, TextureEngine.missleTexture);
			if (!InWindowBounds()) isExpired = true;
			window.Draw(shape);
		}

		public override void Update(float dt, Stopwatch sw)
		{
			if (lifeTimeCounter > maxLifetime) isExpired = true;
			lifeTimeCounter++;
			base.Update(dt, sw);
		}

		public bool IsExpired
		{
			get
			{
				return isExpired;
			}
		}

		public override float Hit()
		{
			return baseDamage;
		}
	}
}
