using Avalon.Core;
using Avalon.Entities;
using Avalon.Textures;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Avalon.Game;

namespace Avalon
{
	class Ship : Entity
	{
		//Констатнты
		private int shotChargingTime; //Время для зарядки выстрелов из плазмогана
		private float rotationPower; //Сила вращения
		private float accelerationPower; //Ускорение

		private float shipSize;			//Условный размер корабля

		private bool hasThrust = false; //Ускоряется в данный момент
		private bool hasSpin = false;   //Врщается в данный момент
		private bool isShotCharged = false;
		private bool wantsToShoot = false;
		private bool wantsToLaser = false;
		private long shipTime;
		private long lastShotTimeStamp;
		private long laserReserve; //Запас мощности лазера
		private long laserLifetime; //Запас мощности лазера
		private float laserActivationPercent; //Запас мощности лазера

		private float angularChangeSpeed;

		private Shape jetShape; //прорисовка двигателя
		private Texture jetShapeTexture; //прорисовка двигателя

		public Ship(Vector2f p)
		{
			Id = "S1";
			#region Constants
			shipSize = Constants.Ship.shipSize;

			shotChargingTime = Constants.Ship.shotChargingTime;
			rotationPower = Constants.Ship.rotationPower;
			accelerationPower = Constants.Ship.accelerationPower;
			laserReserve = Constants.Ship.laserLifeTime;
			laserLifetime = Constants.Ship.laserLifeTime;
			laserActivationPercent = Constants.Ship.laserActivationPercent;

			//Инерция
			speedLimit = Constants.Ship.speedLimit;
			decayRate = Constants.Ship.decayRate;
			angularDecayRate = Constants.Ship.angularDecayRate;
			angularSpeedLimit = Constants.Ship.angularSpeedLimit;
			#endregion

			shape = new CircleShape(shipSize, 3)
			{
				Scale = new Vector2f(1.3f, 1),
				Origin = new Vector2f(shipSize, shipSize),
				Position = p
			};
			speed = new Vector2f(0, 0); //направление скорости

			// Двигатель
			jetShape = new CircleShape(shipSize / 2, 3)
			{
				Scale = new Vector2f(0.6f, 1),
				Origin = new Vector2f(shipSize / 2, shipSize / 2.5f),
				Rotation = shape.Rotation + 180f,
				Position = p
			};
		}

		public override void UpdateTextures(bool loadTextures, Texture texture)
		{
			if (loadTextures)
			{
				shape.Texture = texture;
				jetShape.Texture = TextureEngine.flameTexture;
			}
			else if (shape.Texture != null && !loadTextures)
			{
				shape.Texture.Dispose();
				jetShape.Texture.Dispose();
			}
		}

		public override void Draw(RenderWindow window, bool textures)
		{
			UpdateTextures(textures, TextureEngine.shipTexture);
			Edge curEdge = CheckBound(window, shipSize / 2);
			if (curEdge != Edge.NULL) CrossingEdge(curEdge, window, shipSize / 2);
			if (hasThrust) window.Draw(jetShape);
			window.Draw(shape);
			hasThrust = false;
			hasSpin = false;
		}

		public override void Update(float dt, Stopwatch sw)
		{
			if (Keyboard.IsKeyPressed(Keyboard.Key.Up)) Acceleration(-1);

			if (Keyboard.IsKeyPressed(Keyboard.Key.Right)) Rotate(1);
			else if (Keyboard.IsKeyPressed(Keyboard.Key.Left)) Rotate(-1);

			if (Keyboard.IsKeyPressed(Keyboard.Key.Space) && isShotCharged) wantsToShoot = true;
			else ChargeShot(sw.ElapsedMilliseconds);

			if (Keyboard.IsKeyPressed(Keyboard.Key.LControl) && laserReserve > 0 && wantsToLaser) wantsToLaser = true;
			else if (Keyboard.IsKeyPressed(Keyboard.Key.LControl) && laserReserve/(laserLifetime*1.0)*100.0 > laserActivationPercent) wantsToLaser = true;
			else
			{
				ChargeLaser(sw.ElapsedMilliseconds);
				wantsToLaser = false;
			}

			shipTime = sw.ElapsedMilliseconds;

			Kinematics(dt);

			jetShape.Position = GetEnginePostion();
			jetShape.Rotation = shape.Rotation + 180f;
		}
		/// <summary>
		/// Направление ускорения корабля
		/// </summary>
		public void Acceleration(sbyte direction)
		{
			float headingRads = shape.Rotation.ToRadians();
			float xNew = (float)Math.Sin(headingRads) * accelerationPower * direction;
			float yNew = (float)Math.Cos(headingRads) * accelerationPower * direction;

			if (speed.AbsoluteValue() < speedLimit)
			{
				speed = new Vector2f(speed.X - xNew, speed.Y + yNew);
				hasThrust = true;
			}
		}
		public void Rotate(sbyte direction)
		{
			if (Math.Abs(angularChangeSpeed) < angularSpeedLimit) angularChangeSpeed += rotationPower * direction;
			hasSpin = true;
		}
		/// <summary>
		/// Инерция корабля
		/// </summary>
		private void Kinematics(float dt)
		{
			//Изменение показателей
			shape.Position += speed * dt;
			shape.Rotation += angularChangeSpeed * dt;

			//Затухание движения ()
			if (!hasThrust) speed = speed * decayRate;
			if (!hasSpin) angularChangeSpeed = angularChangeSpeed * angularDecayRate;
		}

		public void Shoot(Dictionary<string, Projectile> dictProjectiles)
		{
			Projectile pleft = new Projectile(GetGunPositionLeft(), speed, shape.Rotation);
			string key = pleft.Id;
			dictProjectiles.Add(key, pleft);

			Projectile pright = new Projectile(GetGunPositionRight(), speed, shape.Rotation);
			key = pright.Id;
			dictProjectiles.Add(key, pright);

			wantsToShoot = false;
			isShotCharged = false;
			lastShotTimeStamp = shipTime;
		}

		public void LaserAttack(Dictionary<string, Laser> dictLasers)
		{
			Laser l = new Laser(GetLaserPosition(), shape.Rotation);
			laserReserve -= 50;
			string key = l.Id;
			dictLasers.Clear();
			dictLasers.Add(key, l);
		}

		/// <summary>
		/// Получение крайних координат объекта
		/// </summary>
		public List<Vector2f> GetVertices()
		{
			List<Vector2f> points = new List<Vector2f> { };
			for (Byte i = 0; i < shape.GetPointCount(); i++)
			{
				points.Add(shape.Transform.TransformPoint(shape.GetPoint(i)));
			}
			return points;
		}

		private void ChargeShot(long milisecondsPassed)
		{
			if (milisecondsPassed - lastShotTimeStamp >= shotChargingTime) isShotCharged = true;
			wantsToShoot = false;
		}

		private void ChargeLaser(long milisecondsPassed)
		{
			if (laserReserve<laserLifetime) laserReserve += (milisecondsPassed - shipTime)/3;
		}

		/// <summary>
		/// Нос корабля (место орудия)
		/// </summary>
		private Vector2f GetGunPositionLeft()
		{
			Vector2f p1 = shape.Transform.TransformPoint(shape.GetPoint(1));
			Vector2f p3 = shape.Transform.TransformPoint(shape.GetPoint(3));
			Vector2f d2 = (p3 - p1) / 1.5f;
			return p1 + d2;
		}

		/// <summary>
		/// Нос корабля (место орудия)
		/// </summary>
		private Vector2f GetGunPositionRight()
		{
			Vector2f p2 = shape.Transform.TransformPoint(shape.GetPoint(2));
			Vector2f p3 = shape.Transform.TransformPoint(shape.GetPoint(3));
			Vector2f d2 = (p3 - p2) / 1.5f;
			return p2 + d2;
		}

		/// <summary>
		/// Левый борт корабля (место лазера)
		/// </summary>
		private Vector2f GetLaserPosition()
		{
			Vector2f p1 = shape.Transform.TransformPoint(shape.GetPoint(1));
			Vector2f p2 = shape.Transform.TransformPoint(shape.GetPoint(2));
			Vector2f p3 = shape.Transform.TransformPoint(shape.GetPoint(3));
			double offset = Math.Pow(p2.X, 2) + Math.Pow(p2.Y, 2);
			double bc = (Math.Pow(p1.X, 2) + Math.Pow(p1.Y, 2) - offset) / 2.0;
			double cd = (offset - Math.Pow(p3.X, 2) - Math.Pow(p3.Y, 2)) / 2.0;
			double det = (p1.X - p2.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p2.Y);

			double idet = 1 / det;

			float centerX = (float)((bc * (p2.Y - p3.Y) - cd * (p1.Y - p2.Y)) * idet);
			float centerY = (float)((cd * (p1.X - p2.X) - bc * (p2.X - p3.X)) * idet);
			return new Vector2f(centerX, centerY);
		}
		/// <summary>
		/// Место двигателя
		/// </summary>
		private Vector2f GetEnginePostion()
		{
			Vector2f p1 = shape.Transform.TransformPoint(shape.GetPoint(1));
			Vector2f p2 = shape.Transform.TransformPoint(shape.GetPoint(2));
			Vector2f d = (p1 - p2) / 2;
			return p2 + d;
		}
		public bool IsShotCharged
		{
			get
			{
				return isShotCharged;
			}
		}

		public bool WantsToShoot
		{
			get
			{
				return wantsToShoot;
			}
		}

		public bool WantsToLaser
		{
			get
			{
				return wantsToLaser;
			}
		}

		public ScreenText GetLaserChargePercent(Font font)
		{
			int percent = (int)Math.Round(laserReserve/ (laserLifetime * 1.0) * 100.0f);
			Color c = percent < laserActivationPercent ? Color.Red : Color.Green;
			string laserState = percent < laserActivationPercent ? "LASER CHARGING:" : "LASER CHARGED:";
			ScreenText scoreText = new ScreenText(laserState + percent.ToString() + "%", font, Constants.Fonts.bigFontSize, c);
			return scoreText;
		}
	}
}
