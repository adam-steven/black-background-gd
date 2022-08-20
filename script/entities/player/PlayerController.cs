using Godot;
using static Enums;

//Player movement and firing
//Player acts as a bullet, movement done via impulse forces
public partial class PlayerController : Entities
{
	public override void _Ready() {
		gun = new GunController(this);
		this.Connect("body_entered", this, "_OnPlayerBodyEntered");

		VisibilityNotifier2D vis = this.GetNode<VisibilityNotifier2D>("VisibilityNotifier2D");
		vis.Connect("screen_exited", this, "_OnScreenExited");

		connectAnimEndSignal("Stop", "StopIFramesEnd"); 
	}

	public override void _PhysicsProcess(float delta) {
		WASDMovement();
		MouseRotation();
		
		if (Input.IsActionPressed("ui_fire1"))
			gun.Shoot(BulletVariations.Player);
	}

	private void _OnScreenExited() {
		this.EmitSignal("_player_left_camera");  
	}
}


