using System;
using Godot;
public class ExplosionSpawner : Obstacle
{
	Random rnd = new Random();

	internal override void _EntityReady()
	{
		float spawnSpeedModifier = shotDelay / rnd.Next(1, 3);
		anim.PlaybackSpeed = spawnSpeedModifier;
		anim.Connect("animation_finished", this, "ShootBullet");
	}

	private void ShootBullet(string animName)
	{
		gun.Shoot();
		EmitDeathSignal();
	}
}
