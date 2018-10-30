using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalon.Entities
{
	public abstract class Weapon : Entity
	{
		protected float radius;
		protected float length;
		protected float baseSpeed;
		protected float maxSpeed;
		protected float baseDamage;
		protected float direction; //напрвление

		abstract public float HitDamage();
		public virtual List<Vector2f> GetVertices()
		{
			List<Vector2f> points = new List<Vector2f> { };
			for (Byte i = 0; i < shape.GetPointCount(); i++)
			{
				points.Add(shape.Transform.TransformPoint(shape.GetPoint(i)));
			}
			return points;
		}
	}
}
