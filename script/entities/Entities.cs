using System;
using static Enums;
using Newtonsoft.Json; 

namespace Godot
{
    public class Entities : RigidBody2D
    {
        [Export] public BulletOwner entityType;

        [Export] public int health = 100;
        [Export] public float movementForce = 1000f;

        public Color colour;
        
        #region Gun
                
            public GunController gun; 

            [Export] public float shotDelay = 1;
            [Export] public int noOfBullets = 1; //Number of bullets fired at once (Shotgun effect)
            [Export] public float bulletForce = 3000; //Bullet's speed
            [Export] public int bulletStrength = 10; //Amount of damage the bullet does
            [Export] public float bulletAccuracy = 0.2f; //Bullet's accuracy (0 is perfect accuracy)
            [Export] public int bulletBurstAmount = 1; //Number of bullets fired in quick succession (fixed delay interval)
            [Export] public float bulletTimeAlive = 0.25f; //Bullet Range (>0 = 0.05f)
            [Export] public float bulletSize = 1.5f; //Modifies the size of the bullet sprite

        #endregion
        
        #region Signals

            [Signal] internal delegate void _destroy_all_bullets();
            [Signal] internal delegate void _section_text(string text, bool inverted);
            [Signal] internal delegate void _shake_screen(int shakeForce, float shakeDuration);
            [Signal] internal delegate void _player_left_camera();
            
            [Signal] internal delegate void _update_score(int points);
            [Signal] internal delegate void _break_score_update();

        #endregion       

        #region Public Methods 


            public virtual void TakeDamage(BulletController strikingBullet) {}

            public virtual void UpdateHealth(int addend) {}

            public void UpdateStats(int addHealth, float addMovementForce, float addShotDelay, int addNoOfBullets, float addBulletForce, int addBulletStrength, float addBulletAccuracy, int addBulletBurstAmount, float addBulletTimeAlive, float addBulletSize) {
	            health = Mathc.Limit(0, health + addHealth, 100);
				movementForce = Mathc.Limit(100f, movementForce + addMovementForce, 5000f);
				shotDelay = Mathc.Limit(0.1f, shotDelay + addShotDelay, 10f); 
				noOfBullets = Mathc.Limit(1, noOfBullets + addNoOfBullets, 30); 
				bulletForce = Mathc.Limit(100f, bulletForce + addBulletForce, 5000f);  
				bulletStrength = Mathc.Limit(1, bulletStrength + addBulletStrength, 100); 
				bulletAccuracy = Mathc.Limit(0f, bulletAccuracy + addBulletAccuracy, 360f);  
				bulletBurstAmount = Mathc.Limit(1, bulletBurstAmount + addBulletBurstAmount, 15);  
				bulletTimeAlive = Mathc.Limit(0.05f, bulletTimeAlive + addBulletTimeAlive, 10f);  
				bulletSize = Mathc.Limit(0.5f, bulletSize + addBulletSize, 15f);  

                //Update background colour based on health for player
                if(entityType == BulletOwner.PlayerController) { Colour.UpdateBackgroundColour(health); }
            }

        #endregion
    }
}