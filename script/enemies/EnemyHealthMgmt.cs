//Stat and stat management
using Godot;

public partial class EnemyController
{
    //Called by the bullet script to take damage / die
	public void TakeDamage(int damage) {
		if(stats.health <= 0) return;
		
		stats.health -= damage;
		GD.Print("Enemy: " + stats.health);

		AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("EnemyHit");

		if(stats.health <= 0) {
			anim.Connect("animation_finished", this, "DestorySelf");
			anim.Play("EnemyDeath");
		}	
	}

	private void DestorySelf(string animName = "") {
		Godot.Node2D gameController = (Godot.Node2D)GetNode(Globals.gamePath);
		GameController controllerScript = (GameController)gameController;
		controllerScript.CheckIfEnemies();

		this.QueueFree();
	}
}
