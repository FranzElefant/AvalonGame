using Avalon.Sounds;
using Avalon.Textures;
using SFML.Graphics;
using SFML.System;
using System;
using System.Diagnostics;

namespace Avalon.Entities
{
	public class Laser : Shot
	{
		private float lifeTimeCounter = 0.0f;
		private float maxShapeLifetime;	//время существования отрисовки

		// Static variable for unique ID creation
		private static long idCount = 0;

		public Laser(Vector2f p, float direction)
		{
			BoundReflection = false;
			movement = new Movement(this);
			reaction = new Reaction();
			#region Constants
			size = Constants.Laser.radius;
			var length = Constants.Laser.length;
			baseDamage = Constants.Laser.baseDamage;
			maxShapeLifetime = Constants.Laser.lifetime;
			#endregion

			Id = "L" + idCount.ToString();
			idCount++;

			shape = new RectangleShape(new Vector2f(size, length))
			{
				Origin = new Vector2f(size / 2, length),
				Position = p
			};

			movement.Rotation = direction;
			Vector2f components = new Vector2f((float)Math.Sin(movement.Rotation.ToRadians()), (float)Math.Cos(movement.Rotation.ToRadians()) * -1);
			SoundEngine.missleSound.Play();
		}

		public override void Draw(RenderWindow window, bool textures)
		{
			UpdateTextures(textures, TextureEngine.laserTexture);
			window.Draw(shape);
		}

		public override void Update(float dt, Stopwatch sw)
		{
			if (lifeTimeCounter > maxShapeLifetime) isExpired = true;
			lifeTimeCounter++;
			base.Update(dt, sw);
		}

		public override float Hit()
		{
			return baseDamage;
		}
	}
}
