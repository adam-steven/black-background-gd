using System;
using Godot;

public partial class BulletSpawner : Obstacle
{
	Random rnd = new Random();
	[Export] bool facePlayer = true;

	internal override void _EntityReady()
	{
		SetProcess(facePlayer);

		float spawnSpeedModifier = ShotDelay / rnd.Next(1, 3);
		anim.SpeedScale = spawnSpeedModifier;
		anim.Connect(AnimationPlayer.SignalName.AnimationFinished, new Callable(this, "ShootBullet"));
	}

	public override void _Process(double delta)
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
