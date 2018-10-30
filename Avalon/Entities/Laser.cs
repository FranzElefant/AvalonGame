using Avalon.Sounds;
using Avalon.Textures;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Avalon.Game;

namespace Avalon.Entities
{
	public class Laser : Weapon
	{
		private float heading;
		private float lifeTimeCounter = 0.0f;
		private float maxLifetime;

		bool isExpired = false;

		// Static variable for unique ID creation
		private static long idCount = 0;

		public Laser(Vector2f p, float direction)
		{
			#region Constants
			radius = Constants.Laser.radius;
			length = Constants.Laser.length;
			baseDamage = Constants.Laser.baseDamage;
			maxLifetime = Constants.Laser.lifetime;
			#endregion

			Id = "L" + idCount.ToString();
			idCount++;

			shape = new RectangleShape(new Vector2f(radius, length))
			{
				Origin = new Vector2f(radius / 2, length),
				Position = p
			};
			texture = TextureEngine.laserTexture;

			heading = direction.ToRadians();
			shape.Rotation = direction;
			Vector2f components = new Vector2f((float)Math.Sin(heading), (float)Math.Cos(heading) * -1);
			SoundEngine.missleSound.Play();
		}
		public override void Draw(RenderWindow window, bool textures)
		{
			if (texture != null && textures)
			{
				shape.Texture = texture;
			}
			else
			{
				shape.Texture.Dispose();
			}
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
