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
		protected float size; // условный размер любой сущности
		protected Shape shape;

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