//Stat and stat management 
using Godot;
using System;
using static Enums;

public partial class PlayerController
{
    [Export] private int pointsOnBlock = 50;

    [Export] private bool invincible = false;

    //Called by the bullet script to take damage / die
    public override void _TakeDamage(Projectile strikingBullet)
    {
        //Indicate that no damage was taken + health gained 
        if (invincible)
        {
            switch (strikingBullet.type)
            {
                case BulletVariations.Normal:
                    GainHealth(strikingBullet, Colour.LevelColour);
                    return;

                case BulletVariations.NormalStrong:
                    GainHealth(strikingBullet, Colour.LevelColour, "NICE");
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

    private void DeductHealth(Projectile strikingBullet)
    {
        if (health <= 0) return;

        //Decrease health
        _UpdateHealth(-strikingBullet.strength);

        //Damage indication
        AnimationPlayer anim = this.GetNode<AnimationPlayer>("AnimationPlayer");
        anim.Play("PlayerHit");
        this.EmitSignal("_shake_screen", 12, 0.2f);
        this.EmitSignal("_break_score_update");

        //Kill player if health is 0
        if (health <= 0)
        {
            SetPhysicsProcess(false);

            anim.Play("PlayerDeath");

            //Go to game-over screen
            this.EmitSignal("_on_death");
        }
    }

    private void GainHealth(Projectile strikingBullet, Color backgroundColour, string flashText = null)
    {
        _UpdateHealth((int)Math.Round(strikingBullet.strength / 1.5f));
        this.EmitSignal("_update_score", pointsOnBlock);

        //Show effect
        BlockEffect(backgroundColour, flashText);
    }

    private void BlockEffect(Color backgroundColour, string flashText = null)
    {
        //Flash text
        if (flashText != null) { this.EmitSignal("_section_text", flashText, true); }

        //Flash colour + freeze frame
        Colour.FlashBackgroundColourAsync(backgroundColour, GetTree(), health);
    }

    public override void _UpdateHealth(int addend)
    {
        health = Mathc.Limit(0, health + addend, 1000);
        this.EmitSignal("_update_health_ui", health, (addend > 0));
        Colour.UpdateBackgroundColour(health);
    }
}
