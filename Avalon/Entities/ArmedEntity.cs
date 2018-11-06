using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalon;
using Avalon.Core;

namespace Avalon.Entities
{
	public abstract class ArmedEntity : BehavioralEntity
	{
		protected Weapon primaryWeapon;
		protected Weapon secondaryWeapon;
		protected bool usePrimaryWeapon;
		protected bool useSecondaryWeapon;

		public abstract void Shoot(Dictionary<string, Shot> dictWeapon);

		public bool UsePrimaryWeapon
		{
			get => usePrimaryWeapon; set => usePrimaryWeapon = value;
		}

		public bool UseSecondaryWeapon
		{
			get => useSecondaryWeapon; set => useSecondaryWeapon = value;
		}
	}
}
