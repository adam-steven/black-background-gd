using Godot;
using System;
using System.Threading.Tasks;
using static Enums;

public class Chaser : Enemies
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
		MoveInDirection(Vector2.Right);
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
