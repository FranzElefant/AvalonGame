using Avalon.Entities;
using SFML.System;
using System;
using System.Diagnostics;

namespace Avalon
{
	class MissleGun : Weapon
	{
		protected long shotChargingTime;

		public MissleGun(): base()
		{
			shotChargingTime = Constants.MissleGun.shotChargingTime;
		}

		public MissleGun(long shotChargingTime): base()
		{
			this.shotChargingTime = shotChargingTime;
		}

		public override Shot Shoot(Vector2f gunPosition, Vector2f speed, float rotation)
		{
			Projectile proj = new Projectile(gunPosition, speed, rotation);
			isWeaponCharged = false;
			lastShotTime = Avalon.GetGameInstance().GameTimer().ElapsedMilliseconds;
			return proj;
		}

		public override void Charge(Object stateInfo)
		{
			var gameTime = Avalon.GetGameInstance().GameTimer().ElapsedMilliseconds;
			if (gameTime - lastShotTime >= shotChargingTime) isWeaponCharged = true;
		}
	}
}