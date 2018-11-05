namespace Avalon.Entities
{
	public class Target
	{
		public Entity entity;
		public bool active;
		public bool inversion;

		public Target() { }

		public Target(Entity e, bool active, bool invesion)
		{
			this.entity = e;
			this.active = active;
			this.inversion = invesion;
		}

		public Target(Entity e, bool inversion)
		{
			this.entity = e;
			this.active = true;
			this.inversion = inversion;
		}

		public Target(Entity e)
		{
			this.entity = e;
			this.active = true;
			this.inversion = false;
		}
	}
}