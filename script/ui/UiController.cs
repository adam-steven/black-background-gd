using Godot;
using System;

public class UiController : Control
{

	private Godot.Label pointIndicatorUi;
	private AnimationPlayer pointIndicatorAnim;
	private Godot.Label scoreUi;
	private Godot.Label multiplierUi;
	private AnimationPlayer multiplierAnim;

	private TextureProgress waveIndicatorUi;

	public override void _Ready()
	{
		GetScoreUi();
		GetWaveIndicatorUi();
	}

	#region GetElements

		private void GetScoreUi() {
			Godot.BoxContainer leftPanel = this.GetNode<Godot.BoxContainer>("HBoxContainer/VBoxContainer");

			pointIndicatorUi = leftPanel.GetNode<Godot.Label>("PointsIndicator");
			pointIndicatorAnim = pointIndicatorUi.GetNode<AnimationPlayer>("AnimationPlayer");

			scoreUi = leftPanel.GetNode<Godot.Label>("Score");

			multiplierUi = leftPanel.GetNode<Godot.Label>("ScoreMultiplier");
			multiplierAnim = multiplierUi.GetNode<AnimationPlayer>("AnimationPlayer");
		}

		private void GetWaveIndicatorUi() {
			Godot.BoxContainer rightPanel = this.GetNode<Godot.BoxContainer>("HBoxContainer/HBoxContainer/VBoxContainer");
			waveIndicatorUi = rightPanel.GetNode<TextureProgress>("TextureProgress"); 
		}

	#endregion

	#region UpdateElements

		//-- Score --

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

		//-- Wave Indicator

		public void SetWaveSegments(int noOfSegments) {
			if(waveIndicatorUi == null) { return; }
			var waveMaterial = waveIndicatorUi.Material;
			(waveMaterial as ShaderMaterial).SetShaderParam("Segments", noOfSegments);
		}

		public void SetWaveProgress(double value) {
			if(waveIndicatorUi == null) { return; }
			waveIndicatorUi.Value = value;
		}

	#endregion
}
