using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Avalon.Game;

namespace Avalon
{
	public class Movement
	{
		protected Entity e;
		protected Trajectory trajectory;
		protected Vector2f speed;
		protected float rotation;
		protected float angularSpeed;			//скорость вращения
		protected float speedDecayRate;			//Настройка инерции движения
		protected float angularDecayRate;       //Затухание инерции вращения
		protected float speedLimit;				//предел скорости
		protected float angularSpeedLimit;      //предел вращения объекта

		private float rotationPower;			//Сила вращения
		private float accelerationPower;        //Ускорение

		public Movement()
		{
			trajectory = new LineTrajectory();
		}

		public Movement(Trajectory t)
		{
			trajectory = t;
		}

		public Movement(Vector2f speed, float rotation)
		{
			this.speed = speed;
			this.rotation = rotation;
		}

		/// <summary>
		/// Установить скорость объект для преследования
		/// </summary>
		/// <param name="direction">1 - towards object, 0 - backward</param>
		public void SetTargetPointSpeed(Entity target, sbyte direction)
		{
			Vector2f shipOrigin = e.Position;
			var absoluteSpeed = speed.AbsoluteValue();
			Vector2f targetSpeed = new Vector2f(shipOrigin.X - e.Position.X, shipOrigin.Y - e.Position.Y);
			speed = targetSpeed.Normalize(absoluteSpeed);
		}

		/// <summary>
		/// Движение
		/// </summary>
		public void Move(float dt)
		{
			//Изменение показателей
			e.Position += speed * dt;
			e.Rotation += angularSpeed * dt;

			//Затухание движения (инерция)
			speed = speed * speedDecayRate;
			angularSpeed = angularSpeed * angularDecayRate;
		}

		/// <summary>
		/// Ускорить объект в выбранном направлении
		/// </summary>
		public void Accelerate(sbyte direction)
		{
			float headingRads = rotation.ToRadians();
			float xNew = (float)Math.Sin(headingRads) * accelerationPower * direction;
			float yNew = (float)Math.Cos(headingRads) * accelerationPower * direction;
			if (speed.AbsoluteValue() < speedLimit)
			{
				speed = new Vector2f(speed.X - xNew, speed.Y + yNew);
			}
		}

		/// <summary>
		/// Повернуть объект
		/// </summary>
		public void Rotate(sbyte direction)
		{
			if (Math.Abs(angularSpeed) < angularSpeedLimit) angularSpeed += rotationPower * direction;
		}

		/// <summary>
		/// Получение границы экрана, которую пересекли
		/// </summary>
		virtual protected Edge GetCrossedBound()
		{
			if ((e.Position.X + e.Size) < 0.0) return Edge.LEFT;
			else if ((e.Position.X - e.Size) > Avalon.GetGameInstance().Window().Size.X) return Edge.RIGHT;
			else if ((e.Position.Y + e.Size) < 0) return Edge.UP;
			else if ((e.Position.Y - e.Size) > Avalon.GetGameInstance().Window().Size.X) return Edge.DOWN;
			else return Edge.NULL;
		}

		/// <summary>
		/// Пересечение границы экрана
		/// </summary>
		virtual public void CrossingEdge()
		{
			Edge edge = GetCrossedBound();
			var window = Avalon.GetGameInstance().Window();
			if (edge != Edge.NULL)
			{
				if (edge == Edge.LEFT) e.Position = new Vector2f(window.Size.X + e.Size, e.Position.Y);
				else if (edge == Edge.RIGHT) e.Position = new Vector2f(-e.Size, e.Position.Y);
				else if (edge == Edge.UP) e.Position = new Vector2f(e.Position.X, window.Size.Y + e.Size);
				else e.Position = new Vector2f(e.Position.X, -e.Size);
			}
		}

		/// <summary>
		/// Проверка пересечения границ экрана для сущности
		/// </summary>
		public bool InWindowBounds()
		{
			if (GetCrossedBound() == Edge.NULL) return true;
			return false;
		}
	}
}
