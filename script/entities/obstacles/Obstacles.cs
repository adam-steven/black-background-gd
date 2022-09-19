using System;
using static Enums;

namespace Godot
{
    public class Obstacles : Entities
    {
        [Export] private float rotationSpeed = 3f;

        internal AnimationPlayer anim;

        public override void _Ready()
        {
            this.entityType = BulletOwner.EnemyController;

            //Init fixed components
            anim = this.GetNode<AnimationPlayer>("AnimationPlayer");
            gun = new GunController(this, this);

            FacePlayer();
            _EntityReady();
        }

        //EntityReady is for entity specific function as Ready is used for generics
        public virtual void _EntityReady() { }

        #region Controls

        internal void FacePlayer()
        {
            this.LookAt(player.GlobalPosition);
        }

        internal void TurnToPlayer(float delta)
        {
            Vector2 direction = (player.GlobalPosition - this.GlobalPosition);
            Single angleTo = this.Transform.x.AngleTo(direction);
            this.Rotate(Math.Sign(angleTo) * Math.Min(delta * rotationSpeed, Math.Abs(angleTo)));
        }

        internal virtual bool IsActive()
        {
            return (IsInstanceValid(player));
        }

        //Emit signal on death
        public void EmitDeathSignal(string animName = "")
        {
            this.EmitSignal("_on_death");
            this.QueueFree();
        }

        #endregion
    }
}