using System;
using System.Collections.Generic;
using Godot;

public class MultiBulletSpawner : Obstacle
{
	Random rnd = new Random();

	[Export] bool facePlayer = true;

	[Export] List<string> spawnPoints = new List<string>();
	List<GunController> guns = new List<GunController>();
	
	internal override void _EntityReady()
	{
		SetProcess(facePlayer);

		for (int i = 0; i < spawnPoints.Count; i++)
		{
			Node2D spawnPoint = this.GetNode<Node2D>(spawnPoints[i]);
			GunController gun = new GunController(this, spawnPoint, GetTree());
			guns.Add(gun);
		}

		float spawnSpeedModifier = shotDelay / rnd.Next(1, 3);
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
		for (int i = 0; i < guns.Count; i++) { guns[i].Shoot(); }
		EmitDeathSignal();
	}
}
