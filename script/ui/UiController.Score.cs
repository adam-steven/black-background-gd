using Godot;

public partial class UiController
{
	private Label pointIndicatorUi;
	private AnimationPlayer pointIndicatorAnim;
	private Label scoreUi;
	private Label multiplierUi;
	private AnimationPlayer multiplierAnim;

	#region GetElements

	private void GetScoreUi()
	{
		BoxContainer leftPanel = this.GetNode<BoxContainer>("HBoxContainer/VBoxContainer");

		pointIndicatorUi = leftPanel.GetNode<Label>("PointsIndicator");
		pointIndicatorAnim = pointIndicatorUi.GetNode<AnimationPlayer>("AnimationPlayer");

		scoreUi = leftPanel.GetNode<Label>("Score");

		multiplierUi = leftPanel.GetNode<Label>("ScoreMultiplierControl/ScoreMultiplier");
		multiplierAnim = multiplierUi.GetNode<AnimationPlayer>("AnimationPlayer");
	}

	#endregion

	#region UpdateElements

	public void FlashPoints(int value)
	{
		if (pointIndicatorUi is null) { return; }
		pointIndicatorUi.Text = (value >= 0) ? $"+{value}" : value.ToString();
		pointIndicatorAnim.Play("PointsIndicatorAppear");
	}

	public void UpdateScoreUi(long value)
	{
		if (scoreUi is null) { return; }
		scoreUi.Text = value.ToString("D6");
	}

	public void UpdateMultiplierUi(int value)
	{
		if (multiplierUi is null) { return; }
		multiplierAnim.Connect(AnimationPlayer.SignalName.AnimationFinished, new Callable(this, "MultiplierUiIdleAnim"), (uint)ConnectFlags.OneShot);
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
