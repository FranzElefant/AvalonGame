using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Avalon
{
	public abstract class Entity
	{
		protected string id;
		protected float size; // условный размер любой сущности
		protected Shape shape;
		protected long entityTime;

		abstract public void Draw(RenderWindow window, bool textureActive);
		abstract public void Update(float dt, Stopwatch sw);

		public virtual void UpdateTextures(bool loadTextures, Texture texture)
		{
			if (loadTextures)
			{
				shape.Texture = texture;
			}
			else if (shape.Texture != null && !loadTextures)
			{
				shape.Texture.Dispose();
			}
		}

		/// <summary>
		/// Получение крайних координат объекта
		/// </summary>
		public List<Vector2f> GetVertices()
		{
			List<Vector2f> points = new List<Vector2f> { };
			for (uint i = 0; i < shape.GetPointCount(); i++)
			{
				points.Add(shape.Transform.TransformPoint(shape.GetPoint(i)));
			}
			return points;
		}

		public string Id { get => id; set => id = value; }

		public Vector2f Position
		{
			 get => shape.Position; set => shape.Position = value;
		}

		public float Rotation
		{
			get => shape.Rotation; set => shape.Rotation = value;
		}

		public float Size
		{
			get => size; set => size = value;
		}
	}
}