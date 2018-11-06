using Avalon.Entities;
using SFML.System;
using System;
using Avalon.Core;
using SFML.Graphics;

namespace Avalon
{
	class LaserGun : Weapon
	{
		protected long shotChargingTime;
		private long laserReserve; //Запас мощности лазера
		private long laserLifetime; //Запас мощности лазера
		private float laserActivationPercent; //Запас мощности лазера
		private bool laserActive;

		public LaserGun() : base()
		{
			laserReserve = Constants.LaserGun.laserLifeTime;
			laserLifetime = Constants.LaserGun.laserLifeTime;
			laserActivationPercent = Constants.LaserGun.laserActivationPercent;
		}

		public override Shot Shoot(Vector2f gunPosition, Vector2f speed, float rotation)
		{
			Laser l = new Laser(gunPosition, rotation);
			laserReserve -= 50;
			laserActive = true;
			return l;
		}

		public override void Charge(Object stateInfo)
		{
			var gameTime = Avalon.GetGameInstance().GameTimer().ElapsedMilliseconds;
			if (laserReserve < laserLifetime) laserReserve += (gameTime - lastTime) / 3;
			lastTime = gameTime;
		}

		public override bool IsWeaponCharged
		{
			get
			{
				var result = (laserReserve / (laserLifetime * 1.0) * 100.0 > laserActivationPercent || laserActive);
				isWeaponCharged = laserReserve > 0 && result;
				laserActive = false;
				return isWeaponCharged;
			}
		}

		public override string GetTextInfo()
		{
			int percent = (int)Math.Round(laserReserve / (laserLifetime * 1.0) * 100.0f);
			Color c = percent < laserActivationPercent ? Color.Red : Color.Green;
			string laserState = percent < laserActivationPercent ? "LASER CHARGING:" : "LASER CHARGED:";
			laserState = laserState + percent.ToString() + "%";
			return laserState;
		}
	}
}
