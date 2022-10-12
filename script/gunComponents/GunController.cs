using System;
using Godot;
using static Enums;
using System.Threading.Tasks;

public class GunController
{
    Random rnd = new Random();

    private PackedScene bulletScene;
    private Entities ownerNode;
    private Node2D spawnPoint;

    private SceneTree tree;

    private int timeLastShot = 0;

    ///<summary> 
    ///		Allows nodes to shoot bullets form there body.
    ///</summary>
    public GunController(Entities ownerNode, Node2D spawnPoint, SceneTree tree)
    {
        this.bulletScene = (PackedScene)GD.Load("res://scenes/misc/Bullet.tscn");
        this.ownerNode = ownerNode;
        this.spawnPoint = spawnPoint;
        this.tree = tree;
    }

    public void Shoot(BulletVariations bulletType)
    {
        if (CanShoot())
        {
            BurstBulletsAsync(bulletType);
        }
    }

    //Checks if the threshold amount of time has passed since the last shot
    //isBursting = true lowers the nextShotThreshold
    private bool CanShoot()
    {
        int shotDelayMs = (int)(ownerNode.shotDelay * 1000);
        int nextShotThreshold = timeLastShot + shotDelayMs;

        int currentTime = (int)OS.GetTicksMsec();
        bool canShoot = (currentTime >= nextShotThreshold);
        if (canShoot) timeLastShot = currentTime;

        return canShoot;
    }

    //Loop spawn bullets with delay for burst effect
    private async void BurstBulletsAsync(BulletVariations bulletType)
    {
        //Make sure the burst is always faster than normal shots
        float betweenBurstDelay = (ownerNode.shotDelay < 0.2f) ? ownerNode.shotDelay / 2f : 0.1f;
        betweenBurstDelay *= 1000; //convert to ms

        for (int i = 0; i < ownerNode.bulletBurstAmount; i++)
        {
            if(!Godot.Object.IsInstanceValid(ownerNode) || tree.Paused) { break; }
            SpawnBullets(bulletType);
            await Task.Delay((int)betweenBurstDelay);
        }
    }

    //Loop spawn bullets for shotgun effect
    private void SpawnBullets(BulletVariations bulletType)
    {
        Godot.Node2D gameController = ownerNode.GetParent<Godot.Node2D>();

        //Set can shoot timer here so thats the burst will never be slower than shoot rate
        timeLastShot = (int)OS.GetTicksMsec();

        //Loop for shotgun effect
        for (int i = 0; i < ownerNode.noOfBullets; i++)
        {
            SpawnBullet(gameController, bulletType);
        }
    }

    //Spawn 1 bullet
    private void SpawnBullet(Godot.Node2D gameController, BulletVariations bulletType)
    {
        Bullet bullet = (Bullet)bulletScene.Instance();
        float randomAccuracyDeviation = (float)((rnd.NextDouble() * ownerNode.bulletAccuracy) - (rnd.NextDouble() * ownerNode.bulletAccuracy));

        // Access bullet properties
        bullet.Position = ownerNode.Position;
        bullet.Rotation = spawnPoint.GlobalRotation + randomAccuracyDeviation;
        bullet.Scale = new Vector2(ownerNode.bulletSize, ownerNode.bulletSize);
        bullet.Modulate = ownerNode.bulletColour;

        // Access bullet script 
        bullet.bOwner = ownerNode.entityType;
        // bulletCon.openMotion = ownerNode.LinearVelocity/2f;
        bullet.strength = ownerNode.bulletStrength;
        bullet.movementForce = ownerNode.bulletForce;
        bullet.timeAlive = ownerNode.bulletTimeAlive;
        bullet.type = bulletType;

        // Shoot bullet + start cooldown 
        gameController.AddChild(bullet);
    }
}
