using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalon
{
	public static class NumericExtensions
	{
		public static float ToRadians(this float val)
		{
			return ((float)Math.PI / 180f) * val;
		}
	}
	public static class VectorExtension
	{
		/// <summary>
		/// Расстояние между точками
		/// </summary>
		public static float VectorDistance(this Vector2f v, Vector2f u)
		{
			return (float)Math.Sqrt(Math.Pow(v.X - u.X,2) + Math.Pow(v.Y - u.Y, 2));
		}

		/// <summary>
		/// Intersection, c - Circle
		/// </summary>
		public static bool Intersect(Vector2f pointA, Vector2f pointB, Vector2f pointC, float radius)
		{
			float dx, dy, A, B, C, det, t;

			dx = pointB.X - pointA.X;
			dy = pointB.Y - pointA.Y;

			A = dx * dx + dy * dy;
			B = 2 * (dx * (pointA.X - pointC.X) + dy * (pointA.Y - pointC.Y));
			C = (pointA.X - pointC.X) * (pointA.X - pointC.X) +
				(pointA.Y - pointC.Y) * (pointA.Y - pointC.Y) -
				radius * radius;

			det = B * B - 4 * A * C;
			if ((A <= 0.0000001) || (det < 0))
			{
				return false;
			}
			else if (det == 0)
			{
				// One solution.
				t = -B / (2 * A);
				var p1 = new Vector2f(pointA.X + t * dx, pointA.Y + t * dy);
				var p2 = new Vector2f(pointA.X + t * dx, pointA.Y + t * dy);
				if (p1.X < Math.Max(pointA.X, pointB.X) && p1.X > Math.Min(pointA.X, pointB.X)) return true;
				if (p2.X < Math.Max(pointA.X, pointB.X) && p2.X > Math.Min(pointA.X, pointB.X)) return true;
				if (p1.Y < Math.Max(pointA.Y, pointB.Y) && p1.Y > Math.Min(pointA.Y, pointB.Y)) return true;
				if (p2.Y < Math.Max(pointA.Y, pointB.Y) && p2.Y > Math.Min(pointA.Y, pointB.Y)) return true;
			}
			else
			{
				// Two solutions.
				t = (float)((-B + Math.Sqrt(det)) / (2 * A));
				var p1 = new Vector2f(pointA.X + t * dx, pointA.Y + t * dy);
				t = (float)((-B - Math.Sqrt(det)) / (2 * A));
				var p2 = new Vector2f(pointA.X + t * dx, pointA.Y + t * dy);
				if (p1.X < Math.Max(pointA.X, pointB.X) && p1.X > Math.Min(pointA.X, pointB.X)) return true;
				if (p2.X < Math.Max(pointA.X, pointB.X) && p2.X > Math.Min(pointA.X, pointB.X)) return true;
				if (p1.Y < Math.Max(pointA.Y, pointB.Y) && p1.Y > Math.Min(pointA.Y, pointB.Y)) return true;
				if (p2.Y < Math.Max(pointA.Y, pointB.Y) && p2.Y > Math.Min(pointA.Y, pointB.Y)) return true;
			}
			return false;
		}

		public static float AbsoluteValue(this Vector2f v)
		{
			double dotItself = v.X * v.X + v.Y * v.Y;
			return (float)Math.Sqrt(dotItself);
		}

		public static Vector2f Normalize(this Vector2f v, float absoluteValue, bool inversion)
		{
			if (v.Y != 0)
			{
				var c = v.X / v.Y;
				var yNew = (Math.Abs(v.Y)/ v.Y)*(float)Math.Sqrt(absoluteValue * absoluteValue / (c * c + 1.0f));
				var xNew = c * yNew;
				if (!inversion) return new Vector2f(xNew, yNew);
				return new Vector2f(-xNew, -yNew);
			}
			else
			{
				var yNew = 0.0f;
				var xNew = 0.0f;
				if (v.X>0) xNew = absoluteValue;
				else xNew = -absoluteValue;
				return new Vector2f(xNew, yNew);
			}
		}
	}
}
