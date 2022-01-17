//Stat and stat management 
using Godot;

public partial class PlayerController
{
	private bool invincible = false;

    //Called by the bullet script to take damage / die
	public void TakeDamage(int damage) {
		//Indicate that no damage was taken (+ health gained) by playing all movement effects
		if(invincible) {
			string[] effectPos = {"Right", "Left", "Bottom", "Top"};
			for (int i = 0; i < effectPos.Length; i++)
				PlayEffect(effectPos[i]);

			stats.health += damage/2;
			UpdateBackgroundColour();
			return;
		}

		if(stats.health <= 0) return;

		//Decrease health
		stats.health -= damage;
		GD.Print("Player: " + stats.health);

		//Damage indication
		AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("PlayerHit");
		cameraControl.StartShakeScreen(12, 0.2f);

		UpdateBackgroundColour();

		//Kill player if health is 0
		if(stats.health <= 0) {
			anim.Connect("animation_finished", this, "DestorySelf");
			anim.Play("PlayerDeath");
		}	
	}

	private void UpdateBackgroundColour(){
		//Update background colour based on health
		Godot.Node2D gameController = this.GetParent<Godot.Node2D>();
		GameController controllerScript = (GameController)gameController;
		controllerScript.UpdateBackgroundColour(stats.health);
	}

	private void DestorySelf(string animName) { this.QueueFree(); }
}