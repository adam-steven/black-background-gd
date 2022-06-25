using Godot;
using System;
using static Enums;

//MainMenu.tscn 
public class MainMenu : Control
{
	public override void _Ready() {
		Godot.VBoxContainer buttonContainer = this.GetNode<Godot.VBoxContainer>("Buttons");
		Godot.Collections.Array buttons = buttonContainer.GetChildren();

		for (int i = 0; i < buttons.Count; i++)
		{
			if(!buttons[i].GetType().Equals(typeof(MenuButtons))) { continue; }
		
			Godot.Button button = (Godot.Button)buttons[i];
			button.Connect("on_pressed", this, "_OnButtonPress");
		}
	}

	private void _OnButtonPress(MenuButtons button) {
		switch (button.action)
		{
			case MenuButtonActions.Play:
				Play(button);
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

		this.QueueFree();
	}

	[Signal] public delegate void play_game();
	private void Play(MenuButtons button) {
		this.EmitSignal("play_game");
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
