public static class Enums
{
	/// <summary> Indicates whether the bullet was spawned by the player or an enemy </summary>
	public enum BulletOwner {
		PlayerController,
		EnemyController,
	};

	/// <summary> The types of bullets </summary>
	public enum BulletVariations {
		Player,
		Normal,
		NormalStrong,
		Spectral,
		SpectralStrong,
	}

	/// <summary> The staves that can occur in a level </summary>
	public enum GameStages {
		Dodge,
		Fight,
		Boss,
		Shop,
		Event,
	}

	/// <summary> Buttons for Menu GUI </summary>
	public enum MenuButtonActions {
		MainMenu,
		Play, 
		Options,
		Leaderboard,
		Quit,
		Continue,
		StartCountDown,
		Up,
		Down,
		Left,
		Right,
		Shoot,
		Block,
		Pause
	};

	/// <summary> List of Godot's integrated colour </summary>
	public enum Colour {
		aqua, 
		aquamarine,  
		blue, 
		blueviolet,
		burlywood, 
		cadetblue, 
		chartreuse, 
		chocolate, 
		coral, 
		crimson, 
		cyan, 
		darkcyan, 
		darkgoldenrod,
		darkgreen, 
		darkkhaki,
		darkmagenta,
		darkorange, 
		darkorchid,
		darkred,
		darksalmon,
		darkseagreen, 
		darkslateblue,
		darkturquoise,
		darkviolet,
		deeppink,
		deepskyblue,
		dodgerblue,
		firebrick,
		forestgreen,
		fuchsia,
		gold, 
		goldenrod,
		green, 
		greenyellow,
		hotpink,			
		khaki,		
		lawngreen, 	 
		lightcoral,
		lightgreen, 
		lightseagreen,
		lime, 
		limegreen, 		 
		magenta, 
		maroon, 
		mediumaquamarine,		
		mediumorchid,
		mediumpurple, 
		mediumseagreen,
		mediumslateblue, 
		mediumspringgreen,  
		mediumturquoise, 
		mediumvioletred, 
		olivedrab,
		orange, 
		orangered, 
		orchid, 
		palegoldenrod,
		palegreen, 		
		palevioletred, 
		plum, 		
		purple, 
		rebeccapurple, 
		red,
		royalblue, 		
		salmon,		
		seagreen, 		 		 
		skyblue,
		slateblue, 				
		springgreen,
		steelblue, 		
		teal,		
		tomato,		 
		turquoise,
		violet, 		
		webgreen, 
		webmaroon, 
		webpurple, 		
		yellow,
		yellowgreen,
	}
}


