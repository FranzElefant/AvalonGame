using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using static Avalon.Game;

namespace Avalon
{
	public class BehavioralEntity : Entity
	{
		public Movement movement;
		public Reaction reaction;

		protected bool hasTarget;
		protected Entity target;

		protected bool boundReflection;

		public override void Draw(RenderWindow window, bool textureActive)
		{
			throw new NotImplementedException();
		}

		public override void Update(float dt, Stopwatch sw)
		{
			if (hasTarget) movement.SetTargetPointSpeed(target);
			movement.Move(dt);
			if (boundReflection) movement.CrossingEdge();
		}
	}
}