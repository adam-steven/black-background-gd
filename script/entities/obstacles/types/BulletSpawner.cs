using System;
using Godot;
using static Enums;

public class BulletSpawner : Obstacles
{
	Random rnd = new Random();
	
	public override void _EntityReady()
	{
		float spawnSpeedModifier = shotDelay / rnd.Next(1, 3);
		anim.PlaybackSpeed = spawnSpeedModifier;
		anim.Connect("animation_finished", this, "ShootBullet");
	}

	public override void _Process(float delta)
	{
		if(!IsInstanceValid(player)) return;
		TurnToPlayer(delta);
	}

	private void ShootBullet(string animName) { 
		gun.Shoot(BulletVariations.Normal);
		EmitDeathSignal(); 
	}
}
