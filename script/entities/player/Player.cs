using Godot;
using static Enums;

//Player movement and firing
//Player acts as a bullet, movement done via impulse forces
public partial class Player : Entity
{
	[Signal] public delegate void UpdateHealthUiEventHandler(int health, bool healthIncrease); //Event: the player has gained/lost health
    [Signal] public delegate void PlayerLeftCameraEventHandler(); //Event: the player has left the cameras view (VisibleOnScreenNotifier2D bubble up)

    private Godot.Sprite2D sprite;

	public override void _Ready()
	{
		this.entityType = BulletOwner.PlayerController;

		sprite = this.GetNode<Godot.Sprite2D>("Sprite2D");
		gun = new GunController(this, sprite, GetTree());

		this.Connect(RigidBody2D.SignalName.BodyEntered, new Callable(this, "OnBodyEntered"));

		VisibleOnScreenNotifier2D vis = this.GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
		vis.Connect(VisibleOnScreenNotifier2D.SignalName.ScreenExited, new Callable(this, "ScreenExited"));

		//ReSet the health UI, background colour, and block indicator
		_UpdateHealth(0);
	}

	public override void _PhysicsProcess(double delta)
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

	private void ScreenExited()
	{
		this.EmitSignal(SignalName.PlayerLeftCamera);
	}
}


