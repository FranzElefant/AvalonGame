using SFML.System;

namespace Avalon
{
	public abstract class Trajectory
	{
		public Trajectory() {}

		public abstract Vector2f GetNextPoint(Vector2f position, Vector2f speed);
	}

	public class LineTrajectory : Trajectory
	{
		public LineTrajectory() : base() { }

		public override Vector2f GetNextPoint(Vector2f position, Vector2f speed)
		{
			return position + speed;
		}
	}

	public class ZigZagTrajectory : Trajectory
	{
		public float amplitude;
		public Vector2f startPoint;
		public Vector2f destination;
	
		public ZigZagTrajectory() : base() { }

		public override Vector2f GetNextPoint(Vector2f position, Vector2f speed)
		{
			return position + speed;
		}
	}

	public class RandomTrajectory : Trajectory
	{
		public RandomTrajectory() : base() { }

		public override Vector2f GetNextPoint(Vector2f position, Vector2f speed)
		{
			return position + speed;
		}
	}
}