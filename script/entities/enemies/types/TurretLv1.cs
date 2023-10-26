using Godot;
using System.Threading.Tasks;

public partial class TurretLv1 : Enemy
{
	internal override void _EntityReady()
	{
		InitDelayedStart();

		anim.Connect(AnimationPlayer.SignalName.AnimationFinished, new Callable(this, "Attack"));
		StartAttackTimerAsync();
	}

	public override void _PhysicsProcess(double delta)
	{
		TurnToPlayer(delta);
	}

	private async void StartAttackTimerAsync()
	{
		int shotDelayMs = (int)(this.ShotDelay * 1000) + 500; //+500 to account for anim time
		await Task.Delay(shotDelayMs);

		while (_IsActive())
		{
			if(!GetTree().Paused) { anim.Play("EnemyAttack"); }
			await Task.Delay(shotDelayMs);
		}
	}

	private void Attack(string animName = "")
	{
		if (animName == "EnemyAttack")
		{
			gun.Shoot();
		}
	}
}
