using Godot;
using static Enums;

//Player movement and firing
//Player acts as a bullet, movement done via impulse forces
public partial class PlayerController : RigidBody2D
{
	private GunController gun; 
	private EntityStats stats;
	
	public override void _Ready() {
		Node2D thisStats = this.GetNodeOrNull<Node2D>("Stats");
        stats = (EntityStats)thisStats;

		gun = new GunController(this, BulletOwner.PlayerController, stats);
		this.Connect("body_entered", this, "_OnPlayerBodyEntered"); 
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


