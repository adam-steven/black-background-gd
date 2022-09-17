using Godot;
using System;

public class WeakPoint : Entities
{
    [Signal] internal delegate void _hit();

    public override void TakeDamage(BulletController strikingBullet) 
    {
        this.EmitSignal("_hit", strikingBullet);
    }
}
