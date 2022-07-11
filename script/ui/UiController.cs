using Godot;
using System;

public class UiController : Control
{
	private Godot.Label scoreUi;

	public override void _Ready()
	{
		GetScoreUi();
	}

	#region GetElements

		private void GetScoreUi() {
			Godot.BoxContainer HBox = this.GetNode<Godot.BoxContainer>("HBoxContainer");
			Godot.BoxContainer VBoxLeft = HBox.GetNode<Godot.BoxContainer>("VBoxContainer");
			scoreUi = VBoxLeft.GetNode<Godot.Label>("Score");
		}

	#endregion

	#region UpdateElements

		public void UpdateScoreUi(int value) {
			scoreUi.Text = value.ToString();
		}

	#endregion
}
