//Stat and stat management
using Godot;

public partial class EnemyController
{
    //Called by the bullet script to take damage / die
	public override void TakeDamage(int damage) {
		if(health <= 0) return;
		
		health -= damage;
		GD.Print("Enemy: " + health);

		AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("EnemyHit");

		if(health <= 0) {
			anim.Connect("animation_finished", this, "DestroySelf");
			anim.Play("EnemyDeath");
		}	
	}

	private void DestroySelf(string animName = "") {
		Godot.Node2D gameController = GetNode<SceneController>(Globals.scenePath).GetCurrentScene();
		Main controllerScript = (Main)gameController;
		controllerScript.CheckIfEnemies();

		this.QueueFree();
	}
}
