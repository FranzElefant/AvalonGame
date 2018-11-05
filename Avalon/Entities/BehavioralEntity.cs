using Avalon.Entities;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Avalon.Game;

namespace Avalon
{
	public class BehavioralEntity : Entity
	{
		protected Movement movement;
		protected Reaction reaction;
		protected Target target;
		protected float health;
		private bool boundReflection = true;

		public override void Draw(RenderWindow window, bool textureActive)
		{
			throw new NotImplementedException();
		}

		public override void Update(float dt, Stopwatch sw)
		{
			if (target != null && target.active) movement.SetTargetPointSpeed(target);
			movement.Move(dt);
			if (boundReflection) movement.CrossingEdge();
		}

		public virtual void SetTarget(Entity t, bool inversion)
		{
			target = new Target(t, inversion);
		}

		public virtual void DeleteTarget(Entity t, sbyte direction)
		{
			target = null;
		}

		/// <summary>
		/// Проверка пересечения границ экрана для сущности
		/// </summary>
		public bool InWindowBounds()
		{
			if (movement.GetCrossedBound() == Edge.NULL) return true;
			return false;
		}

		public bool BoundReflection
		{
			get 
			{
				return boundReflection;
			}
			set 
			{
				boundReflection = value;
			}
		}

		public float Health
		{
			get
			{
				return health;
			}
			set
			{
				health = value;
			}
		}

		/// <summary>
		/// Проверка пересечения одной сущности с другой
		/// </summary>
		public bool HasCollided(Entity s)
		{
			List<Vector2f> shipVertices = s.GetVertices(); //Точки корабля
			Vector2f c = shape.Position;
			for (int i = 0; i < shipVertices.Count; i++)
			{
				for (int j = i; j < shipVertices.Count; j++)
				{
					if (i != j)
					{
						if (VectorExtension.Intersect(shipVertices[i], shipVertices[j], c, size)) return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Проверка уничтожен ли объект оружием + нанесение урона
		/// </summary>
		public bool HasDamaged(Weapon proj)
		{
			List<Vector2f> shipVertices = proj.GetVertices(); //Точки снаряда
			Vector2f c = shape.Position;
			for (int i = 0; i < shipVertices.Count; i++)
			{
				for (int j = i; j < shipVertices.Count; j++)
				{
					if (i != j)
					{
						if (VectorExtension.Intersect(shipVertices[i], shipVertices[j], c, size))
						{
							Health -= proj.Hit();
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}