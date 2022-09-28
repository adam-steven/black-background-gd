using Godot;

public partial class UiController
{
	private Godot.Label pointIndicatorUi;
	private AnimationPlayer pointIndicatorAnim;
	private Godot.Label scoreUi;
	private Godot.Label multiplierUi;
	private AnimationPlayer multiplierAnim;

	#region GetElements

	private void GetScoreUi()
	{
		Godot.BoxContainer leftPanel = this.GetNode<Godot.BoxContainer>("HBoxContainer/VBoxContainer");

		pointIndicatorUi = leftPanel.GetNode<Godot.Label>("PointsIndicator");
		pointIndicatorAnim = pointIndicatorUi.GetNode<AnimationPlayer>("AnimationPlayer");

		scoreUi = leftPanel.GetNode<Godot.Label>("Score");

		multiplierUi = leftPanel.GetNode<Godot.Label>("ScoreMultiplier");
		multiplierAnim = multiplierUi.GetNode<AnimationPlayer>("AnimationPlayer");
	}

	#endregion

	#region UpdateElements

	public void FlashPoints(int value)
	{
		if (pointIndicatorUi == null) { return; }
		pointIndicatorUi.Text = (value >= 0) ? $"+{value}" : value.ToString();
		pointIndicatorAnim.Play("PointsIndicatorAppear");
	}

	public void UpdateScoreUi(long value)
	{
		if (scoreUi == null) { return; }
		scoreUi.Text = value.ToString("D6");
	}

	public void UpdateMultiplierUi(int value)
	{
		if (multiplierUi == null) { return; }
		multiplierAnim.Connect("animation_finished", this, "MultiplierUiIdleAnim", null, (uint)Godot.Object.ConnectFlags.Oneshot);
		multiplierAnim.Play("multiplierChange");
		multiplierUi.Text = $"x{value}";
	}

	//Go back to idle animation after change anim finishes
	private void MultiplierUiIdleAnim(string animName = "")
	{
		multiplierAnim.Play("multiplierIdle");
	}

	#endregion
}
