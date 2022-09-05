using Godot;
using System;

public class UpgradeButton : Position2D
{    
	[Export] private Texture graphic;
	[Export] private string name;
	[Export] private string description;

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

	private Godot.Button btn;

	public override void _Ready() {
		btn = this.GetNode<Godot.Button>("Button");
		btn.Connect("mouse_entered", this, "MouseEntered");
		btn.Connect("mouse_exited", this, "MouseExited");
		btn.Connect("pressed", this, "_OnButtonPress");

		Godot.Sprite sprite = btn.GetNode<Godot.Sprite>("Sprite");
		sprite.Texture = graphic;

		Godot.Label label = btn.GetNode<Godot.Label>("Label");
		label.Text = name;
	}

	private void MouseEntered() {
		AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("UpgradeSelected");

		Godot.Label label = btn.GetNode<Godot.Label>("Label");
		label.Visible = true;
	}

	private void MouseExited() {
		AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("UpgradeDeselected");

		Godot.Label label = btn.GetNode<Godot.Label>("Label");
		label.Visible = false;
	}

	[Signal] public delegate void on_pressed(MenuButtons button);
	private void _OnButtonPress() {
		if(IsInstanceValid(player)) {
			player.UpdateStats(health, movementForce, shotDelay, noOfBullets, bulletForce, bulletStrength, bulletAccuracy, bulletBurstAmount, bulletTimeAlive, bulletSize);
		}

		this.EmitSignal("on_pressed", this);
		this.QueueFree();
	}
}
