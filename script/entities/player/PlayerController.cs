using Godot;
using static Enums;

//Player movement and firing
//Player acts as a bullet, movement done via impulse forces
public partial class PlayerController : Entities
{
	public override void _Ready() {
		gun = new GunController(this);
		this.Connect("body_entered", this, "_OnPlayerBodyEntered");

		connectAnimEndSignal("Stop", "StopIFramesEnd"); 
	}

	public override void _PhysicsProcess(float delta) {
		if(health <= 0) return;

		WASDMovement();
		MouseRotation();
		gun.UpdateBurst(BulletVariations.Player);

		if (Input.IsActionPressed("ui_fire1"))
			gun.Shoot(BulletVariations.Player);
	}
}


