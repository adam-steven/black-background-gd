using Godot;
using System;

public class StatUpgrade : Area2D
{    
	private EntityStats stats;
	private bool objectSelected = false;
	GameController gameControlScript;

	public override void _Ready() {
		Node2D thisStats = this.GetNodeOrNull<Node2D>("Stats");
		stats = (EntityStats)thisStats;

		this.Connect("mouse_entered", this, "MouseEntered");
		this.Connect("mouse_exited", this, "MouseExited");

		Godot.Node2D gameController = GetNode<SceneController>(Globals.scenePath).GetCurrentScene();
		gameControlScript = (GameController)gameController;
		gameController.Connect("upgrading_finished", this, "DeleteSelf");
	}

	public override void _Process(float delta) {
		if (Input.IsActionJustPressed("ui_select") && objectSelected){
			Godot.Node2D gameController = GetNode<SceneController>(Globals.scenePath).GetCurrentScene();
			RigidBody2D player = gameController.GetNodeOrNull<RigidBody2D>("Player");
			Node2D playerStats = player.GetNodeOrNull<Node2D>("Stats");

			if(IsInstanceValid(playerStats)) {
				EntityStats pStats = (EntityStats)playerStats;

				pStats.health += stats.health;
				pStats.movementForce += stats.movementForce;
				
				pStats.shotDelay += stats.shotDelay; 
				pStats.noOfBullets += stats.noOfBullets;
				pStats.bulletForce += stats.bulletForce; 
				pStats.bulletStrength += stats.bulletStrength; 
				pStats.bulletAccuracy += stats.bulletAccuracy; 
				pStats.bulletBurstAmount += stats.bulletBurstAmount; 
				pStats.bulletTimeAlive += stats.bulletTimeAlive;

				//Update the background colour just incase its no longer in the red
				gameControlScript.UpdateBackgroundColour(pStats.health);
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
