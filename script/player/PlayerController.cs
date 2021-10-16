using Godot;
using static BulletOwnerList;

//Player movement and firing
//Player acts as a bullet, movement done via impulse forces
public partial class PlayerController : RigidBody2D
{
	GunController gun; 
	public override void _Ready()
	{
		gun = new GunController(shotDelay, this, BulletOwner.PlayerController); 
	}

	public override void _PhysicsProcess(float delta)
	{
		WASDMovement();
        MouseRotation();

		if (Input.IsActionPressed("ui_fire1"))
			gun.Shoot();

		if (Input.IsActionJustPressed("ui_fire2"))
			StopPlayer();
	}

	private void ShootCooledDown() {
		gun.ShootCooledDown();
	}
}


