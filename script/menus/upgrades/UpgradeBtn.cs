using Godot;
using System.Collections.Generic;

public class UpgradeBtn : Position2D, IStats
{
	[Export] private string description = "";
	[Export] public bool endUpgrading = false;

    [Export] public int Health { get; set; }
    [Export] public float MovementForce { get; set; }
    [Export] public float ShotDelay { get; set; }
    [Export] public int NoOfBullets { get; set; }
    [Export] public float BulletForce { get; set; }
    [Export] public int BulletStrength { get; set; }
    [Export] public float BulletAccuracy { get; set; }
    [Export] public int BulletBurstAmount { get; set; }
    [Export] public float BulletTimeAlive { get; set; }
    [Export] public float BulletSize { get; set; }
    [Export] public List<string> OnBulletDestroyScenes { get; set; }

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
			EntityStats addStats = new EntityStats(this);
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