using Godot;
using static Enums;

public class GameOverButtons : Button
{
	[Export] MenuButtons action;

	public override void _Ready()
	{
		this.Connect("pressed", this, "_OnButtonPress");
		this.Connect("mouse_entered", this, "_OnMouseEntered");
		this.Connect("mouse_exited", this, "_OnMouseExit");
	}

	private void _OnButtonPress() {
		switch (action)
		{
			case MenuButtons.Play:
				Replay();
				break;
			case MenuButtons.Options:
				Options();
				break;
			case MenuButtons.Leaderboard:
				Leaderboard();
				break;
			case MenuButtons.Quit:
				Quit();
				break;
		}
	}

	//Play expand anim
	private void _OnMouseEntered() {
		AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("MenuBtnSelected");
	}

	//Play collapse anim
	private void _OnMouseExit() {
		AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("MenuBtnDeselected");
	}

	private void Replay() {
		MainGameObj restartObj = new MainGameObj(true);
		SceneController sceneScript = GetNode<SceneController>(Globals.scenePath);
		sceneScript.ChangeScene("res://scenes/Main.tscn", 5f, restartObj);
		this.Disabled = true;
	}

	private void Options() {
		this.Disabled = true;
	}

	private void Leaderboard() {
		this.Disabled = true;
	}

	private void Quit() {
		GetTree().Quit();
	}
}
