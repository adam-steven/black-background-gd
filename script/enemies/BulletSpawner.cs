using Godot;
using static Enums;

public class BulletSpawner : RigidBody2D
{
	[Export] float spawnSpeedModifier = 1f; //(1 * spawnSpeedModifier) = speed of the animation;
	private RigidBody2D player;
	private GunController gun;
	
	public override void _Ready()
	{
		Godot.Node2D gameController = this.GetParent<Godot.Node2D>();
		player = gameController.GetNodeOrNull<RigidBody2D>("Player");

		Node2D thisStats = this.GetNodeOrNull<Node2D>("Stats");
		EntityStats stats = (EntityStats)thisStats;

		gun = new GunController(this, BulletOwner.EnemyController, stats); 

		AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.PlaybackSpeed = spawnSpeedModifier;
		anim.Connect("animation_finished", this, "ShootBullet");
	}

	public override void _Process(float delta)
	{
		if(!IsInstanceValid(player)) return;
		FacePlayer();
	}

	private void FacePlayer() {
		this.LookAt(player.GlobalPosition); 
	}

	private void ShootBullet(string animName) { 
		gun.Shoot();
		this.QueueFree(); 
	}
}
