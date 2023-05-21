using Godot;
using System;
using System.Collections.Generic;

public class UpgradeBtn : Position2D, IStats
{
	[Export] private string description = "";
	[Export] public bool endUpgrading = false;

	// [Export] internal int health = 0;
	// [Export] internal float movementForce = 0f;
	// [Export] internal float shotDelay = 0;
	// [Export] internal int noOfBullets = 0; //Number of bullets fired at once (Shotgun effect)
	// [Export] internal float bulletForce = 0; //Bullet's speed
	// [Export] internal int bulletStrength = 0; //Amount of damage the bullet does
	// [Export] internal float bulletAccuracy = 0; //Bullet's accuracy (0 is perfect accuracy)
	// [Export] internal int bulletBurstAmount = 0; //Number of bullets fired in quick succession (fixed delay interval)
	// [Export] internal float bulletTimeAlive = 0; //Bullet Range (>0 = 0.05f)
	// [Export] internal float bulletSize = 0; //Modifies the size of the bullet sprite
	// [Export] public List<string> onBulletDestroyScenes = new List<string>(); //Scenes to spawn in a random direction after the bullet is destroyed 

	[Export] public int health { get; set; }
	[Export] public float movementForce { get; set; }
	[Export] public float shotDelay { get; set; }
	[Export] public int noOfBullets { get; set; }
	[Export] public float bulletForce { get; set; }
	[Export] public int bulletStrength { get; set; }
	[Export] public float bulletAccuracy { get; set; }
	[Export] public int bulletBurstAmount { get; set; }
	[Export] public float bulletTimeAlive { get; set; }
	[Export] public float bulletSize { get; set; }
	[Export] public List<string> onBulletDestroyScenes { get; set; }

	[Signal] public delegate void _on_pressed(MenuBtn button);
	[Signal] public delegate void _update_upgrade_ui(string value);

	public Player player;
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

		_UniqueCalcOnLoad(player);
	}

	private void MouseEntered()
	{
		AnimationPlayer anim = btn.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("UpgradeSelected");

		ShowDescriptionUi(true);
		ShowDescriptionUi(description);
	}

	private void MouseExited()
	{
		AnimationPlayer anim = btn.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("UpgradeDeselected");

		ShowDescriptionUi(false);
		ShowDescriptionUi("");
	}

	private void _OnButtonPress()
	{
		if (IsInstanceValid(player))
		{
			_UniqueCalcOnPress(player);
			EntityStats addStats = new EntityStats(health, movementForce, shotDelay, noOfBullets, bulletForce, bulletStrength, bulletAccuracy, bulletBurstAmount, bulletTimeAlive, bulletSize, onBulletDestroyScenes);
			player.UpdateStats(addStats);
		}

		ShowDescriptionUi("");
		this.EmitSignal("_on_pressed", this);
		this.QueueFree();
	}

	private void ShowDescriptionUi(bool visiable)
	{
		if (!showNames) { return; }
		Godot.Label label = btn.GetNode<Godot.Label>("Label");
		label.Visible = visiable;
	}

	private void ShowDescriptionUi(string value)
	{
		if (!showDesc) { return; }
		this.EmitSignal("_update_upgrade_ui", value);
	}

	#region Unique Upgrade Helpers

	internal virtual void _UniqueCalcOnLoad(Player player) { }
	internal virtual void _UniqueCalcOnPress(Player player) { }

	#endregion
}
