using Godot;
using System;
using System.Threading.Tasks;
using static Enums;

public class Chaser : Enemies
{
	private AnimationPlayer shootAnim;

	public override void _Ready() 
	{
		FacePlayer();
		InitDelayedStart("LoadingSpinner");

		Godot.Sprite thisSprite = this.GetNode<Godot.Sprite>("Sprite");
		thisSprite.SelfModulate = colour;

		shootAnim = this.GetNode<AnimationPlayer>("AnimationPlayer");
		shootAnim.Connect("animation_finished", this, "Attack");
		gun = new GunController(this); 

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

		while(health > 0) {
			shootAnim.Play("EnemyAttack");
			await Task.Delay(shotDelayMs);
		}
	}

	private void Attack(string animName = "") {
		if(animName == "EnemyAttack") {
			gun.Shoot(BulletVariations.Normal);
		}
	}
}
