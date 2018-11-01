using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalon
{
	public class Movement
	{
		protected Trajectory trajectory;
		protected Vector2f speed; 
		protected float angularSpeed;		//скорость вращения

		public Movement()
		{
			trajectory = new LineTrajectory();
		}

		public Movement(Trajectory t)
		{
			trajectory = t;
		}

		public void UpdatePosition(ref Vector2f position)
		{
			trajectory.UpdateSpeed();
		}
	}
}
