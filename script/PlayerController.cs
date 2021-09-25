using Godot;
using System.Linq;

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

	public override void _IntegrateForces(Physics2DDirectBodyState state)
	{
		//Complete Force Stop
		if (Input.IsActionJustPressed("ui_fire2"))
			state.LinearVelocity = new Vector2(0, 0);
	}

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

	// when player collides with *any rigibody bounce
	private void _On_Player_Body_Entered(object body)
	{
		GD.Print("player pos: " + this.GlobalPosition);
		GD.Print("col pos: " + ((Node)body).Get("global_position"));

		if (Enumerable.Range(1,100).Contains((int)this.GlobalPosition.x)){}
		if (Enumerable.Range(1,100).Contains((int)this.GlobalPosition.y)) {}
		

		Test((Node)body);
		//Vector2 _thrust = (Vector2.Left * movementForce);
		//ApplyCentralImpulse(_thrust);
	}
	
	private void Test(Node hitPos) {
		Vector2 hitPosCenter = (Vector2)hitPos.Get("global_position");
		Vector2 hitPosScaleHalf = hitPos.GetNode<Godot.Sprite>("Sprite").GlobalScale/2f;

		// Top, Bottom, Left, Right
		float[] edgePoints = 
			new float[4] {
				3f,
				3f,
				3f,
				3f
			};

		PackedScene testingDot = (PackedScene)GD.Load("res://scenes/TestingDot.tscn");
		Godot.Sprite tDot = (Godot.Sprite)testingDot.Instance();

		tDot.GlobalPosition = hitPosCenter;
		gameController.AddChild(tDot);

		// tDot.GlobalPosition = this.Position;
		// gameController.AddChild(tDot);
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


