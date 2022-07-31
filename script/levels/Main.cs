using Godot;
using System;
using System.Collections.Generic;
using static Enums;

//Main.tscn 
public class Main : Levels
{
	private static Random rnd = new Random();

	private UiController ui;
	private Score scoreControl;

	private Godot.Node2D levelNode;
	private Vector2 levelCenter;
	
	private int level = 0;
	private int numberOfWaves = -1;

	private int enemySpawnMin = 0;
	private int enemySpawnMax = 2;
	private int noOfEnemies = 0;

	private Entities player;

	private List<string> enemies; //paths to enemy scenes
	private List<string> obstacles; //paths to obstacle scenes (dodge section)

	private bool isStartingCountDown;

	public override void _Ready() {
		//Reset the background color
		ColourController.UpdateBackgroundColour(10000);

		Godot.Control uiNode = this.GetNode<Godot.Control>("UI");
		ui = (UiController)uiNode; 

		scoreControl = new Score(ui);

		levelNode = this.GetNode<Godot.Node2D>("Level");
		levelCenter = levelNode.GlobalPosition;

		obstacles = FileManager.GetScenes(Globals.obstaclesFolder);
		enemies = FileManager.GetScenes(Globals.enemyFolder);
	}

	public override void LoadLevelParameters(System.Object sceneData) {
		MainGameObj mainData = (sceneData != null) 
			? (MainGameObj)sceneData 
			: new MainGameObj(false);

		if(mainData.isQuickReset) {
			Vector2 playerPos = new Vector2(960, 540);
			SpawnPlayer(playerPos);
			PlayGame();
		} else {
			Vector2 playerPos = new Vector2(1200, 540);
			SpawnPlayer(playerPos);
			SpawnMainMenu();
		}
	}

	public override void _Process(float delta) {
		scoreControl._ScoreProcess(delta);
	}

	#region Spawn Functions

		private void SpawnPlayer(Vector2 location) {
			PackedScene playerScene = (PackedScene)GD.Load("res://scenes/misc/Player.tscn");
			player = (Entities)playerScene.Instance();
			player.GlobalPosition = location;
			this.AddChild(player);

			player.Connect("_end_game", this, "EndGame");
			player.Connect("_shake_screen", (CameraController)mainCamera, "StartShakeScreen");
			player.Connect("_section_text", this, "DisplaySectionText");
			player.Connect("_destroy_all_bullets", this, "DestroyBullets");
			player.Connect("_update_score", scoreControl, "UpdateScore");
			player.Connect("_break_score_update", scoreControl, "BreakScoreUpdate");
		}

		private void SpawnMainMenu() {
			PackedScene mainMenuScene = (PackedScene)GD.Load("res://scenes/menus/MainMenu.tscn");
			Godot.Control mainMenu = (Godot.Control)mainMenuScene.Instance();
			this.AddChild(mainMenu);

			//PlayGame() requires a string to be passed in
			//as this is not needed in the _play_game signal a surrogate  param is added
			mainMenu.Connect("_play_game", this, "PlayGame", new Godot.Collections.Array(new string[1]));
			mainMenu.Connect("_options", this, "GoToOptions");
			mainMenu.Connect("_leaderboard", this, "GoToLeaderboard");
		}

		private void SpawnObstacles() {
			noOfEnemies = rnd.Next(enemySpawnMin, enemySpawnMax + 2);
			for (int i = 0; i < noOfEnemies; i++) {
				string randomObstacles = obstacles[rnd.Next(obstacles.Count)];
				PackedScene obstacleScene = (PackedScene)GD.Load(Globals.obstaclesFolder + randomObstacles);
				Enemies obstacle = (Enemies)obstacleScene.Instance();

				int spawnPosX = rnd.Next((int)-Globals.levelSize.x, (int)Globals.levelSize.x);
				int spawnPosY = rnd.Next((int)-Globals.levelSize.y, (int)Globals.levelSize.y);
				Vector2 spawnPosition = new Vector2(spawnPosX, spawnPosY) + levelCenter;
				obstacle.GlobalPosition = spawnPosition;

				obstacle.player = player;

				this.AddChild(obstacle);

				obstacle.Connect("_on_death", this, "CheckIfEnemies");
			}
		}

		private void SpawnEnemies() {
			noOfEnemies = rnd.Next(enemySpawnMin, enemySpawnMax + 1);
			for (int i = 0; i < noOfEnemies; i++) {
				string chosenEnemyScene = enemies[rnd.Next(enemies.Count)];
				PackedScene enemyScene = (PackedScene)GD.Load(Globals.enemyFolder + chosenEnemyScene);
				Enemies enemy = (Enemies)enemyScene.Instance();

				int spawnPosX = rnd.Next((int)-Globals.levelSize.x, (int)Globals.levelSize.x);
				int spawnPosY = rnd.Next((int)-Globals.levelSize.y, (int)Globals.levelSize.y);
				Vector2 spawnPosition = new Vector2(spawnPosX, spawnPosY) + levelCenter;
				enemy.GlobalPosition = spawnPosition;

				enemy.player = player;
				enemy.colour = ColourController.enemyColour;

				this.AddChild(enemy);

				enemy.Connect("_on_death", this, "CheckIfEnemies");
				enemy.Connect("_update_score", scoreControl, "UpdateScore");
			}
		}

		private void SpawnBoss() {
			GD.Print("Boss");
			SpawnEnemies();
		}

		private void SpawnUpgrades() {
			PackedScene upgradeMenuScene = (PackedScene)GD.Load("res://scenes/menus/UpgradeMenu.tscn");
			UpgradeMenu upgradeMenu = (UpgradeMenu)upgradeMenuScene.Instance();

			upgradeMenu.levelCenter = levelCenter;
			upgradeMenu.player = player;

			this.AddChild(upgradeMenu);

			//if the upgrading is finished call CheckIfEnemies to continue game
			upgradeMenu.Connect("_upgrading_finished", this, "UpgradingFinished", new Godot.Collections.Array(new string[1]));
			upgradeMenu.Connect("_decrease_multiplier", scoreControl, "DecrementMultiplier");
		}

	#endregion 

 	#region Navigation Functions

		private void PlayGame(string animName = "") {
			//count down
			isStartingCountDown = (bool)SettingsController.GetValue(MenuButtonActions.StartCountDown.ToString(), false);
			if(isStartingCountDown && animName != "SectionTextCountDown") {
				DisplaySectionTextCountDown("PlayGame");
				return;
			}
			
			this.LevelSpin();
			this.CheckIfEnemies();

			PackedScene pauseMenuScene = (PackedScene)GD.Load("res://scenes/menus/PauseMenu.tscn");
			Godot.Control pauseMenu = (Godot.Control)pauseMenuScene.Instance();
			pauseMenu.Visible = false;
			this.AddChild(pauseMenu);
			
			pauseMenu.Connect("_play_game", this, "RestartGame");
			pauseMenu.Connect("_main_menu", this, "ReturnToMenu");
			pauseMenu.Connect("_options", this, "GoToOptions");
		}

		private void RestartGame() {
			MainGameObj restartObj = new MainGameObj(true);
			EmitChangeScene("res://scenes/Main.tscn", 5f, restartObj);
		}

		private void EndGame() {
			GameOverObj deathObj = new GameOverObj(scoreControl.score, 0);
			EmitChangeScene("res://scenes/menus/DeathScreen.tscn", 1f, deathObj);
		}

		private void ReturnToMenu() {
			MainGameObj restartObj = new MainGameObj(false);
			EmitChangeScene("res://scenes/Main.tscn", 5f, restartObj);
		}

		private void GoToOptions() {
			bool inGame = level > 0;
			OptionsObj optionsObj = new OptionsObj(inGame);
			EmitChangeScene("res://scenes/menus/OptionsScreen.tscn", 5f, optionsObj);
		}

		private void GoToLeaderboard() {
			EmitChangeScene("res://scenes/menus/LeaderboardScreen.tscn", 5f);
		}

		private void UpgradingFinished(string animName = "") {
			//count down
			if(isStartingCountDown && animName != "SectionTextCountDown") {
				DisplaySectionTextCountDown("UpgradingFinished");
			} else {
				this.CheckIfEnemies();
			}
		}	

	#endregion

	#region Misc Functions

		//If the last enemy is dying spawn next way
		public void CheckIfEnemies() {
			noOfEnemies--;
			if(noOfEnemies > 0) return;

			//Waves > level + 2 obstacles 
			//Waves >0 basic enemies
			//Wave 0 bosses
			//Waves <0 Upgrades
			numberOfWaves--;
			if(numberOfWaves < -1) {
				level++;
				numberOfWaves = (level + 2) * 2;

				//Slowly increase the number of enemies each wave
				if(level%2 == 0) { enemySpawnMax++; } 
				else { enemySpawnMin++; }
			}

			GD.Print("Level: " + level + " Wave: " + numberOfWaves);

			if (numberOfWaves > level + 2) {

				if(numberOfWaves ==  (level + 2) * 2) { DisplaySectionText("DODGE"); }
				SpawnObstacles(); 
			}
			else if (numberOfWaves > 0) { 

				if(numberOfWaves == level + 2) { DisplaySectionText("FIGHT"); }
				SpawnEnemies(); 
			}
			else if (numberOfWaves == 0) { 
				DisplaySectionText("BOSS"); 
				SpawnBoss(); 
			} 
			else { 
				LevelSpin();
				scoreControl.ResetMultiplier();
				SpawnUpgrades();
			} 
		}

		//Displays big faint text in the background for a short amount of time
		//Used to indicate the changes in gameplay sections 
		private void DisplaySectionText(string text, bool inverted = false) {
			Position2D sectionText = this.GetNode<Position2D>("SectionText");
			Godot.Label label = sectionText.GetNode<Godot.Label>("Label");
			AnimationPlayer anim  = sectionText.GetNode<AnimationPlayer>("AnimationPlayer");

			string textColor = inverted ? "black" : "white";
			sectionText.Modulate = Color.ColorN(textColor);

			label.Text = text;
			anim.Play("SectionTxtDisplay");
		}

		private void DisplaySectionTextCountDown(string callFunction) {
			Position2D sectionText = this.GetNode<Position2D>("SectionText");
			Godot.Label label = sectionText.GetNode<Godot.Label>("Label");
			AnimationPlayer anim  = sectionText.GetNode<AnimationPlayer>("AnimationPlayer");
			
			anim.Connect("animation_finished", this, callFunction, null, (uint)Godot.Object.ConnectFlags.Oneshot);
			anim.Play("SectionTextCountDown");
		}

		//Spins the level boarders + changes the level colour
		private void LevelSpin() {
			Position2D room = levelNode.GetNode<Position2D>("Room");
			AnimationPlayer anim = room.GetNode<AnimationPlayer>("AnimationPlayer");
			anim.Play("RoomSpin");
			ColourController.UpdateGameColours(levelNode, player);
		}

		//Destroys all bullets on the screen
		private void DestroyBullets() {
			var children = this.GetChildren();

			foreach (var child in children)
				if(child.GetType() == typeof(BulletController))
					((BulletController)child).QueueFree(); 
		}

	#endregion 

	#region Testing Functions

	// private void PlaceTestingDot(Vector2 tDotPos) {
	// 	PackedScene testingDot = (PackedScene)GD.Load("res://scenes/TestingDot.tscn");
	// 	Godot.Sprite tDot = (Godot.Sprite)testingDot.Instance();
	// 	tDot.GlobalPosition = tDotPos;
	// 	this.AddChild(tDot);
	// }

	// private void LogFrameRate() {
	//   	GD.Print(Performance.GetMonitor(0));
	// }

	// private int currentColour = -1;
	// private string colourName;
	// private void RunThoughColours() {
	//     currentColour++;

	//     var values = Enum.GetValues(typeof(Enums.Colour));
	//     colourName = values.GetValue(currentColour).ToString();
		
	//     enemyColour = Color.ColorN(colourName);
	//     levelNode.Modulate = enemyColour;

	// 	GD.Print(colourName);
	// }

	#endregion

}
