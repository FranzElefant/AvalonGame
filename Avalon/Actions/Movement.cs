using Avalon.Entities;
using SFML.System;
using System;
using static Avalon.Game;

namespace Avalon
{
	public class Movement
	{
		protected Entity e;
		protected Trajectory Trajectory;
		public Vector2f Speed;
		public float Rotation { set; get; }
		public float AngularSpeed	{ set; get; }						//скорость вращения
		public float SpeedDecayRate { set; get; } = 1.0f;				//Настройка инерции движения
		public float AngularDecayRate  { set; get; } = 1.0f;			//Затухание инерции вращения
		public float SpeedLimit		{ set; get; }                       //предел скорости
		public float AngularSpeedLimit   { set; get; }					//предел вращения объекта

		public float RotationPower { set; get; } = 0.0f;				//Сила вращения
		public float AccelerationPower { set; get; } = 0.0f;			//Ускорение

		public Movement(Entity e)
		{
			this.e = e;
			Trajectory = new LineTrajectory();
		}

		public Movement(Entity e, Vector2f speed, Trajectory t)
		{
			this.e = e;
			Trajectory = t;
			this.Speed = speed;
		}

		public Movement(Entity e, Vector2f speed, float rotation)
		{
			this.e = e;
			Speed = speed;
			Rotation = rotation;
		}

		/// <summary>
		/// Установить скорость объект для преследования
		/// </summary>
		/// <param name="direction">1 - towards object, 0 - backward</param>
		public void SetTargetPointSpeed(Target t)
		{
			Vector2f targetOrigin = t.entity.Position;
			var absoluteSpeed = Speed.AbsoluteValue();
			Vector2f targetSpeed = new Vector2f(targetOrigin.X - e.Position.X, targetOrigin.Y - e.Position.Y);
			Speed = targetSpeed.Normalize(absoluteSpeed, t.inversion);
		}

		/// <summary>
		/// Движение
		/// </summary>
		public void Move(float dt)
		{
			//Изменение показателей
			e.Position = Trajectory.GetNextPoint(e.Position,Speed);
			Rotation += AngularSpeed * dt;
			e.Rotation = Rotation;

			//Затухание движения (инерция)
			Speed = Speed * SpeedDecayRate;
			AngularSpeed = AngularSpeed * AngularDecayRate;
		}

		/// <summary>
		/// Ускорить объект в выбранном направлении
		/// </summary>
		public void Accelerate(sbyte direction)
		{
			float headingRads = e.Rotation.ToRadians();
			float xNew = (float)Math.Sin(headingRads) * AccelerationPower * direction;
			float yNew = (float)Math.Cos(headingRads) * AccelerationPower * direction;
			if (Speed.AbsoluteValue() < SpeedLimit)
			{
				Speed = new Vector2f(Speed.X - xNew, Speed.Y + yNew);
			}
		}

		/// <summary>
		/// Повернуть объект
		/// </summary>
		public void Rotate(sbyte direction)
		{
			if (Math.Abs(AngularSpeed) < AngularSpeedLimit) AngularSpeed += RotationPower * direction;
		}

		/// <summary>
		/// Получение границы экрана, которую пересекли
		/// </summary>
		virtual public Edge GetCrossedBound()
		{
			if ((e.Position.X + e.Size) < 0.0) return Edge.LEFT;
			else if ((e.Position.X - e.Size) > Avalon.GetGameInstance().Window().Size.X) return Edge.RIGHT;
			else if ((e.Position.Y + e.Size) < 0) return Edge.UP;
			else if ((e.Position.Y - e.Size) > Avalon.GetGameInstance().Window().Size.Y) return Edge.DOWN;
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
	}
}
