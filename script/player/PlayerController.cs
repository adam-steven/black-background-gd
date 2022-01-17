using Godot;
using static Enums;

//Player movement and firing
//Player acts as a bullet, movement done via impulse forces
public partial class PlayerController : RigidBody2D
{
	private GunController gun; 
	private EntityStats stats;
	private CameraController cameraControl;
	
	public override void _Ready() {
		Node2D thisStats = this.GetNodeOrNull<Node2D>("Stats");
		stats = (EntityStats)thisStats;

		gun = new GunController(this, BulletOwner.PlayerController, stats);
		this.Connect("body_entered", this, "_OnPlayerBodyEntered");

		Godot.Node2D gameController = this.GetParent<Godot.Node2D>();
		Camera2D camera = gameController.GetNode<Camera2D>("Camera2D");
		cameraControl = (CameraController)camera;

		connectAnimEndSignal("Stop", "StopIFramesEnd"); 
	}

	public override void _PhysicsProcess(float delta) {
		if(stats.health <= 0) return;

		WASDMovement();
		MouseRotation();
		gun.UpdateBurst();

		if (Input.IsActionPressed("ui_fire1"))
			gun.Shoot();

		if (Input.IsActionJustPressed("ui_fire2"))
			StopPlayer();
	}
}


