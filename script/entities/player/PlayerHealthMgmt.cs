//Stat and stat management 
using Godot;
using System;
using static Enums;

public partial class PlayerController
{
	[Export] private int pointsOnBlock = 50;
	
	private bool invincible = false;

	[Signal] public delegate void _end_game();

	//Called by the bullet script to take damage / die
	public override void TakeDamage(BulletController strikingBullet) {
		//Indicate that no damage was taken + health gained 
		if(invincible) {
			switch (strikingBullet.type)
			{
				case BulletVariations.Normal:
					GainHealth(strikingBullet, Colour.enemyColour);
					return;

				case BulletVariations.NormalStrong:
					GainHealth(strikingBullet, Colour.enemyColour, "NICE");
					this.EmitSignal("_destroy_all_bullets");  
					return;
					
				case BulletVariations.Spectral:
					strikingBullet.strength = 0;
					GainHealth(strikingBullet, Color.ColorN("black"));
					return;
			}
		}

		DeductHealth(strikingBullet);
	}

	private void DeductHealth(BulletController strikingBullet) {
		if(health <= 0) return;

		//Decrease health
		UpdateHealth(-strikingBullet.strength);

		//Damage indication
		AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("PlayerHit");
		this.EmitSignal("_shake_screen", 12, 0.2f);
		this.EmitSignal("_break_score_update");

		//Kill player if health is 0
		if(health <= 0) {
			SetPhysicsProcess(false);

			anim.Play("PlayerDeath");

			//Go to game-over screen
			this.EmitSignal("_end_game");
		}
	}

	private void GainHealth(BulletController strikingBullet, Color backgroundColour, string flashText = null) {
		UpdateHealth(strikingBullet.strength/2);
		this.EmitSignal("_update_score", pointsOnBlock);

		//Show effect
		blockEffect(backgroundColour, flashText); 
	}

	private void blockEffect(Color backgroundColour, string flashText = null) {
		//Flash text
		if(flashText != null) { this.EmitSignal("_section_text", flashText, true); } 

		//Flash colour + freeze frame
		var darkenedColour = backgroundColour.LinearInterpolate(Color.ColorN("black"), 0.5f);
		Colour.FlashBackgroundColour(darkenedColour, GetTree(), health);
	}

	public override void UpdateHealth(int addend) {
		health = Mathc.Limit(0, health + addend, 100);	
		Colour.UpdateBackgroundColour(health);
		GD.Print("Player: " + health);
	}
}
