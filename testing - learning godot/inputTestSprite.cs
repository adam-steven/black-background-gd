using Godot;
using System;

public partial class inputTestSprite : Sprite2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	public override void _UnhandledInput(InputEvent @event){
		if(@event is InputEventMouseButton buttonEvent){
			if(buttonEvent.Pressed)
				this.Position = buttonEvent.GlobalPosition;
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(double delta)
//  {
//      
//  }
}
