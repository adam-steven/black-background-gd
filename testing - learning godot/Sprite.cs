using Godot;
using System;

public class Sprite : Godot.Sprite
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Hello");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		//Godot.Sprite child = this.GetNode<Godot.Sprite>("Sprite");

		float AMOUNT = 5f;
		if(Input.IsKeyPressed((int)KeyList.W)){
			this.Position += new Vector2(0, -AMOUNT);
			//child.GlobalPosition = new Vector2(0,0);
		}

		if(Input.IsKeyPressed((int)KeyList.S)){
			this.Position += new Vector2(0, AMOUNT);
		}

		if(Input.IsKeyPressed((int)KeyList.A)){
			this.Position += new Vector2(-AMOUNT, 0);
		}

		if(Input.IsKeyPressed((int)KeyList.D)){
			this.Position += new Vector2(AMOUNT, 0);
		}
	}
}
