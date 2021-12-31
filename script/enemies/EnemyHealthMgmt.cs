//Stat and stat management
using Godot;

public partial class EnemyController
{
    //Called by the bullet script to take damage / die
	public void TakeDamage(int damage) {
		if(stats.health <= 0) return;
		
		stats.health -= damage;
		GD.Print("Enemy: " + stats.health);

		if(stats.health <= 0) {
			AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
			anim.Connect("animation_finished", this, "DestorySelf");
			anim.Play("EnemyDeath");
		}	
	}

	private void DestorySelf(string animName = "") {
		Godot.Node2D gameController = this.GetParent<Godot.Node2D>();
		GameController controllerScript = (GameController)gameController;
		controllerScript.CheckIfEnemies();

		this.QueueFree();
	}
}
