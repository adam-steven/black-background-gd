using Godot;
using System;

public class UiController : Control
{

	private Godot.Label pointIndicatorUi;
	private AnimationPlayer pointIndicatorAnim;
	private Godot.Label scoreUi;
	private Godot.Label multiplierUi;
	private AnimationPlayer multiplierAnim;

	public override void _Ready()
	{
		GetScoreUi();
	}

	#region GetElements

		private void GetScoreUi() {
			Godot.BoxContainer HBox = this.GetNode<Godot.BoxContainer>("HBoxContainer");
			Godot.BoxContainer VBoxLeft = HBox.GetNode<Godot.BoxContainer>("VBoxContainer");

			pointIndicatorUi = VBoxLeft.GetNode<Godot.Label>("PointsIndicator");
			pointIndicatorAnim = pointIndicatorUi.GetNode<AnimationPlayer>("AnimationPlayer");

			scoreUi = VBoxLeft.GetNode<Godot.Label>("Score");

			multiplierUi = VBoxLeft.GetNode<Godot.Label>("ScoreMultiplier");
			multiplierAnim = multiplierUi.GetNode<AnimationPlayer>("AnimationPlayer");
		}

	#endregion

	#region UpdateElements

		public void FlashPoints(int value) {
			if(pointIndicatorUi == null) { return; }
			pointIndicatorUi.Text = $"+{value}";
			pointIndicatorAnim.Play("PointsIndicatorAppear");
		}

		public void UpdateScoreUi(long value) {
			if(scoreUi == null) { return; }
			scoreUi.Text = value.ToString("D6");
		}

		public void UpdateMultiplierUi(int value) {
			if(multiplierUi == null) { return; }
			multiplierAnim.Connect("animation_finished", this, "MultiplierUiIdleAnim", null, (uint)Godot.Object.ConnectFlags.Oneshot);
			multiplierAnim.Play("multiplierChange");
			multiplierUi.Text = $"x{value}";
		}

		//Go back to idle animation after change anim finishes
		private void MultiplierUiIdleAnim(string animName = "") {
			multiplierAnim.Play("multiplierIdle");
		}

	#endregion
}
