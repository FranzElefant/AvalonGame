using Avalon.Entities;
using SFML.System;
using System;
using System.Diagnostics;
using System.Threading;

namespace Avalon
{
	public abstract class Weapon
	{
		protected Timer timer;
		protected bool isWeaponCharged;
		protected long lastShotTime;
		protected long lastTime;
		public abstract void Charge(Object stateInfo);
		public abstract Shot Shoot(Vector2f weaponPosition, Vector2f speed, float rotation);

		protected Weapon()
		{
			var autoEvent = new AutoResetEvent(false);
			timer = new Timer(Charge, autoEvent, 0, 10);
		}

		public virtual bool IsWeaponCharged { get { return isWeaponCharged; } }
		public long LastShotTime { get { return lastShotTime; } }
		public virtual string GetTextInfo()
		{
			return string.Empty;
		}
	}
}
