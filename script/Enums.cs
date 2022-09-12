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
		Pause,
		UpgradeName,
		UpgradeDesc,
	};
}


