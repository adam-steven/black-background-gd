using Godot;
using System;
using static Enums;

//Player movement and firing
//Player acts as a bullet, movement done via impulse forces
public partial class PlayerController : Entities
{
	[Signal] internal delegate void _update_health_ui(int health);
	[Signal] internal delegate void _player_left_camera();

	public override void _Ready() {
		gun = new GunController(this);
		this.Connect("body_entered", this, "_OnPlayerBodyEntered");

		VisibilityNotifier2D vis = this.GetNode<VisibilityNotifier2D>("VisibilityNotifier2D");
		vis.Connect("screen_exited", this, "_OnScreenExited");

		//Set the health UI and background colour
		UpdateHealth(0);
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

	public void UpdateStats(int addHealth, float addMovementForce, float addShotDelay, int addNoOfBullets, float addBulletForce, int addBulletStrength, float addBulletAccuracy, int addBulletBurstAmount, float addBulletTimeAlive, float addBulletSize) {
		health = Mathc.Limit(0, health + addHealth, 1000);
		movementForce = Mathc.Limit(100f, movementForce + addMovementForce, 5000f);
		shotDelay = Mathc.Limit(0.1f, shotDelay + addShotDelay, 10f); 
		noOfBullets = Mathc.Limit(1, noOfBullets + addNoOfBullets, 30); 
		bulletForce = Mathc.Limit(100f, bulletForce + addBulletForce, 5000f);  
		bulletStrength = Mathc.Limit(1, bulletStrength + addBulletStrength, 100); 
		bulletAccuracy = Mathc.Limit(0f, bulletAccuracy + addBulletAccuracy, 360f);  
		bulletBurstAmount = Mathc.Limit(1, bulletBurstAmount + addBulletBurstAmount, 15);  
		bulletTimeAlive = Mathc.Limit(0.05f, bulletTimeAlive + addBulletTimeAlive, 10f);  
		bulletSize = Mathc.Limit(0.5f, bulletSize + addBulletSize, 15f);  

		//Update background colour and health UI
		this.EmitSignal("_update_health_ui", health);
		Colour.UpdateBackgroundColour(health); 
	}
}


