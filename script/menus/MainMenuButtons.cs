using Godot;
using static Enums;

public class MainMenuButtons : Button
{
    [Export] MenuButtons action;
    Godot.Control mainMenu;

    public override void _Ready()
    {
        this.Connect("pressed", this, "_OnButtonPress");
        this.Connect("mouse_entered", this, "_OnMouseEntered");
        this.Connect("mouse_exited", this, "_OnMouseExit");

        mainMenu = this.GetParent<Godot.Control>();
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

        mainMenu.QueueFree(); 
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
        Godot.Node2D gameController = GetNode<SceneController>(Globals.scenePath).GetCurrentScene();
		GameController controllerScript = (GameController)gameController;
        controllerScript.LevelSpin();
		controllerScript.CheckIfEnemies();
    }

    private void Options() {
    }

    private void Leaderboard() {
    }

    private void Quit() {
        GetTree().Quit();
    }
}
