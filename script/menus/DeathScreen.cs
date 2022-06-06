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
		GD.Print(((MainGameObj)sceneData).isQuickReset);
	}

	private void _OnButtonPress(MenuButtons button) {
		switch (button.action)
		{
			case MenuButtonActions.Play:
				Replay(button);
				break;
			case MenuButtonActions.Options:
				Options(button);
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

	private void Options(MenuButtons button) {
		button.Disabled = true;
	}

	private void Leaderboard(MenuButtons button) {
		button.Disabled = true;
	}

	private void Quit() {
		GetTree().Quit();
	}
}
