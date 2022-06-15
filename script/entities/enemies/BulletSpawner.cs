using System;
using Godot;
using static Enums;

public class BulletSpawner : Entities
{
	Random rnd = new Random();

	private float spawnSpeedModifier;
	private RigidBody2D player;
	private GunController gun;
	
	public override void _Ready()
	{
		Godot.Node2D gameController = GetNode<SceneController>(Globals.scenePath).GetCurrentScene();
		player = gameController.GetNodeOrNull<RigidBody2D>("Player");

		spawnSpeedModifier = shotDelay / rnd.Next(1, 3);

		gun = new GunController(this, BulletOwner.EnemyController); 

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

		Godot.Node2D gameController = GetNode<SceneController>(Globals.scenePath).GetCurrentScene();
		Main controllerScript = (Main)gameController;
		controllerScript.CheckIfEnemies();

		this.QueueFree(); 
	}
}
