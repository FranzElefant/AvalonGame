using SFML.System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Avalon.Entities
{
	public abstract class Shot : BehavioralEntity
	{
		protected bool isExpired = false;
		protected float baseDamage;
		protected float baseSpeed;
		protected float maxSpeed;
		abstract public float Hit();
		public bool IsExpired
		{
			get
			{
				return isExpired;
			}
		}
	}
}
