using System;
using static Enums;

namespace Godot
{
    public class Enemies : Obstacles
    {
        [Export] private float rotationSpeed = 3f;
        [Export] private int pointsOnKill = 100;

        public override void _Ready() 
        {
            this.entityType = BulletOwner.EnemyController;

            //Set the sprites colour to level colour
            Godot.Sprite sprite = this.GetNodeOrNull<Godot.Sprite>("Sprite");
            if(IsInstanceValid(sprite)) { sprite.SelfModulate = colour; }

            //Configure weakpoint if one exists 
            Godot.Node2D weakPoint = this.GetNodeOrNull<Godot.Node2D>("WeakPoint");
            if(IsInstanceValid(weakPoint)) { 
                Godot.Sprite weakPointSprite = weakPoint.GetNodeOrNull<Godot.Sprite>("Sprite");
                sprite.Modulate = bulletColour;
                weakPoint.Connect("_hit", this, "WeakPointHit"); 
            }

            //Init fixed components
            anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
            gun = new GunController(this, this); 

            FacePlayer();
            _EntityReady();
        }

        #region Controls

            internal void MoveInDirection(Vector2 _thrustDirection) 
            {
                Vector2 _thrust = _thrustDirection * movementForce;
                SetAxisVelocity(_thrust.Rotated(Rotation));
            }

            internal void PushInDirection(Vector2 _thrustDirection) 
            {
                Vector2 _thrust = _thrustDirection * movementForce;
                ApplyCentralImpulse(_thrust);
            }

            internal override bool IsActive() {
                return (IsInstanceValid(player) && health > 0);
            }

        #endregion

        #region Death handling

            //Called by the bullet script to take damage / die
            public override void TakeDamage(BulletController strikingBullet) 
            {
                if(health <= 0) return;
                
                UpdateHealth(-strikingBullet.strength);

                anim = this.GetNode<AnimationPlayer>("AnimationPlayer");
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

            public override void UpdateHealth(int addend) 
            {
                health = Math.Max(0, health + addend);
            }

            private void WeakPointHit(BulletController strikingBullet)
            {
                GD.Print("Weak point hit");

                strikingBullet.strength *= 2;
                TakeDamage(strikingBullet);
                //Strike Effect
                //Zoom in Effect
            }

        #endregion

        #region Delayed start

            private void Start(string animName = "") 
            {
                ToggleLoadingVisuals(false);
                SetPhysicsProcess(true);
            }

            public void InitDelayedStart() 
            {
                SetPhysicsProcess(false);
                ToggleLoadingVisuals(true);

                Godot.Position2D loadingSpinner = this.GetNode<Godot.Position2D>("LoadingSpinner");
                AnimationPlayer loadingAnim  = loadingSpinner.GetNode<AnimationPlayer>("AnimationPlayer");
                loadingAnim.Connect("animation_finished", this, "Start");
            }

            private void ToggleLoadingVisuals(bool loading) 
            {
                Godot.Position2D loadingSpinner = this.GetNode<Godot.Position2D>("LoadingSpinner");
                Godot.CollisionShape2D collider = this.GetNode<Godot.CollisionShape2D>("CollisionShape2D");
                Godot.Sprite sprite = this.GetNode<Godot.Sprite>("Sprite");
                Godot.Node2D weakPoint = this.GetNodeOrNull<Godot.Node2D>("WeakPoint");

                if(loading) { loadingSpinner.Visible = loading; }
                else { loadingSpinner.QueueFree(); }

                collider.Disabled = loading;
                sprite.Visible = !loading;
                if(IsInstanceValid(weakPoint)) { weakPoint.Visible = !loading;  }
            }
            
        #endregion
    }
}