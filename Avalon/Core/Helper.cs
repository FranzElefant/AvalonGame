using SFML.Graphics;

namespace Avalon.Core
{
	public class ScreenText
	{
		public Text text;
		public ScreenText(string text, Font font, uint size, Color color)
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
}
