//Stat and stat management 
using Godot;

public partial class PlayerController
{
	private bool invincible = false;

	[Signal] public delegate void _end_game();
	[Signal] public delegate void _shake_screen(int shakeForce, float shakeDuration);

	//Called by the bullet script to take damage / die
	public override void TakeDamage(int damage) {
		//Indicate that no damage was taken (+ health gained) by playing all movement effects
		if(invincible) {
			string[] effectPos = {"Right", "Left", "Bottom", "Top"};
			for (int i = 0; i < effectPos.Length; i++)
				PlayEffect(effectPos[i]);

			health += damage/2;
			//Update background colour based on health
			ColourControl.UpdateBackgroundColour(health);
			return;
		}

		if(health <= 0) return;

		//Decrease health
		health -= damage;
		GD.Print("Player: " + health);

		//Damage indication
		AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("PlayerHit");
		this.EmitSignal("_shake_screen", 12, 0.2f);

		//Update background colour based on health
		ColourControl.UpdateBackgroundColour(health);

		//Kill player if health is 0
		if(health <= 0) {
			anim.Play("PlayerDeath");

			//Go to gameover screen
			this.EmitSignal("_end_game");
		}	
	}
}
