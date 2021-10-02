using Godot;
using System;

public class GameController : Node2D
{
	// public override void _Ready() {}

	// public override void _Process(float delta) {}

	private void PlaceTestingDot(Vector2 tDotPos) {
		PackedScene testingDot = (PackedScene)GD.Load("res://scenes/TestingDot.tscn");

		Godot.Sprite tDot = (Godot.Sprite)testingDot.Instance();
		tDot.GlobalPosition = tDotPos;
		this.AddChild(tDot);
	}

	private void LogFrameRate() {
	  	GD.Print(Performance.GetMonitor(0));
	}
}
