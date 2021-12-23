using Godot;
using static Enums;

//Player movement and firing
//Player acts as a bullet, movement done via impulse forces
public partial class PlayerController : RigidBody2D
{
	private GunController gun; 
	
	public override void _Ready()
	{
		gun = new GunController(shotDelay, this, BulletOwner.PlayerController, noOfBullets, bulletStrength, bulletForce, bulletAccuracy, bulletBurstAmount, bulletTimeAlive);
		this.Connect("body_entered", this, "_OnPlayerBodyEntered"); 
	}

	public override void _PhysicsProcess(float delta)
	{
		if(health <= 0) return;

		WASDMovement();
		MouseRotation();
		gun.UpdateBurst();

		if (Input.IsActionPressed("ui_fire1"))
			gun.Shoot();

		if (Input.IsActionJustPressed("ui_fire2"))
			StopPlayer();
	}
}


