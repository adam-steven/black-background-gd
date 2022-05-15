using Godot;
using System;

public class SceneController : Node2D
{
	public void ChangeScene(string scenePath) {
		 GetTree().ChangeScene(scenePath);
	}
}