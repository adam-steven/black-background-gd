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
                Play();
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

    private void Play() {
		//Reset the background color
		Godot.Node2D gameController = (Godot.Node2D)GetNode(Globals.gamePath);
		GameController gameScript = (GameController)gameController;
		gameScript.UpdateBackgroundColour(1000);

        Godot.Node2D sceneController = (Godot.Node2D)GetNode(Globals.scenePath);
		SceneController sceneScript = (SceneController)sceneController;
        sceneScript.ChangeScene("res://scenes/Main.tscn");
    }

    private void Options() {
    }

    private void Leaderboard() {
    }

    private void Quit() {
        GetTree().Quit();
    }
}
