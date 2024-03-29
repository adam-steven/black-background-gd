using System;
using System.Threading.Tasks;
using static Enums;

namespace Godot
{
    public class Enemy : Obstacle
    {
        [Export] private int pointsOnKill = 100;
        [Export] private int healthOnCrit = 20; //Also handles points

        public override void _Ready()
        {
            this.entityType = BulletOwner.EnemyController;

            //Set the sprites colour to level colour
            Godot.Sprite sprite = this.GetNode<Godot.Sprite>("Sprite");
            sprite.SelfModulate = colour;

            //Configure weakpoint if one exists 
            Godot.Node2D weakPoint = this.GetNodeOrNull<Godot.Node2D>("WeakPoint");
            if (IsInstanceValid(weakPoint))
            {
                Godot.Sprite weakPointSprite = weakPoint.GetNode<Godot.Sprite>("Sprite");
                weakPointSprite.SelfModulate = bulletColour;
                weakPoint.Connect("_hit", this, "WeakPointHit");
            }

            //Init fixed components
            anim = this.GetNode<AnimationPlayer>("AnimationPlayer");
            gun = new GunController(this, this, GetTree());

            FacePlayer();
            _EntityReady();
        }

        #region Controls

        internal void MoveInDirection(Vector2 _thrustDirection)
        {
            Vector2 _thrust = _thrustDirection * MovementForce;
            SetAxisVelocity(_thrust.Rotated(Rotation));
        }

        internal void PushInDirection(Vector2 _thrustDirection)
        {
            Vector2 _thrust = _thrustDirection * MovementForce;
            ApplyCentralImpulse(_thrust);
        }

        internal override bool _IsActive()
        {
            return IsInstanceValid(player) && Health > 0;
        }

        #endregion

        #region Death handling

        //Called by the bullet script to take damage / die
        public override void _TakeDamage(Projectile strikingBullet)
        {
            if (!_IsActive()) { return; }
            _UpdateHealth(-strikingBullet.strength);

            anim = this.GetNode<AnimationPlayer>("AnimationPlayer");
            anim.Play("EnemyHit");
        }

        public override void _UpdateHealth(int addend)
        {
            Health = Math.Max(0, Health + addend);

            if (Health <= 0)
            {
                SetPhysicsProcess(false);

                //Give score
                this.EmitSignal("_update_score", pointsOnKill);

                //Start death sequence 
                anim.Connect("animation_finished", this, "EmitDeathSignal");
                anim.Play("EnemyDeath");
            }
        }

        private void WeakPointHit(Projectile strikingBullet)
        {
            if (!_IsActive()) { return; }
            _UpdateHealth(-strikingBullet.strength * 2);

            CritHitFlashAsync();
            this.EmitSignal("_update_player_heath", healthOnCrit);
            this.EmitSignal("_update_score", healthOnCrit);

            //Drop health on kill on repeated strike
            healthOnCrit = (int)Math.Round(healthOnCrit / 1.25f);
        }

        private async void CritHitFlashAsync()
        {
            Godot.Sprite sprite = this.GetNodeOrNull<Godot.Sprite>("Sprite");
            Godot.Sprite weakPointSprite = this.GetNodeOrNull<Godot.Sprite>("WeakPoint/Sprite");
            Godot.Particles2D weakPointParticles = this.GetNodeOrNull<Godot.Particles2D>("WeakPoint/Particles2D");

            SetPhysicsProcess(false); //0.3 second stun
            sprite.SelfModulate = Color.Color8(251, 255, 255);
            weakPointSprite.SelfModulate = Color.Color8(251, 255, 255);
            weakPointParticles.Emitting = true;

            await Task.Delay(400);

            if (!IsInstanceValid(sprite)) return;
            SetPhysicsProcess(true);
            sprite.SelfModulate = colour;
            weakPointSprite.SelfModulate = bulletColour;
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
            AnimationPlayer loadingAnim = loadingSpinner.GetNode<AnimationPlayer>("AnimationPlayer");
            loadingAnim.Connect("animation_finished", this, "Start");
        }

        private void ToggleLoadingVisuals(bool loading)
        {
            Godot.Position2D loadingSpinner = this.GetNode<Godot.Position2D>("LoadingSpinner");
            Godot.CollisionShape2D collider = this.GetNode<Godot.CollisionShape2D>("CollisionShape2D");
            Godot.Sprite sprite = this.GetNode<Godot.Sprite>("Sprite");
            Godot.Node2D weakPoint = this.GetNodeOrNull<Godot.Node2D>("WeakPoint");

            if (loading) { loadingSpinner.Visible = loading; }
            else { loadingSpinner.QueueFree(); }

            collider.Disabled = loading;
            sprite.Visible = !loading;
            if (IsInstanceValid(weakPoint))
            {
                Godot.CollisionShape2D weakPointCollider = weakPoint.GetNode<Godot.CollisionShape2D>("CollisionShape2D");
                weakPointCollider.Disabled = loading;
                weakPoint.Visible = !loading;
            }
        }

        #endregion
    }
}