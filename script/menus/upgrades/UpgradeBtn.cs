using Godot;

public partial class UpgradeBtn : Marker2D, IStats
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
    [Export] public Godot.Collections.Array<string> OnBulletDestroyScenes { get; set; }

	[Signal] public delegate void OnPressedEventHandler(MenuBtn button); //Event: the menu button has been pressed
    [Signal] public delegate void UpdateUpgradeUiEventHandler(string value); //Event: request to display the upgrades description

	public Player player;
	public bool showNames;
	public bool showDesc;
	private Godot.Button btn;

	public override void _Ready()
	{
		btn = this.GetNode<Button>("Button");
		btn.Connect(Control.SignalName.MouseEntered, new Callable(this, "MouseEntered"));
		btn.Connect(Control.SignalName.MouseExited, new Callable(this, "MouseExited"));
		btn.Connect(BaseButton.SignalName.Pressed, new Callable(this, "_OnButtonPressed"));

		Label label = btn.GetNode<Label>("Label");
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

	private void _OnButtonPressed()
	{
		if (IsInstanceValid(player))
		{
			_UniqueCalcOnPress(player);
			EntityStats addStats = new EntityStats(this);
			player.UpdateStats(addStats);
		}

		ShowDescriptionUi("");
		this.EmitSignal(SignalName.OnPressed, this);
		this.QueueFree();
	}

	private void ShowDescriptionUi(bool visiable)
	{
		if (!showNames) { return; }
		Label label = btn.GetNode<Label>("Label");
		label.Visible = visiable;
	}

	private void ShowDescriptionUi(string value)
	{
		if (!showDesc) { return; }
		this.EmitSignal(SignalName.UpdateUpgradeUi, value);
	}

	#region Unique Upgrade Helpers

	internal virtual void _UniqueCalcOnLoad(Player player) { }
	internal virtual void _UniqueCalcOnPress(Player player) { }

	#endregion
}