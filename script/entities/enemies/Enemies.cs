using System;

namespace Godot
{
    public class Enemies : Entities
    {
        [Export] private float rotationSpeed = 3f;
        [Export] private int pointsOnKill = 100;

	    public RigidBody2D player;

        public Enemies() {}

        internal void FacePlayer() {
            this.LookAt(player.GlobalPosition); 
        }

        internal void TurnToPlayer(float delta) {
            Vector2 direction = (player.GlobalPosition - this.GlobalPosition);
            Single angleTo = this.Transform.x.AngleTo(direction);
            this.Rotate(Math.Sign(angleTo) * Math.Min(delta * rotationSpeed, Math.Abs(angleTo)));
        }

        internal void MoveInDirection(Vector2 _thrustDirection) {
            Vector2 _thrust = _thrustDirection * movementForce;
            SetAxisVelocity(_thrust.Rotated(Rotation));
        }

        internal void PushInDirection(Vector2 _thrustDirection) {
            Vector2 _thrust = _thrustDirection * movementForce;
            ApplyCentralImpulse(_thrust);
        }

        internal bool IsActive() {
            return (IsInstanceValid(player) && health > 0);
        }

        #region Death handling

            //Called by the bullet script to take damage / die
            public override void TakeDamage(BulletController strikingBullet) {
                if(health <= 0) return;
                
                UpdateHealth(-strikingBullet.strength);

                AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
                anim.Play("EnemyHit");

                if(health <= 0) {
                    SetPhysicsProcess(false);

                    //Give score
                    this.EmitSignal("_update_score", pointsOnKill);

                    //Start death sequence 
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

            public override void UpdateHealth(int addend) {
                health = Math.Max(0, health + addend);
            }

        #endregion

        #region Delayed start

            private Godot.Position2D loadingSpinner;
            private Godot.CollisionShape2D collider;
            private Godot.Sprite sprite;

            private void Start(string animName = "") {
                loadingSpinner.QueueFree();
                collider.Disabled = false;
                sprite.Visible = true;
                SetPhysicsProcess(true);
            }

            public void InitDelayedStart(string nodeName) {
                SetPhysicsProcess(false);

                loadingSpinner = this.GetNode<Godot.Position2D>(nodeName);
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