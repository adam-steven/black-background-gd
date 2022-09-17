using Godot;
using System;
using System.Threading.Tasks;
using static Enums;

public class TurretLv1 : Enemies
{
	public override void _EnemyReady() 
	{
		InitDelayedStart("LoadingSpinner");
		
		anim.Connect("animation_finished", this, "Attack");
		StartAttackTimer();
	}

	public override void _PhysicsProcess(float delta) 
	{
		TurnToPlayer(delta);
	}

	private async void StartAttackTimer() {
		int shotDelayMs = (int)(this.shotDelay * 1000) + 500; //+500 to account for anim time
		await Task.Delay(shotDelayMs);

		while(IsActive()) {
			anim.Play("EnemyAttack");
			await Task.Delay(shotDelayMs);
		}
	}

	private void Attack(string animName = "") {
		if(animName == "EnemyAttack") {
			gun.Shoot(BulletVariations.Normal);
		}
	}
}
