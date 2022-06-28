using Godot;
using System;

public class UpgradeButton : Area2D
{    
	[Export] private int health = 0;
	[Export] private float movementForce = 0f;
	[Export] private float shotDelay = 0;
	[Export] private int noOfBullets = 0; //Number of bullets fired at once (Shotgun effect)
	[Export] private float bulletForce = 0; //Bullet's speed
	[Export] private int bulletStrength = 0; //Amount of damage the bullet does
	[Export] private float bulletAccuracy = 0; //Bullet's accuracy (0 is perfect accuracy)
	[Export] private int bulletBurstAmount = 0; //Number of bullets fired in quick succession (fixed delay interval)
	[Export] private float bulletTimeAlive = 0; //Bullet Range (>0 = 0.05f)

	[Export] public bool endUpgrading = true;

	private bool objectSelected = false;

	public Entities player;

	public override void _Ready() {
		this.Connect("mouse_entered", this, "MouseEntered");
		this.Connect("mouse_exited", this, "MouseExited");
	}

	public override void _Process(float delta) {
		if (Input.IsActionJustPressed("ui_select") && objectSelected){

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
				ColourControl.UpdateBackgroundColour(player.health);
			}

			_OnButtonPress();
			this.QueueFree();
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

	[Signal]
    public delegate void on_pressed(MenuButtons button);
	private void _OnButtonPress() {
		this.EmitSignal("on_pressed", this);
	}
}
