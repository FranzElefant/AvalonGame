namespace Avalon.Entities
{
	public class Target
	{
		Entity e;
		bool active;
		sbyte direction;

		Target() { }

		Target(Entity e, bool active, sbyte direction)
		{
			this.e = e;
			this.active = active;
			this.direction = direction;
		}

		Target(Entity e, sbyte direction)
		{
			this.e = e;
			this.active = true;
			this.direction = direction;
		}

		Target(Entity e)
		{
			this.e = e;
			this.active = true;
			this.direction = 1;
		}
	}
}