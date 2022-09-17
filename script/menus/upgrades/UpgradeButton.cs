using Godot;
using System;

public class UpgradeButton : Position2D
{    
	[Export] private string description = "";

	[Export] private int health = 0;
	[Export] private float movementForce = 0f;
	[Export] private float shotDelay = 0;
	[Export] private int noOfBullets = 0; //Number of bullets fired at once (Shotgun effect)
	[Export] private float bulletForce = 0; //Bullet's speed
	[Export] private int bulletStrength = 0; //Amount of damage the bullet does
	[Export] private float bulletAccuracy = 0; //Bullet's accuracy (0 is perfect accuracy)
	[Export] private int bulletBurstAmount = 0; //Number of bullets fired in quick succession (fixed delay interval)
	[Export] private float bulletTimeAlive = 0; //Bullet Range (>0 = 0.05f)
	[Export] private float bulletSize = 0; //Modifies the size of the bullet sprite

	[Export] public bool endUpgrading = false;

	[Signal] public delegate void _on_pressed(MenuButtons button);
	[Signal] public delegate void _update_upgrade_ui(string value);

	public PlayerController player;
	public bool showNames;
	public bool showDesc;
	private Godot.Button btn;

	public override void _Ready() 
	{
		btn = this.GetNode<Godot.Button>("Button");
		btn.Connect("mouse_entered", this, "MouseEntered");
		btn.Connect("mouse_exited", this, "MouseExited");
		btn.Connect("pressed", this, "_OnButtonPress");

		Godot.Label label = btn.GetNode<Godot.Label>("Label");
		label.Visible = false;
	}

	private void MouseEntered() 
	{
		AnimationPlayer anim  = btn.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("UpgradeSelected");

		ShowDescriptionUi(true);
		ShowDescriptionUi(description);
	}

	private void MouseExited() 
	{
		AnimationPlayer anim  = btn.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("UpgradeDeselected");

		ShowDescriptionUi(false);
		ShowDescriptionUi("");
	}

	private void _OnButtonPress() 
	{
		if(IsInstanceValid(player)) {
			player.UpdateStats(health, movementForce, shotDelay, noOfBullets, bulletForce, bulletStrength, bulletAccuracy, bulletBurstAmount, bulletTimeAlive, bulletSize);
		}

		ShowDescriptionUi("");
		this.EmitSignal("_on_pressed", this);
		this.QueueFree();
	}

	private void ShowDescriptionUi(bool visiable) 
	{
		if(!showNames) { return; }
		Godot.Label label = btn.GetNode<Godot.Label>("Label");
		label.Visible = visiable;
	}

	private void ShowDescriptionUi(string value) 
	{
		if(!showDesc) { return; }
		this.EmitSignal("_update_upgrade_ui", value);
	}
}
