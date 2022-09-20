using Godot;
using static Enums;

public class WeakPoint : Entities
{
    [Signal] internal delegate void _hit();

    public override void _Ready()
    {
        this.entityType = BulletOwner.EnemyController;
    }

    public override void _TakeDamage(BulletController strikingBullet)
    {
        this.EmitSignal("_hit", strikingBullet);
    }
}
