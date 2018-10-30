using System;

namespace Avalon
{
	public class Player
	{
		public string name;
		public long score;
		protected double difficulty;	 //Параметр отвечает за сложность игры - количество одновременно находящихся астероидов
		public Player() 
		{
			score = 0;
			difficulty = Constants.Player.difficulty;
		}

		public Player(string name)
		{
			score = 0;
			this.name = name;
			difficulty = Constants.Player.difficulty;
		}

		public int SpawnAsteroidsFactorFunction(int astroidsCount)
		{
			return Convert.ToInt32(Math.Round(difficulty * Math.Log(0.3*astroidsCount+1.0),0));
		}

		public int SpawnUfosFactorFunction(int astroidsCount)
		{
			return Convert.ToInt32(Math.Round(difficulty*0.5 * Math.Log(0.3 * astroidsCount + 1.0), 0));
		}

	}
}