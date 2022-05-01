using Godot;
using System;
using static Enums;

public class MainMenuButton : Button
{
    [Export] MainMenuBtn action;
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
            case MainMenuBtn.Play:
                Play();
                break;
            case MainMenuBtn.Options:
                Options();
                break;
            case MainMenuBtn.Quit:
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

    private void _OnMouseExit() {
        AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("MenuBtnDeselected");
    }

    private void Play() {
        Godot.Node2D gameController = (Godot.Node2D)GetNode("/root/GameController");
		GameController controllerScript = (GameController)gameController;
        controllerScript.LevelSpin();
		controllerScript.CheckIfEnemies();
    }

    private void Options() {

    }

    private void Quit() {
        GetTree().Quit();
    }
}
