using System;
using Godot;

public partial class GameController
{
    public Color playerColour;
    public Color enemyColour;

    private void UpdateGameColours() {
        enemyColour = Color.Color8(0,255,255);

        levelNode.Modulate = enemyColour;
    }

    //Starts turning the background red if player health is less than 30
	public void UpdateBackgroundColour(int playerHealth) {
		//make sure the number is never less than 0
		int red = Math.Max(0, 30 - playerHealth) * 2;
		VisualServer.SetDefaultClearColor(Color.Color8((byte)red,0,0));
	}
}