using SFML.System;

namespace Avalon
{
	public abstract class Trajectory
	{
		public Trajectory() {}

		public abstract Vector2f GetNextPoint(Vector2f position, Vector2f speed);

		public abstract Vector2f GetNextPoint(Vector2f position, Vector2f speed, Vector2f target);
	}

	public class LineTrajectory : Trajectory
	{
		public LineTrajectory() : base() { }

		public override Vector2f GetNextPoint(Vector2f position, Vector2f speed)
		{
			return position + speed;
		}

		public override Vector2f GetNextPoint(Vector2f position, Vector2f speed, Vector2f target)
		{
			var absoluteSpeed = speed.AbsoluteValue();
			Vector2f targetSpeed = new Vector2f(target.X - position.X, target.Y - position.Y);
			speed = targetSpeed.Normalize(absoluteSpeed);
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

		public override Vector2f GetNextPoint(Vector2f position, Vector2f speed, Vector2f target)
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

		public override Vector2f GetNextPoint(Vector2f position, Vector2f speed, Vector2f targety)
		{
			return position + speed;
		}
	}
}