using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Diagnostics;
using static Avalon.Game;

namespace Avalon
{
	public abstract class Entity
	{
		protected string id;
		protected Shape shape;
		protected Vector2f speed;
		protected Texture texture;

		abstract public void Draw(RenderWindow window, bool textureActive);
		abstract public void Update(float dt, Stopwatch sw);

		virtual protected void AddTexture(Texture t)
		{
			shape.Texture = t;
		}
		/// <summary>
		/// Проверка пересечения границ экрана для сущности
		/// </summary>
		virtual protected Edge CheckBound(Window window, float nominalShapeSize)
		{
			if ((shape.Position.X + nominalShapeSize) < 0.0) return Edge.LEFT;
			else if ((shape.Position.X - nominalShapeSize) > window.Size.X) return Edge.RIGHT;
			else if ((shape.Position.Y + nominalShapeSize) < 0) return Edge.UP;
			else if ((shape.Position.Y - nominalShapeSize) > window.Size.Y) return Edge.DOWN;
			else return Edge.NULL;
		}
		/// <summary>
		/// Перерисовка объекта с обратной стороны после пересечения границы экрана
		/// </summary>
		virtual protected void CrossingEdge(Edge edge, Window window, float nominalShapeSize)
		{
			if (edge == Edge.LEFT) shape.Position = new Vector2f(window.Size.X + nominalShapeSize, shape.Position.Y);
			else if (edge == Edge.RIGHT) shape.Position = new Vector2f(-nominalShapeSize, shape.Position.Y);
			else if (edge == Edge.UP) shape.Position = new Vector2f(shape.Position.X, window.Size.Y + nominalShapeSize);
			else shape.Position = new Vector2f(shape.Position.X, -nominalShapeSize);
		}

		public string Id { get => id; set => id = value; }

		virtual public Vector2f GetPosition()
		{
			return shape.Position;
		}
	}
}