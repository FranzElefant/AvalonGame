﻿using Avalon.Entities;
using Avalon.Textures;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalon
{
	class Avalon : Game
	{
		private Ship playerShip;
		private Player player;

		// Элементы, находящиеся на экране
		private Dictionary<string, Projectile> dictProjectiles;
		private Dictionary<string, Laser> dictLasers;
		private Dictionary<string, Asteroid> dictAsteroids;
		private Dictionary<string, Ufo> dictUfos;

		// Элементы, которые очищаем при следующем обновлении
		private HashSet<string> objectForDelete;
		// Сет разбитых астероидов
		private HashSet<Asteroid> partedAsteroids;

		private Random rnd;
		//Границы экрана
		private Edge[] edgeArray = (Edge[])Enum.GetValues(typeof(Edge));

		//Тексты на экране
		ScoreText scoreText;

		protected class ScoreText
		{
			public Text text;
			public ScoreText(string text, Font font, uint size, Color color)
			{
				this.text = new Text(text, font)
				{
					CharacterSize = size,
					FillColor = color
				};
			}

			public void UpdateText(string text)
			{
				this.text.DisplayedString = text;
			}
		}


		public Avalon(uint width, uint height, string title, Color clrColor) : base(width, height, title, clrColor)
		{
			//NULL - это показатель того, что элемент внутри экрана
			edgeArray = edgeArray.Where(x => x != Edge.NULL).ToArray();
			player = new Player();
			rnd = new Random();
			dictAsteroids = new Dictionary<string, Asteroid>();
			dictProjectiles = new Dictionary<string, Projectile>();
			dictLasers = new Dictionary<string, Laser>();
			dictUfos = new Dictionary<string, Ufo>();

			objectForDelete = new HashSet<string>();

			partedAsteroids = new HashSet<Asteroid>();

			backGround = new Sprite(TextureEngine.spaceTexture);

			scoreText = new ScoreText("Score: 0", windowFont, Constants.Fonts.bigFontSize, Color.White);
		}

		public override void CleanUp()
		{
			dictAsteroids.Clear();
			dictProjectiles.Clear();
			dictLasers.Clear();
			dictUfos.Clear();
			objectForDelete.Clear();
			partedAsteroids.Clear();
			playerShip = null;
		}

		public override void Init()
		{
			player = new Player();
			playerShip = new Ship(new Vector2f(window.Size.X / 2, window.Size.Y / 2));
			gameTimer.Restart();
		}

		public override void Restart()
		{
			lastGameScore = player.score;
			CleanUp();
			Init();
		}

		public override void Update(RenderWindow window, float dt)
		{
			Spawn();
			CollisionCheck();
			DeleteOldObjects();
			UpdateAndDraw();
		}
		/// <summary>
		/// Проверяем элементы на столкновение
		/// </summary>
		private void CollisionCheck()
		{
			//Parallel.ForEach(asteroidsCopy, (a) =>
			foreach (var a in dictAsteroids.Values)
			{
				if (a.HasCollided(playerShip))
				{
					isGameOver = true;
					Restart();
					return;
				}
				// Столкновение зарядов с астероидами
				foreach (Projectile p in dictProjectiles.Values)
				{
					if (p.IsExpired) objectForDelete.Add(p.Id);
					else if (a.ShouldExplode(p))
					{
						Sounds.SoundEngine.explosionSound.Play();
						objectForDelete.Add(a.Id);
						objectForDelete.Add(p.Id);
						if (a.WillBreakApart()) partedAsteroids.Add(a);
						player.score++;
					}
				}
				// Столкновение зарядов с астероидами
				foreach (Laser l in dictLasers.Values)
				{
					if (l.IsExpired) objectForDelete.Add(l.Id);
					else if (a.ShouldExplode(l))
					{
						Sounds.SoundEngine.explosionSound.Play();
						objectForDelete.Add(a.Id);
						objectForDelete.Add(l.Id);
						if (a.WillBreakApart()) partedAsteroids.Add(a);
						player.score++;
					}
				}
			}
			foreach (var u in dictUfos.Values)
			{
				u.ChooseShipTarget(playerShip);
				if (u.HasCollided(playerShip))
				{
					isGameOver = true;
					Restart();
					return;
				}
				// Столкновение зарядов с летающей тарелкой
				foreach (Projectile p in dictProjectiles.Values)
				{
					if (p.IsExpired) objectForDelete.Add(p.Id);
					else if (u.ShouldExplode(p))
					{
						objectForDelete.Add(p.Id);
						if (u.GetHealth() < 0.0)
						{
							Sounds.SoundEngine.explosionSound.Play();
							objectForDelete.Add(u.Id);
							player.score += 10;
						}
					}
				}
				foreach (Laser l in dictLasers.Values)
				{
					if (l.IsExpired) objectForDelete.Add(l.Id);
					else if (u.ShouldExplode(l))
					{
						objectForDelete.Add(l.Id);
						if (u.GetHealth() < 0.0)
						{
							Sounds.SoundEngine.explosionSound.Play();
							objectForDelete.Add(u.Id);
							player.score += 10;
						}
					}
				}
			}
		}
		/// <summary>
		/// Очистка уничтоженных объектов
		/// </summary>
		private void DeleteOldObjects()
		{
			foreach (string id in objectForDelete)
			{
				dictProjectiles.Remove(id);
				dictLasers.Remove(id);
				dictAsteroids.Remove(id);
				dictUfos.Remove(id);
			}
			objectForDelete.Clear();
		}

		/// <summary>
		/// Обновление положения и перерисовка
		/// </summary>
		private void UpdateAndDraw()
		{
			playerShip.Update(dt, gameTimer);
			if (playerShip.WantsToShoot && playerShip.IsShotCharged) playerShip.Shoot(dictProjectiles);
			if (playerShip.WantsToLaser && playerShip.IsShotCharged) playerShip.LaserAttack(dictLasers);
			foreach (Asteroid a in dictAsteroids.Values)
			{
				a.Update(dt, gameTimer);
				a.Draw(window, true);
			}
			foreach (Projectile p in dictProjectiles.Values)
			{
				p.Update(dt, gameTimer);
				p.Draw(window, true);
			}
			foreach (Laser l in dictLasers.Values)
			{
				l.Update(dt, gameTimer);
				l.Draw(window, true);
			}
			foreach (Ufo p in dictUfos.Values)
			{
				p.Update(dt, gameTimer);
				p.Draw(window, true);
			}
			playerShip.Draw(window, true);
			UpdateScore();
			window.Draw(scoreText.text);
		}


		private void Spawn()
		{
			foreach (Asteroid a in partedAsteroids)
			{
				Asteroid a1, a2;
				a1 = SpawnAsteroid(a.GetCenterVertex(), (int)a.Radius / 2);
				a2 = SpawnAsteroid(a.GetCenterVertex(), (int)a.Radius / 2);
				dictAsteroids.Add(a1.Id, a1);
				dictAsteroids.Add(a2.Id, a2);
			}
			partedAsteroids.Clear();

			if (dictAsteroids.Count <= player.SpawnAsteroidsFactorFunction(dictAsteroids.Count))
			{
				if (rnd.NextDouble() < Constants.Asteroid.spawnChance)
				{
					Edge randomEdge = (Edge)edgeArray.GetValue(rnd.Next(edgeArray.Length));
					Asteroid newAsteroid = SpawnAsteroid(randomEdge);
					dictAsteroids.Add(newAsteroid.Id, newAsteroid);
				}
			}

			if (dictUfos.Count <= player.SpawnUfosFactorFunction(dictUfos.Count))
			{
				if (rnd.NextDouble() < Constants.Ufo.spawnChance)
				{
					Edge randomEdge = (Edge)edgeArray.GetValue(rnd.Next(edgeArray.Length));
					Ufo u = SpawnUfo(randomEdge);
					dictUfos.Add(u.Id, u);
				}
			}
		}
		/// <summary>
		/// Генерация астероида с рандомной скоростью и размером
		/// </summary>
		private Ufo SpawnUfo(Edge edge)
		{
			Vector2f p = new Vector2f();
			Vector2f v = new Vector2f();
			int asteroidRadius;
			v.X = rnd.Next(Constants.Ufo.minSpeed, Constants.Ufo.maxSpeed);
			v.Y = rnd.Next(Constants.Ufo.minSpeed, Constants.Ufo.maxSpeed);
			if (rnd.Next(10) > 5) v.X = -v.X;
			if (rnd.Next(10) < 5) v.Y = -v.Y;
			switch (edge)
			{
				case Edge.LEFT:
					p.X = 0 - Constants.Ufo.baseSize;
					p.Y = rnd.Next(0, (int)window.Size.Y);
					break;
				case Edge.RIGHT:
					p.X = window.Size.X + Constants.Ufo.baseSize;
					p.Y = rnd.Next(0, (int)window.Size.Y);
					break;
				case Edge.UP:
					p.X = rnd.Next(0, (int)window.Size.X);
					p.Y = 0 - Constants.Ufo.baseSize;
					break;
				case Edge.DOWN:
					p.X = rnd.Next(0, (int)window.Size.X);
					p.Y = window.Size.Y + Constants.Ufo.baseSize;
					break;
			}
			return new Ufo(p, v);
		}
		/// <summary>
		/// Генерация астероида с рандомной скоростью и размером
		/// </summary>
		private Asteroid SpawnAsteroid(Edge edge)
		{
			Vector2f p = new Vector2f(); //положение астероида
			Vector2f v = new Vector2f(); //компонента скорости
			int asteroidRadius;
			v.X = rnd.Next(-Constants.Asteroid.maxSpeed, Constants.Asteroid.maxSpeed);
			v.Y = rnd.Next(-Constants.Asteroid.maxSpeed, Constants.Asteroid.maxSpeed);
			switch (edge)
			{
				case Edge.LEFT:
					p.X = 0 - Constants.Asteroid.maxSize;
					p.Y = rnd.Next(0, (int)window.Size.Y);
					break;
				case Edge.RIGHT:
					p.X = window.Size.X + Constants.Asteroid.maxSize;
					p.Y = rnd.Next(0, (int)window.Size.Y);
					break;
				case Edge.UP:
					p.X = rnd.Next(0, (int)window.Size.X);
					p.Y = 0 - Constants.Asteroid.maxSize;
					break;
				case Edge.DOWN:
					p.X = rnd.Next(0, (int)window.Size.X);
					p.Y = window.Size.Y + Constants.Asteroid.maxSize;
					break;
			}
			asteroidRadius = rnd.Next(Constants.Asteroid.minSize, Constants.Asteroid.maxSize);
			var texture = rnd.Next(1, 5);
			return new Asteroid(p, v, asteroidRadius, texture);
		}
		/// <summary>
		/// Астероид в фиксированной точке и заданного размера
		/// </summary>
		private Asteroid SpawnAsteroid(Vector2f pos, int maxRadius)
		{
			Vector2f v = new Vector2f
			{
				X = rnd.Next(-Constants.Asteroid.maxSpeed, Constants.Asteroid.maxSpeed),
				Y = rnd.Next(-Constants.Asteroid.maxSpeed, Constants.Asteroid.maxSpeed)
			};
			return new Asteroid(pos, v, rnd.Next(Constants.Asteroid.minSize, maxRadius), rnd.Next(1, 5));
		}

		private void UpdateScore()
		{
			scoreText.UpdateText("Score: " + player.score.ToString() + Environment.NewLine + 
			"Time: " + string.Format("{0}:{1}:{2}",
			gameTimer.Elapsed.Hours.ToString("D2"), gameTimer.Elapsed.Minutes.ToString("D2"), gameTimer.Elapsed.Seconds.ToString("D2")));

		}

		public override void DrawPause() 
		{
			Text menuText = new Text("PAUSE", windowFont, Constants.Fonts.ultraBigFontSize);
			menuText.Position = new Vector2f(window.Size.X / 2 - Constants.Fonts.ultraBigFontSize, window.Size.Y / 2 - Constants.Fonts.ultraBigFontSize);
			menuText.Style = Text.Styles.Bold;
			window.Draw(menuText);
		}

		public override void DrawGameOver()
		{
			Text menuText = new Text("GAME OVER", windowFont, Constants.Fonts.ultraBigFontSize);
			menuText.Position = new Vector2f(window.Size.X / 2 - Constants.Fonts.ultraBigFontSize * 3, window.Size.Y / 2 - Constants.Fonts.ultraBigFontSize * 2);
			menuText.DisplayedString += Environment.NewLine;
			menuText.DisplayedString += "Your score:" + lastGameScore;
			menuText.Style = Text.Styles.Bold;
			window.Draw(menuText);
		}
	}
}