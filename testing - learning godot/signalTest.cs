using Godot;
using System;

public class signalTest : Sprite
{
	[Signal]
	public delegate void Move(float newX, float newY);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Timer timer = GetNode<Timer>("Timer");
		timer.WaitTime = 1;
		timer.Connect("timeout", this, "on_timeout");
		timer.Start();

		this.Connect("Move", this, "output_move_pos");
	}

	void on_timeout(){
		float randX = (float)GD.RandRange(0, 500);
		float randY = (float)GD.RandRange(0, 500);
		this.Position = new Vector2(randX, randY);

		this.EmitSignal("Move",randX,randY);
	}

	void output_move_pos(float posX, float posY){
		GD.Print(posX.ToString() + " " + posY.ToString());
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		
	}
}
