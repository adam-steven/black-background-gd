using Godot;
using System;
using static Enums;

public class PauseMenu : MenuController
{

	#region Handel Pause

		public override void _Input(InputEvent inputEvent) {
			if (inputEvent.IsActionPressed("Pause")) {
				TogglePause();
			}
		}

		private void TogglePause() {
			bool currentPauseState = GetTree().Paused;
			GetTree().Paused = !currentPauseState;
			this.Visible = !currentPauseState;
		}

		private void TogglePause(bool forcePause) {
			GetTree().Paused = forcePause;
			this.Visible = forcePause;
		}

	#endregion
	
	internal override void _OnButtonPress(MenuButtons button) {
		TogglePause(false);

		switch (button.action)
		{
			case MenuButtonActions.Continue:
				//Do nothing
				break;
			case MenuButtonActions.Play:
				Play(button);
				break;
			case MenuButtonActions.MainMenu:
				MainMenu(button);
				break;
			case MenuButtonActions.Options:
				Options(button);
				break;
			default:
				GD.Print($"Warning: unused btn action: {button.action}");
			break;	
		}
	}

	private void Play(MenuButtons button) {
		this.EmitSignal("_play_game");
		button.Disabled = true;
	}

	private void MainMenu(MenuButtons button) {
		this.EmitSignal("_main_menu");
		button.Disabled = true;
	}

	private void Options(MenuButtons button) {
		this.EmitSignal("_options");
		button.Disabled = true;
	}
}
