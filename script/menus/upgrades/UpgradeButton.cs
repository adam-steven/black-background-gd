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
	[Export] public float bulletSize = 0; //Modifies the size of the bullet sprite

	[Export] public bool endUpgrading = false;

	private bool objectSelected = false;

	public PlayerController player;

	public override void _Ready() {
		this.Connect("mouse_entered", this, "MouseEntered");
		this.Connect("mouse_exited", this, "MouseExited");
	}

	public override void _Process(float delta) {
		if (Input.IsActionJustPressed("ui_select") && objectSelected){

			if(IsInstanceValid(player)) {
				player.UpdateStats(health, movementForce, shotDelay, noOfBullets, bulletForce, bulletStrength, bulletAccuracy, bulletBurstAmount, bulletTimeAlive, bulletSize);
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

	[Signal] public delegate void on_pressed(MenuButtons button);
	private void _OnButtonPress() {
		this.EmitSignal("on_pressed", this);
	}
}
