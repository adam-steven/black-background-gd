using System;
using Newtonsoft.Json; 

namespace Godot
{
    public class Enemies : Entities
    {
        public GunController gun; 
	    public RigidBody2D player;

        public Enemies() {}

        public void FacePlayer() {
            this.LookAt(player.GlobalPosition); 
        }

        public void MoveInDirection(Vector2 _thrustDirection) {
            Vector2 _thrust = _thrustDirection * movementForce;
            SetAxisVelocity(_thrust.Rotated(Rotation));
        }

        public void PushInDirection(Vector2 _thrustDirection) {
            Vector2 _thrust = _thrustDirection * movementForce;
            ApplyCentralImpulse(_thrust);
        }

        #region Death handling

            //Called by the bullet script to take damage / die
            public override void TakeDamage(int damage) {
                if(health <= 0) return;
                
                health -= damage;
                GD.Print("Enemy: " + health);

                AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
                anim.Play("EnemyHit");

                if(health <= 0) {
                    anim.Connect("animation_finished", this, "EmitDeathSignal");
                    anim.Play("EnemyDeath");
                }	
            }

            //Emit signal on death
            [Signal] public delegate void _on_death();
            public void EmitDeathSignal(string animName = "") {
                this.EmitSignal("_on_death");
                this.QueueFree();
            }

        #endregion

        #region Delayed start

            public bool start = false;
            private Godot.Position2D loadingSpinner;
            private Godot.CollisionShape2D collider;
            private Godot.Sprite sprite;

            private void Start(string animName = "") {
                loadingSpinner.QueueFree();
                collider.Disabled = false;
                sprite.Visible = true;
                start = true;
            }

            public void InitDelayedStart() {
                loadingSpinner = this.GetNode<Godot.Position2D>("LoadingSpinner");
                AnimationPlayer loadingAnim  = loadingSpinner.GetNode<AnimationPlayer>("AnimationPlayer");
                loadingAnim.Connect("animation_finished", this, "Start");

                collider = this.GetNode<Godot.CollisionShape2D>("CollisionShape2D");
                collider.Disabled = true;

                sprite = this.GetNode<Godot.Sprite>("Sprite");
                sprite.Visible = false;
            }
            
        #endregion
    }
}