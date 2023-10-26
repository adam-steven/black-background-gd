using Godot;
using static Enums;

public partial class WeakPoint : Entity
{
    [Signal] public delegate void HitEventHandler(); //Event: on bullet touching

    public override void _Ready()
    {
        this.entityType = BulletOwner.EnemyController;

        //Center flash particle 
        Node2D parent = this.GetParent<Node2D>();
        GpuParticles2D particles = this.GetNode<GpuParticles2D>("GPUParticles2D");
        particles.GlobalPosition = parent.GlobalPosition;
    }

    public override void _TakeDamage(Projectile strikingBullet)
    {
        this.EmitSignal(SignalName.Hit, strikingBullet);
    }
}
