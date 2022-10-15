using Godot;
using static Enums;

public class WeakPoint : Entity
{
    [Signal] internal delegate void _hit();

    public override void _Ready()
    {
        this.entityType = BulletOwner.EnemyController;

        //Center flash particle 
        Node2D parent = this.GetParent<Node2D>();
        Particles2D particles = this.GetNode<Particles2D>("Particles2D");
        particles.GlobalPosition = parent.GlobalPosition;
    }

    public override void _TakeDamage(Bullet strikingBullet)
    {
        this.EmitSignal("_hit", strikingBullet);
    }
}
