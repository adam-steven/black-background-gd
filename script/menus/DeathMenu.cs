using Godot;
using System;
using static Enums;

public class DeathMenu : MenuController
{
	public override void _OnButtonPress(MenuButtons button) {
		switch (button.action)
		{
			case MenuButtonActions.Play:
				Replay(button);
				break;
			case MenuButtonActions.MainMenu: 
				MainMenu(button);
				break;
			case MenuButtonActions.Leaderboard:
				Leaderboard(button);
				break;
		}
	}

	private void Replay(MenuButtons button) {
		this.EmitSignal("_play_game");
		button.Disabled = true;
	}

	private void MainMenu(MenuButtons button) {
		this.EmitSignal("_main_menu");
		button.Disabled = true;
	}

	private void Leaderboard(MenuButtons button) {
		this.EmitSignal("_leaderboard");
		button.Disabled = true;
	}
}
