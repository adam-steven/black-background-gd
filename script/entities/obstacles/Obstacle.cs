using System;
using static Enums;

namespace Godot
{
    public partial class Obstacle : Entity
    {
        [Export] private float rotationSpeed = 3f;

        internal AnimationPlayer anim;

        public override void _Ready()
        {
            this.entityType = BulletOwner.EnemyController;

            //Init fixed components
            anim = this.GetNode<AnimationPlayer>("AnimationPlayer");
            gun = new GunController(this, this, GetTree());

            FacePlayer();
            _EntityReady();
        }

        //EntityReady is for entity specific function as Ready is used for generics
        internal virtual void _EntityReady() { }

        #region Controls

        internal void FacePlayer()
        {
            this.LookAt(player.GlobalPosition);
        }

        internal void TurnToPlayer(double delta)
        {
            Vector2 direction = (player.GlobalPosition - this.GlobalPosition);
            Single angleTo = this.Transform.X.AngleTo(direction);
            this.Rotate(Math.Sign(angleTo) * Math.Min((float)delta * rotationSpeed, Math.Abs(angleTo)));
        }

        internal override bool _IsActive()
        {
            return IsInstanceValid(player);
        }

        //Emit signal on death
        public void EmitDeathSignal(string animName = "")
        {
            this.Health = 0; //Saving enemies checks health, this prevents the dying enemy from being saved 

            this.EmitSignal(Entity.SignalName.OnDeath);
            this.QueueFree();
        }

        #endregion
    }
}