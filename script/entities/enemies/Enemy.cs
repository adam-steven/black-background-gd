using System;
using System.Threading.Tasks;
using static Enums;

namespace Godot
{
    public partial class Enemy : Obstacle
    {
        [Export] private int pointsOnKill = 100;
        [Export] private int healthOnCrit = 20; //Also handles points

        public override void _Ready()
        {
            this.entityType = BulletOwner.EnemyController;

            //Set the sprites colour to level colour
            Sprite2D sprite = this.GetNode<Sprite2D>("Sprite2D");
            sprite.SelfModulate = colour;

            //Configure weakpoint if one exists 
            WeakPoint weakPoint = this.GetNodeOrNull<WeakPoint>("WeakPoint");
            if (IsInstanceValid(weakPoint))
            {
                Sprite2D weakPointSprite = weakPoint.GetNode<Sprite2D>("Sprite2D");
                weakPointSprite.SelfModulate = bulletColour;
                weakPoint.Connect(WeakPoint.SignalName.Hit, new Callable(this, "WeakPointHit"));
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
                this.EmitSignal(Entity.SignalName.UpdateScore, pointsOnKill);

                //Start death sequence 
                anim.Connect(AnimationPlayer.SignalName.AnimationFinished, new Callable(this, "EmitDeathSignal"));
                anim.Play("EnemyDeath");
            }
        }

        private void WeakPointHit(Projectile strikingBullet)
        {
            if (!_IsActive()) { return; }
            _UpdateHealth(-strikingBullet.strength * 2);

            CritHitFlashAsync();
            this.EmitSignal(Entity.SignalName.UpdatePlayerHeath, healthOnCrit);
            this.EmitSignal(Entity.SignalName.UpdateScore, healthOnCrit);

            //Drop health on kill on repeated strike
            healthOnCrit = (int)Math.Round(healthOnCrit / 1.25f);
        }

        private async void CritHitFlashAsync()
        {
            Sprite2D sprite = this.GetNodeOrNull<Sprite2D>("Sprite2D");
            Sprite2D weakPointSprite = this.GetNodeOrNull<Sprite2D>("WeakPoint/Sprite2D");
            GpuParticles2D weakPointParticles = this.GetNodeOrNull<GpuParticles2D>("WeakPoint/GPUParticles2D");

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

            Marker2D loadingSpinner = this.GetNode<Marker2D>("LoadingSpinner");
            AnimationPlayer loadingAnim = loadingSpinner.GetNode<AnimationPlayer>("AnimationPlayer");
            loadingAnim.Connect(AnimationPlayer.SignalName.AnimationFinished, new Callable(this, "Start"));
        }

        private void ToggleLoadingVisuals(bool loading)
        {
            Marker2D loadingSpinner = this.GetNode<Marker2D>("LoadingSpinner");
            CollisionShape2D collider = this.GetNode<CollisionShape2D>("CollisionShape2D");
            Sprite2D sprite = this.GetNode<Sprite2D>("Sprite2D");
            Node2D weakPoint = this.GetNodeOrNull<Node2D>("WeakPoint");

            if (loading) { loadingSpinner.Visible = loading; }
            else { loadingSpinner.QueueFree(); }

            collider.Disabled = loading;
            sprite.Visible = !loading;
            if (IsInstanceValid(weakPoint))
            {
                CollisionShape2D weakPointCollider = weakPoint.GetNode<CollisionShape2D>("CollisionShape2D");
                weakPointCollider.Disabled = loading;
                weakPoint.Visible = !loading;
            }
        }

        #endregion
    }
}