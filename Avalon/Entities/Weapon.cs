namespace Avalon.Entities
{
	public abstract class Weapon : BehavioralEntity
	{
		protected float baseDamage;
		protected float baseSpeed;
		protected float maxSpeed;
		abstract public float Hit();
	}
}
