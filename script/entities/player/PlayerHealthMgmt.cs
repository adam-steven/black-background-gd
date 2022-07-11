//Stat and stat management 
using Godot;

public partial class PlayerController
{
	[Export] private int pointsOnBlock = 50;
	
	private bool invincible = false;

	[Signal] public delegate void _end_game();

	//Called by the bullet script to take damage / die
	public override void TakeDamage(BulletController strikingBullet) {
		//Indicate that no damage was taken (+ health gained) by playing all movement effects
		if(invincible) {
			GainHealth(strikingBullet);
			return;
		}

		if(health <= 0) return;

		//Decrease health
		health -= strikingBullet.strength;
		GD.Print("Player: " + health);

		//Damage indication
		AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("PlayerHit");
		this.EmitSignal("_shake_screen", 12, 0.2f);
		this.EmitSignal("_break_score_update");

		//Update background colour based on health
		ColourController.UpdateBackgroundColour(health);

		//Kill player if health is 0
		if(health <= 0) {
			anim.Play("PlayerDeath");

			//Go to gameover screen
			this.EmitSignal("_end_game");
		}	
	}

	private void GainHealth(BulletController strikingBullet) {
		string[] effectPos = {"Right", "Left", "Bottom", "Top"};
		for (int i = 0; i < effectPos.Length; i++)
			PlayEffect(effectPos[i]);

		health += strikingBullet.strength/2;
		this.EmitSignal("_update_score", pointsOnBlock);

		//Flash text
		if(strikingBullet.special) { 
			this.EmitSignal("_section_text", "NICE", true);
			this.EmitSignal("_destroy_all_bullets");  
		}

		//Flash colour + freeze frame
		var darkenedColour = ColourController.enemyColour.LinearInterpolate(Color.ColorN("black"), 0.5f);
		ColourController.FlashBackgroundColour(darkenedColour, GetTree(), health);
	}
}
