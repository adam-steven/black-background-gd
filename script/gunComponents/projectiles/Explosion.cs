using Godot;
using System;
using static Enums;

public class Explosion : Projectile
{
	[Export] private float pushForce = 2000;
	private float maxLength;

    internal override void _ProjectileReady() 
    {
		Position2D edgeNode = this.GetNode<Position2D>("ExplosionEdge");
		Vector2 explosionEdge = edgeNode.GlobalPosition;
		maxLength = this.GlobalPosition.DistanceTo(explosionEdge);

        AnimationPlayer anim = this.GetNode<AnimationPlayer>("AnimationPlayer");
        anim.Connect("animation_finished", this, "_DestroySelf");
    }

    internal override void _RenderColour() 
    {
		this.type = BulletVariations.Spectral;
		this.Modulate = colour.LinearInterpolate(Color.ColorN("white"), 0.65f) + new Godot.Color(0f, 0f, 0f, 0.15f);
		this.Scale *= 1.5f;
    }

	internal override void _BodyEntered(object body)
	{
		//Make sure i hit an entity
		Type bodyType = body.GetType();
		if (bodyType.IsSubclassOf(typeof(Godot.Entity)))
		{
			Entity hitEntity = (Entity)body;

			Vector2 direction = this.GlobalPosition.DirectionTo(hitEntity.GlobalPosition);
			float distance = this.GlobalPosition.DistanceTo(hitEntity.GlobalPosition);
			float percentToCenter = distance / maxLength;

			int maxDamage = this.strength * 2;
			int damage = maxDamage - (int)Math.Round(this.strength * percentToCenter);
			this.strength = damage;

			float impact = pushForce - ((pushForce/2) * percentToCenter);
			hitEntity.ApplyCentralImpulse(direction * impact);

			hitEntity._TakeDamage(this);
		}
	}
}
