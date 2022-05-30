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
			if(!buttons[i].GetType().Equals(typeof(GameOverButtons))) { continue; }
		
			Godot.Button button = (Godot.Button)buttons[i];
			GD.Print(button.Name);
			button.Connect("on_pressed", this, "_OnButtonPress");
		}
	}

	//Handel score
	public override void LoadLevelParameters(System.Object sceneData) {
		if(sceneData == null) return;
		GD.Print(((MainGameObj)sceneData).isQuickReset);
	}

	private void _OnButtonPress(GameOverButtons button) {
		switch (button.action)
		{
			case MenuButtons.Play:
				Replay(button);
				break;
			case MenuButtons.Options:
				Options(button);
				break;
			case MenuButtons.Leaderboard:
				Leaderboard(button);
				break;
			case MenuButtons.Quit:
				Quit();
				break;
		}
	}

	private void Replay(GameOverButtons button) {
		MainGameObj restartObj = new MainGameObj(true);
		SceneController sceneScript = GetNode<SceneController>(Globals.scenePath);
		sceneScript.ChangeScene("res://scenes/Main.tscn", 5f, restartObj);
		button.Disabled = true;
	}

	private void Options(GameOverButtons button) {
		button.Disabled = true;
	}

	private void Leaderboard(GameOverButtons button) {
		button.Disabled = true;
	}

	private void Quit() {
		GetTree().Quit();
	}
}
