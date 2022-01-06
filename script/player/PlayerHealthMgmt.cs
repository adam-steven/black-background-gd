//Stat and stat management 
using Godot;

public partial class PlayerController
{
	private bool invincible = false;

    //Called by the bullet script to take damage / die
	public void TakeDamage(int damage) {
		//Indicate that no damage was taken (+ health gained) by playing all movement effects
		if(invincible) {
			PlayEffect("Right");
			PlayEffect("Left");
			PlayEffect("Bottom");
			PlayEffect("Top");
			stats.health += damage/2;
			return;
		}

		if(stats.health <= 0) return;

		//Decrease health
		stats.health -= damage;
		GD.Print("Player: " + stats.health);

		AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("PlayerHit");

		//Update background colour based on health
		Godot.Node2D gameController = this.GetParent<Godot.Node2D>();
		GameController controllerScript = (GameController)gameController;
		controllerScript.UpdateBackgroundColor(stats.health);

		//Kill player if health is 0
		if(stats.health <= 0) {
			anim.Connect("animation_finished", this, "DestorySelf");
			anim.Play("PlayerDeath");
		}	
	}

	private void DestorySelf(string animName) { this.QueueFree(); }
}