using Godot;
using System;
using System.Collections.Generic;
using static Enums;

interface IStats
{
    int health { get; set; }
    float movementForce { get; set; }
    
    float shotDelay { get; set; }
    int noOfBullets { get; set; } //Number of bullets fired at once (Shotgun effect)
    float bulletForce { get; set; } //Bullet's speed
    int bulletStrength { get; set; } //Amount of damage the bullet does
    float bulletAccuracy { get; set; } //Bullet's accuracy (0 is perfect accuracy)
    int bulletBurstAmount { get; set; } //Number of bullets fired in quick succession (fixed delay interval)
    float bulletTimeAlive { get; set; } //Bullet Range (>0 = 0.05f)
    float bulletSize { get; set; } //Modifies the size of the bullet sprite
    List<string> onBulletDestroyScenes { get; set; } //Scenes to spawn in a random direction after the bullet is destroyed 
}
