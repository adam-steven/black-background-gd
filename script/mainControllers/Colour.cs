using Godot;
using System;
using System.Threading.Tasks;

public static class Colour
{
	private static Color[] gameColors = {
		Color.Color8(255, 231, 0), //yellow
		Color.Color8(255, 173, 0), //orange
		Color.Color8(255, 49, 49), //red
		Color.Color8(240, 0, 255), //pink
		Color.Color8(77, 238, 234), //cyan
		Color.Color8(204, 255, 0), //lime
	};

	public static Color levelColour { get; set; }
	public static Color harmonizingColour { get; set; }

	//Starts turning the background red if player health is less than 30
	public static void UpdateBackgroundColour(int playerHealth) {
		//make sure the number is never less than 0
		int red = Math.Max(0, 30 - playerHealth) * 2;
		VisualServer.SetDefaultClearColor(Color.Color8((byte)red,0,0));
	}

	//Flash a colour on the Scene of a sec + freeze frame
	public static async void FlashBackgroundColour(Color colourToFlash, SceneTree tree, int playerHealth) {
		VisualServer.SetDefaultClearColor(colourToFlash);
		tree.Paused = true;

		//Freeze frame always with colour flash to minimise seizure risk
		await Task.Delay(300);

		tree.Paused = false;
		UpdateBackgroundColour(playerHealth);
	}

	//Updates the Games colour scheme to a new random colour
	public static void UpdateGameColours(Godot.Node2D levelNode, Entities player) {
		Random rnd = new Random();
		int chosenColour = rnd.Next(gameColors.Length);
		Color newColour = gameColors[chosenColour];

		if(newColour == levelColour) { 
			chosenColour = incrementColour();
			newColour = gameColors[chosenColour]; 
		}

		levelColour = newColour;
		harmonizingColour = gameColors[incrementColour()];
		levelNode.Modulate = newColour;

		int incrementColour() {
			return (chosenColour + 1 >= gameColors.Length) ? 0 : chosenColour + 1;
		}
	}
}