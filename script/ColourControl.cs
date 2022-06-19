using Godot;
using System;

public static class ColourControl
{
	//Starts turning the background red if player health is less than 30
	public static void UpdateBackgroundColour(int playerHealth) {
		//make sure the number is never less than 0
		int red = Math.Max(0, 30 - playerHealth) * 2;
		VisualServer.SetDefaultClearColor(Color.Color8((byte)red,0,0));
	}
}