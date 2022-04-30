using Godot;
using System;

public class MainMenu : Control
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    private void Play() {
        Godot.Node2D gameController = (Godot.Node2D)GetNode("/root/GameController");
		GameController controllerScript = (GameController)gameController;
		controllerScript.CheckIfEnemies();
    }

    private void Options() {

    }

    private void Quit() {
        GetTree().Quit();
    }

    //After all these event the main menu scene must be destroyed
}
