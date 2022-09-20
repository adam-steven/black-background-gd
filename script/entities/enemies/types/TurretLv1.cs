using Godot;
using System;
using System.Threading.Tasks;
using static Enums;

public class TurretLv1 : Enemies
{
    public override void _EntityReady()
    {
        InitDelayedStart();

        anim.Connect("animation_finished", this, "Attack");
        StartAttackTimerAsync();
    }

    public override void _PhysicsProcess(float delta)
    {
        TurnToPlayer(delta);
    }

    private async void StartAttackTimerAsync()
    {
        int shotDelayMs = (int)(this.shotDelay * 1000) + 500; //+500 to account for anim time
        await Task.Delay(shotDelayMs);

        while (_IsActive())
        {
            anim.Play("EnemyAttack");
            await Task.Delay(shotDelayMs);
        }
    }

    private void Attack(string animName = "")
    {
        if (animName == "EnemyAttack")
        {
            gun.Shoot(BulletVariations.Normal);
        }
    }
}
