using Godot;
using System;

public partial class signalTest : Sprite2D
{
	[Signal]
	public delegate void MoveEventHandler(float newX, float newY);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Timer timer = GetNode<Timer>("Timer");
		timer.WaitTime = 1;
		timer.Connect(Timer.SignalName.Timeout, new Callable(this, "on_timeout"));
		timer.Start();

		this.Connect(SignalName.Move, new Callable(this, "output_move_pos"));
	}

	void on_timeout(){
		float randX = (float)GD.RandRange(0, 500);
		float randY = (float)GD.RandRange(0, 500);
		this.Position = new Vector2(randX, randY);

		this.EmitSignal(SignalName.Move, randX,randY);
	}

	void output_move_pos(float posX, float posY){
		GD.Print(posX.ToString() + " " + posY.ToString());
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
