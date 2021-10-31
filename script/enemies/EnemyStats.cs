using Godot;

public partial class EnemyController
{
    [Export] private int health = 100;
	[Export] private float movementForce = 1000f;
    [Export] private float shotDelay = 0.2f;

    //Called by the bullet script to take damage / die
	public void TakeDamage(int damage)
	{
		health -= damage;
		GD.Print(health);
	}
}
