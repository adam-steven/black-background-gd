using Godot;
using System;
using static Enums;

//Player movement and firing
//Player acts as a bullet, movement done via impulse forces
public partial class PlayerController : Entity
{
	[Signal] internal delegate void _update_health_ui(int health, bool healthIncrease);
	[Signal] internal delegate void _player_left_camera();

	private Godot.Sprite sprite;

	public override void _Ready()
	{
		this.entityType = BulletOwner.PlayerController;

		sprite = this.GetNode<Godot.Sprite>("Sprite");
		gun = new GunController(this, sprite, GetTree());

		this.Connect("body_entered", this, "_OnPlayerBodyEntered");

		VisibilityNotifier2D vis = this.GetNode<VisibilityNotifier2D>("VisibilityNotifier2D");
		vis.Connect("screen_exited", this, "_OnScreenExited");

		//ReSet the health UI, background colour, and block indicator
		_UpdateHealth(0);
		UpdateBlockIndicator();
	}

	public override void _PhysicsProcess(float delta)
	{
		MouseRotation();

		//shoot need to be here for action hold
		if (Input.IsActionPressed("Shoot")) { gun.Shoot(BulletVariations.Player); }
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent.IsActionPressed("Up")) { PushPlayer(Vector2.Up, "Bottom"); }
		if (inputEvent.IsActionPressed("Down")) { PushPlayer(Vector2.Down, "Top"); }
		if (inputEvent.IsActionPressed("Left")) { PushPlayer(Vector2.Left, "Right"); }
		if (inputEvent.IsActionPressed("Right")) { PushPlayer(Vector2.Right, "Left"); }
		if (inputEvent.IsActionPressed("Block")) { StopPlayer(); }
	}

	internal override bool _IsActive() 
	{ 
		return IsInstanceValid(sprite); 
	}

	private void _OnScreenExited()
	{
		this.EmitSignal("_player_left_camera");
	}
}


