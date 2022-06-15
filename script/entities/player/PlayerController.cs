using Godot;
using static Enums;

//Player movement and firing
//Player acts as a bullet, movement done via impulse forces
public partial class PlayerController : Entities
{
	private GunController gun; 

	private CameraController cameraControl;
	
	public override void _Ready() {
		gun = new GunController(this, BulletOwner.PlayerController);
		this.Connect("body_entered", this, "_OnPlayerBodyEntered");

		Camera2D camera = GetNode<SceneController>(Globals.scenePath).GetMainCamera();
		cameraControl = (CameraController)camera;

		connectAnimEndSignal("Stop", "StopIFramesEnd"); 
	}

	public override void _PhysicsProcess(float delta) {
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


