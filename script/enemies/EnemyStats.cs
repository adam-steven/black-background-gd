using Godot;

public partial class EnemyController
{
    [Export] private int health = 100;
	[Export] private float movementForce = 1000f;

    [Export] private float shotDelay = 0.2f;
	[Export] private float noOfBullet = 1; //Number of bullets fired at once (Shotgun effect)
	[Export] private float bulletForce = 3000; //Bullet's speed
	[Export] private int bulletStrength = 10; //Amount of damage the bullet does
	[Export] private float bulletTimeAlive = 0; //Bullet Range (0 infinite)

    //Called by the bullet script to take damage / die
	public void TakeDamage(int damage)
	{
		health -= damage;
		GD.Print("Enemy: " + health);

		if(health <= 0)
			this.QueueFree(); 
	}
}
