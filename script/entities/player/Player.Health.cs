//Stat and stat management 
using Godot;
using System;
using static Enums;

public partial class Player
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
                    this.EmitSignal(Entity.SignalName.DestroyAllBullets);
                    return;

                case BulletVariations.Spectral:
                    strikingBullet.strength = 0;
                    GainHealth(strikingBullet, new Color(Colors.Black));
                    return;
            }
        }

        DeductHealth(strikingBullet);
    }

    private void DeductHealth(Projectile strikingBullet)
    {
        if (Health <= 0) return;

        //Decrease health
        _UpdateHealth(-strikingBullet.strength);

        //Damage indication
        AnimationPlayer anim = this.GetNode<AnimationPlayer>("AnimationPlayer");
        anim.Play("PlayerHit");
        this.EmitSignal(Entity.SignalName.ShakeScreen, 12, 0.2f);
        this.EmitSignal(Entity.SignalName.BreakScoreUpdate);

        //Kill player if health is 0
        if (Health <= 0)
        {
            SetPhysicsProcess(false);

            anim.Play("PlayerDeath");

            //Go to game-over screen
            this.EmitSignal(Entity.SignalName.OnDeath);
        }
    }

    private void GainHealth(Projectile strikingBullet, Color backgroundColour, string flashText = null)
    {
        _UpdateHealth((int)Math.Round(strikingBullet.strength / 1.5f));
        this.EmitSignal(Entity.SignalName.UpdateScore, pointsOnBlock);

        //Show effect
        BlockEffect(backgroundColour, flashText);
    }

    private void BlockEffect(Color backgroundColour, string flashText = null)
    {
        //Flash text
        if (flashText is not null) { this.EmitSignal(Entity.SignalName.SectionText, flashText, true); }

        //Flash colour + freeze frame
        Colour.FlashBackgroundColourAsync(backgroundColour, GetTree(), Health);
    }

    public override void _UpdateHealth(int addend)
    {
        Health = Mathc.Limit(0, Health + addend, 1000);
        this.EmitSignal(SignalName.UpdateHealthUi, Health, (addend > 0));
        Colour.UpdateBackgroundColour(Health);
    }
}
