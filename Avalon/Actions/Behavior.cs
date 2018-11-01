using SFML.System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Avalon.Game;

namespace Avalon
{
	public class Behavior
	{
		private Movement movement;
		private Reaction reaction;
		private bool boundReflection;

		public Behavior() { }

		public Behavior(Movement movement)
		{
			this.movement = movement;
		}

		/// <summary>
		/// Получение границы экрана, которую пересекли
		/// </summary>
		virtual public Edge GetCrossedBound(Entity entity)
		{
			if ((entity.Position.X + entity.Size) < 0.0) return Edge.LEFT;
			else if ((entity.Position.X - entity.Size) > Avalon.GetGameInstance().Window().Size.X) return Edge.RIGHT;
			else if ((entity.Position.Y + entity.Size) < 0) return Edge.UP;
			else if ((entity.Position.Y - entity.Size) > Avalon.GetGameInstance().Window().Size.X) return Edge.DOWN;
			else return Edge.NULL;
		}

		/// <summary>
		/// Пересечение границы экрана
		/// </summary>
		virtual protected void CrossingEdge(Entity entity)
		{
			Edge edge = GetCrossedBound(entity);
			var window = Avalon.GetGameInstance().Window();
			if (edge != Edge.NULL && boundReflection)
			{
				if (edge == Edge.LEFT) entity.Position = new Vector2f(window.Size.X + entity.Size, entity.Position.Y);
				else if (edge == Edge.RIGHT) entity.Position = new Vector2f(-entity.Size, entity.Position.Y);
				else if (edge == Edge.UP) entity.Position = new Vector2f(entity.Position.X, window.Size.Y + entity.Size);
				else entity.Position = new Vector2f(entity.Position.X, -entity.Size);
			}
		}

	}
}