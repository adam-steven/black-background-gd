using Godot;
using System;
using static Enums;

//DeathScreen.tscn 
public class DeathScreen : Levels
{
	public override void _Ready() {
		Godot.Control control = this.GetNode<Godot.Control>("Control");
		Godot.VBoxContainer buttonContainer = control.GetNode<Godot.VBoxContainer>("Buttons");
		Godot.Collections.Array buttons = buttonContainer.GetChildren();

		for (int i = 0; i < buttons.Count; i++)
		{
			if(!buttons[i].GetType().Equals(typeof(MenuButtons))) { continue; }
		
			Godot.Button button = (Godot.Button)buttons[i];
			button.Connect("on_pressed", this, "_OnButtonPress");
		}
	}

	//Handel score
	public override void LoadLevelParameters(System.Object sceneData) {
		if(sceneData == null) return;
	}

	private void _OnButtonPress(MenuButtons button) {
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
			case MenuButtonActions.Quit:
				Quit();
				break;
		}
	}

	private void Replay(MenuButtons button) {
		MainGameObj restartObj = new MainGameObj(true);
		EmitChangeScene("res://scenes/Main.tscn", 5f, restartObj);
		button.Disabled = true;
	}

	private void MainMenu(MenuButtons button) {
		MainGameObj restartObj = new MainGameObj(false);
		EmitChangeScene("res://scenes/Main.tscn", 5f, restartObj);
		button.Disabled = true;
	}

	private void Leaderboard(MenuButtons button) {
		EmitChangeScene("res://scenes/menus/LeaderboardScreen.tscn", 5f);
		button.Disabled = true;
	}

	private void Quit() {
		GetTree().Quit();
	}
}
