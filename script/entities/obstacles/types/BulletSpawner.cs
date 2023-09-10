using System;
using Godot;

public class BulletSpawner : Obstacle
{
	Random rnd = new Random();
	[Export] bool facePlayer = true;

	internal override void _EntityReady()
	{
		SetProcess(facePlayer);

		float spawnSpeedModifier = ShotDelay / rnd.Next(1, 3);
		anim.PlaybackSpeed = spawnSpeedModifier;
		anim.Connect("animation_finished", this, "ShootBullet");
	}

	public override void _Process(float delta)
	{
		if (!IsInstanceValid(player)) return;
		TurnToPlayer(delta);
	}

	private void ShootBullet(string animName)
	{
		gun.Shoot();
		EmitDeathSignal();
	}
}
