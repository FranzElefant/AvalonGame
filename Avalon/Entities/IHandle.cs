using System.Diagnostics;

namespace Avalon.Entities
{
	public interface IHandle
	{
		void CheckButtonEvents(float dt, Stopwatch sw);
	}
}