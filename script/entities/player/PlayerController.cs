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
		MouseRotation();

		//shoot need to be here for action hold
		if (Input.IsActionPressed("Shoot")) {
			gun.Shoot(BulletVariations.Player);
		}
	}

	public override void _Input(InputEvent inputEvent) {
		if (inputEvent.IsActionPressed("Up")) {
			PushPlayer(Vector2.Up, "Bottom");
		}

		if (inputEvent.IsActionPressed("Down")) {
			PushPlayer(Vector2.Down, "Top");
		}

		if (inputEvent.IsActionPressed("Left")) {
			PushPlayer(Vector2.Left, "Right");
		}

		if (inputEvent.IsActionPressed("Right")) {
			PushPlayer(Vector2.Right, "Left");
		}

		if (inputEvent.IsActionPressed("Block")) {
			StopPlayer();
		}
	}

	private void _OnScreenExited() {
		this.EmitSignal("_player_left_camera");  
	}
}


