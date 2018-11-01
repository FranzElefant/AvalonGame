using System;
using System.Diagnostics;
using System.Timers;
using Avalon.Sounds;
using Avalon.Textures;
using SFML.Graphics;
using SFML.Window;

namespace Avalon
{
	public abstract class Game
    {
		//Границы экрана
		public enum Edge { UP, DOWN, LEFT, RIGHT, NULL }
		protected Stopwatch gameTimer;
		protected RenderWindow window;
		protected Color clearColor;
		protected Sprite backGround;
		protected Font windowFont;
		protected bool useTextures = true;
		protected bool isPaused = false;
		protected bool isGameOver = false;
		protected long lastGameScore;
		protected float dt;

		public Game(uint width, uint height, String title, Color clrColor)
		{
			gameTimer = new Stopwatch();
			dt = Constants.Rendering.deltaTime;
			if (Constants.Rendering.fullScreen) 
			{
				window = new RenderWindow(new VideoMode(width, height), title, Styles.Fullscreen, 
				new ContextSettings() { AntialiasingLevel = Constants.Rendering.antialiasingLevel });
			}
			else 
			{
				window = new RenderWindow(new VideoMode(width, height), title, Styles.Default, 
				new ContextSettings() { AntialiasingLevel = Constants.Rendering.antialiasingLevel });
			}

			window.SetFramerateLimit(Constants.Rendering.frameRate);
			clearColor = clrColor;

			//Обработчики событий
			window.Closed += Window_Closed;
			window.KeyPressed += Window_KeyPressed;

			// Шрифты
			windowFont = new Font(@"Fonts\BebasNeueRegular.ttf");
		}

		private void Window_KeyPressed(object sender, KeyEventArgs e)
		{
			if ((e.Code == Keyboard.Key.Escape))
			{
				if (!isPaused && !isGameOver)
				{
					isPaused = true;
				}
				else if (isPaused)
				{
					isPaused = false;
					gameTimer.Start();
				}
			}

			if ((e.Code == Keyboard.Key.Return))
			{
				if (isGameOver)
				{
					isGameOver = false;
					gameTimer.Start();
				}
			}

			if ((e.Code == Keyboard.Key.Tab))
			{
				if (!useTextures)
				{
					useTextures = true;
					TextureEngine.Init();
				}
				else if (useTextures)
				{
					useTextures = false;
				}
			}
		}

		protected void Window_Closed(object sender, EventArgs e)
		{
			window.Close();
		}

		public void Run()
		{
			Init();
			SoundEngine.Init();
			TextureEngine.LoadImages();
			TextureEngine.Init();
			// Главный цикл программы
			while (window.IsOpen)
			{
				// Вызов обработчиков событий
				window.DispatchEvents();
				window.Clear(clearColor);
				if (isPaused)
				{
					gameTimer.Stop();
					DrawPause();
				}
				else if (isGameOver)
				{
					gameTimer.Stop();
					DrawGameOver();
				}
				else if (!isPaused && !isGameOver)
				{
					Update(window, dt);
				}
				window.Display();
			}
		}

		public abstract void Init();
		public abstract void DrawPause();
		public abstract void DrawGameOver();
		public abstract void CleanUp();
		public abstract void Restart();

		/// <summary>
		/// Обработчик игровой логики
		/// </summary>
		/// <param name="window"></param>
		/// <param name="dt"></param>
		public abstract void Update(RenderWindow window, float deltaTime);

		public Stopwatch GameTimer()
		{
			return gameTimer;
		}

		public Window Window()
		{
			return window;
		}
	}
}