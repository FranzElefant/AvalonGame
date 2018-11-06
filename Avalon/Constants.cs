namespace Avalon
{
    public static class Constants
    {
		public class Rendering
		{
			public static uint frameRate = 60;
			public static float deltaTime = 0.02f;
			public static uint antialiasingLevel = 8;
			public static bool fullScreen = true;
		}

		public class Fonts
		{
			public static uint ultraBigFontSize = 60;
			public static uint bigFontSize = 30;
			public static uint middleFontSize = 15;
			public static uint littleFontSize = 10;
		}

		public class Asteroid
		{
			public const double spawnChance = .90;
			public const int minSize = 15;
			public const int maxSize = 50;
			public const int minSpeed = 3;
			public const int maxSpeed = 25;
			public const float baseSpeed = 5;
			public const float minRadiusForBreakApart = 30;
		}

		public class Ufo
		{
			public const double spawnChance = .50;
			public const float health = 100;
			public const int baseSize = 50;
			public const int minSpeed = 1;
			public const int maxSpeed = 4;

			public const float baseSpeed = 5;
		}

		public class Player
		{
			public const double difficulty = 2; //3 - средняя сложность (коэффициент), функция - логарифмическая
		}

		public class Ship
		{
			public const float shipSize = 50; //Время для зарядки выстрелов из плазмогана

			public const float rotationPower = 50; //Сила вращения
			public const float accelerationPower = 1; //Ускорение

			//Инерция
			public const float speedLimit = 500; //запас скорости
			public const float angularSpeedLimit = 350; //
			public const float speedDecayRate = 0.9f;        //Настройка инерции движение
			public const float angularDecayRate = 0.7f; //Затухание инерции вращения
		}

		public class Projectile
		{
			public const float radius = 15; //Размер хитбокса
			public const float speed = 10000;   //скорость начальная
			public const float maxSpeed = 50; //Максимальная скорость
			public const float maxLifetime = 100; //Время существования, такты перерисовки
			public const float baseDamage = 5; //Время существования
		}

		public class Laser
		{
			public const float radius = 30; //Размер хитбокса
			public const float length = 3000;   //длина лазера
			public const float baseDamage = 20;
			public const float lifetime = 1; //существование лазера, такты перерисовки
		}

		public class LaserGun
		{
			public const long laserLifeTime = 5000; //мс
			public const float laserActivationPercent = 70;
		}

		public class MissleGun
		{
			public const int shotChargingTime = 100; //Время для зарядки выстрелов из плазмогана (мс)
		}
	}
}