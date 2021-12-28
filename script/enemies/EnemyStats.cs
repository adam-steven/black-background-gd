using Godot;

public partial class EnemyController
{
    [Export] private int health = 100;
	[Export] private float movementForce = 1000f;

    [Export] private float shotDelay = 1;
	[Export] private int noOfBullets = 1; //Number of bullets fired at once (Shotgun effect)
	[Export] private float bulletForce = 3000; //Bullet's speed
	[Export] private int bulletStrength = 10; //Amount of damage the bullet does
	[Export] private float bulletAccuracy = 0.2f; //Bullet's accuracy (0 is perfect accuracy)
	[Export] private int bulletBurstAmount = 0; //Number of bullets fired in quick succession (fixed delay interval)
	[Export] private float bulletTimeAlive = 0; //Bullet Range (0 infinite)

    //Called by the bullet script to take damage / die
	public void TakeDamage(int damage) {
		if(health <= 0) return;
		
		health -= damage;
		GD.Print("Enemy: " + health);

		if(health <= 0) {
			AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
			anim.Connect("animation_finished", this, "DestorySelf");
			anim.Play("EnemyDeath");
		}	
	}

	private void DestorySelf(string animName = "") {
		Godot.Node2D gameController = this.GetParent<Godot.Node2D>();
		GameController controllerScript = (GameController)gameController;
		controllerScript.CheckIfEnemies();

		this.QueueFree();
	}
}
