using Godot;
using System;

public class StatUpgrade : Area2D
{    
	[Export] public int health = 100;
	[Export] public float movementForce = 1000f;
	[Export] public float shotDelay = 1;
	[Export] public int noOfBullets = 1; //Number of bullets fired at once (Shotgun effect)
	[Export] public float bulletForce = 3000; //Bullet's speed
	[Export] public int bulletStrength = 10; //Amount of damage the bullet does
	[Export] public float bulletAccuracy = 0.2f; //Bullet's accuracy (0 is perfect accuracy)
	[Export] public int bulletBurstAmount = 0; //Number of bullets fired in quick succession (fixed delay interval)
	[Export] public float bulletTimeAlive = 0.25f; //Bullet Range (>0 = 0.05f)

	//private EntityStats stats;
	private bool objectSelected = false;
	Main gameControlScript;

	public override void _Ready() {
		this.Connect("mouse_entered", this, "MouseEntered");
		this.Connect("mouse_exited", this, "MouseExited");

		Godot.Node2D gameController = GetNode<SceneController>(Globals.scenePath).GetCurrentScene();
		gameControlScript = (Main)gameController;
		gameController.Connect("upgrading_finished", this, "DeleteSelf");
	}

	public override void _Process(float delta) {
		if (Input.IsActionJustPressed("ui_select") && objectSelected){
			Godot.Node2D gameController = GetNode<SceneController>(Globals.scenePath).GetCurrentScene();
			Entities player = gameController.GetNodeOrNull<Entities>("Player");

			if(IsInstanceValid(player)) {
				player.health += health;
				player.movementForce += movementForce;
				player.shotDelay += shotDelay; 
				player.noOfBullets += noOfBullets;
				player.bulletForce += bulletForce; 
				player.bulletStrength += bulletStrength; 
				player.bulletAccuracy += bulletAccuracy; 
				player.bulletBurstAmount += bulletBurstAmount; 
				player.bulletTimeAlive += bulletTimeAlive;

				//Update the background colour just incase its no longer in the red
				gameControlScript.UpdateBackgroundColour(player.health);
			}

			gameControlScript.FinishedUpgrading();
			DeleteSelf();
		}	
	}

	private void MouseEntered() {
		AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("UpgradeSelected");
		objectSelected = true;
	}

	private void MouseExited() {
		AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("UpgradeDeselected");
		objectSelected = false;
	}

	private void DeleteSelf() {
		this.QueueFree();
	}
}
