using System;
using Godot;
using static Enums;

public class BulletSpawner : Enemies
{
	Random rnd = new Random();
	
	public override void _Ready()
	{
		float spawnSpeedModifier = shotDelay / rnd.Next(1, 3);

		gun = new GunController(this); 

		AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.PlaybackSpeed = spawnSpeedModifier;
		anim.Connect("animation_finished", this, "ShootBullet");
	}

	public override void _Process(float delta)
	{
		if(!IsInstanceValid(player)) return;
		FacePlayer();
	}

	private void ShootBullet(string animName) { 
		gun.Shoot(BulletVariations.Normal);
		EmitDeathSignal(); 
	}
}
