using Godot;
using System;
using static Enums;

public class Explosion : Projectile
{
	private float maxLength;

    internal override void _ProjectileReady() 
    {
		this.Scale *= 1.5f;

		Position2D edgeNode = this.GetNode<Position2D>("ExplosionEdge");
		Vector2 explosionEdge = edgeNode.GlobalPosition;
		maxLength = this.GlobalPosition.DistanceTo(explosionEdge);

        AnimationPlayer anim = this.GetNode<AnimationPlayer>("AnimationPlayer");
        anim.Connect("animation_finished", this, "_DestroySelf");
    }

    internal override void _RenderColour() 
    {
        Particles2D particle = this.GetNode<Particles2D>("Particles2D");
        ParticlesMaterial particleMaterial = (ParticlesMaterial)particle.ProcessMaterial;
        particleMaterial.Color = colour;
    }

	internal override void _BodyEntered(object body)
	{
		//Make sure i hit an entity
		Type bodyType = body.GetType();
		if (bodyType.IsSubclassOf(typeof(Godot.Entity)))
		{
			Entity hitEntity = (Entity)body;

			//If collision is made by owner return
			if (hitEntity.entityType == bOwner) { return; }

			float distance = this.GlobalPosition.DistanceTo(hitEntity.GlobalPosition);
			float percentToCenter = distance / maxLength;

			int maxDamage = this.strength * 2;
			int damage = maxDamage - (int)Math.Round(this.strength * percentToCenter);
			this.strength = damage;

			hitEntity._TakeDamage(this);
		}
	}
}
