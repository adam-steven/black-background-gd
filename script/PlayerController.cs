using Godot;
using System;

//Player movement and firing
//Player acts as a bullet, movement done via impulse forces
public class PlayerController : RigidBody2D
{
	//movement vars
  	[Export] private float movementForce = 1000f;
	private Godot.Node2D gameController;
	private Godot.Sprite playerSprite;

	//movement vars
	private PackedScene bulletScene;
	private Timer shotTimer;
	private bool canShoot = true;
	[Export] private float shotDelay = 0.2f;
	
	public override void _Ready()
	{
		gameController = this.GetParent<Godot.Node2D>();
		playerSprite = this.GetNode<Godot.Sprite>("Sprite");
		bulletScene = (PackedScene)GD.Load("res://scenes/Bullet.tscn");

		shotTimer = GetNode<Timer>("Timer");
		shotTimer.WaitTime = shotDelay;
		shotTimer.Connect("timeout", this, "ShootCoolDown");
	}

	// -- Movement --

	public override void _PhysicsProcess(float delta)
	{
		WASDMovement();
		MouseRotation();

		if (Input.IsActionPressed("ui_fire1"))
			Shoot();
	}

	// Handels detection of a right click to apply a complete character force  stop
	public override void _IntegrateForces(Physics2DDirectBodyState state)
	{
		if (Input.IsActionJustPressed("ui_fire2"))
			state.LinearVelocity = Vector2.Zero;
	}

	// Handels detection of WASD inputs to apply character force 
	private void WASDMovement()
	{
		Vector2 _thrust = Vector2.Zero;

		if (Input.IsActionJustPressed("ui_left"))
			_thrust += (Vector2.Left * movementForce);
		if (Input.IsActionJustPressed("ui_right"))
			_thrust += (Vector2.Right * movementForce);
		if (Input.IsActionJustPressed("ui_up"))
			_thrust += (Vector2.Up * movementForce);
		if (Input.IsActionJustPressed("ui_down"))
			_thrust += (Vector2.Down * movementForce);


		if(_thrust != Vector2.Zero)
			ApplyCentralImpulse(_thrust);
	}

	private void MouseRotation() { playerSprite.LookAt(GetGlobalMousePosition()); }

	//When player collides with *any rigibody bounce
	private void _On_Player_Body_Entered(object body)
	{
		//Collided rigidbody stats  
		Godot.Sprite hitBodySprint = ((Node)body).GetNode<Godot.Sprite>("Sprite");
		Vector2 hitCenter = hitBodySprint.GlobalPosition;
		Vector2 hitScaleHalf = (hitBodySprint.GlobalScale/2f) * 20f; //Sprite scale is x20 smaller than global position (might change)    

		//Player stats
		Vector2 playerCenter = this.GlobalPosition;

		//Apply opposite directional forces
		Vector2 _thrust = new Vector2 (
			GetCollisionForceDirection(playerCenter.x, hitCenter.x, hitScaleHalf.x),
			GetCollisionForceDirection(playerCenter.y, hitCenter.y, hitScaleHalf.y)
		) * movementForce;
		ApplyCentralImpulse(_thrust);
	}

	//Verifies player center Axis is at the edge of a collided object same axis
	//If so it returns 1 or -1 for a force to push the player away
	//If no it returns 0
	private int GetCollisionForceDirection(float playerAxisPos, float objectAxisPos, float objectAxisScale)
	{
		float objectLowerPoint = objectAxisPos - objectAxisScale;
		float objectUpperPoint = objectAxisPos + objectAxisScale;

		if (playerAxisPos < objectLowerPoint || objectUpperPoint < playerAxisPos)
		{
			int direction = (int)(playerAxisPos - objectAxisPos);
			return (direction / Math.Abs(direction));
		}

		return 0;
	}

	//-- Shooting --

	private void Shoot()
	{
		if(!canShoot) return;
		
		Area2D bullet = (Area2D)bulletScene.Instance();
		BulletController bulletCon = (BulletController)bullet;

		// Access bullet properties
		bullet.Position = this.Position;
		bullet.Rotation = playerSprite.Rotation;

		// Access bullet script 
		bulletCon.bOwner = BulletController.BulletOwnerList.PlayerController;
		bulletCon.openMotion = this.LinearVelocity/2f;

		// Shoot bullet + start cooldown 
		gameController.AddChild(bullet);
		canShoot = false;
		shotTimer.Start();
	}

	private void ShootCoolDown() { canShoot = true; }
}


