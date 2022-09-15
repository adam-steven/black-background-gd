using Godot;
using System;
using static Enums;

//Main.tscn 
public partial class Main : Levels
{
	MainGameObj mainData = new MainGameObj(false);

	private static Random rnd = new Random();

	private Godot.Node2D levelNode;
	private Vector2 levelCenter;
	
	private int enemySpawnMin = 1;
	private int enemySpawnMax = 2;
	private int noOfEnemies = 0;

	private PlayerController player;

	private bool isStartingCountDown;

	private UiController uiNode;

	public override void _Ready() {
		uiNode = this.GetNode<UiController>("UI");

		levelNode = this.GetNode<Godot.Node2D>("Level");
		levelCenter = levelNode.GlobalPosition;

		this.SetProcess(false);
	}

	public override void LoadLevelParameters(System.Object sceneData) {
		if(sceneData != null) {
			mainData = (MainGameObj)sceneData;
		}

		if(mainData.inGame) {
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
		_ScoreProcess(delta);
		_StageProcess(delta);
	}

	#region Spawn Functions

		private void SpawnPlayer(Vector2 location) {
			PackedScene playerScene = (PackedScene)GD.Load("res://scenes/misc/Player.tscn");
			player = (PlayerController)playerScene.Instance();
			player.GlobalPosition = location;
			
			player.Connect("_end_game", this, "EndGame");
			player.Connect("_shake_screen", (Camera)mainCamera, "StartShakeScreen");
			player.Connect("_section_text", this, "DisplaySectionText");
			player.Connect("_destroy_all_bullets", this, "DestroyBullets");
			player.Connect("_update_score", this, "UpdateScore");
			player.Connect("_break_score_update", this, "BreakScoreUpdate");
			player.Connect("_player_left_camera", this, "ReframePlayer");
			player.Connect("_update_health_ui", uiNode, "UpdateHealthUi");

			this.AddChild(player);
		}

		private void SpawnMainMenu() {
			PackedScene mainMenuScene = (PackedScene)GD.Load("res://scenes/menus/MainMenu.tscn");
			Godot.Control mainMenu = (Godot.Control)mainMenuScene.Instance();
			
			mainMenu.Connect("_play_game", this, "PlayGame");
			mainMenu.Connect("_options", this, "GoToOptions");
			mainMenu.Connect("_leaderboard", this, "GoToLeaderboard");

			this.AddChild(mainMenu);
		}

		private void SpawnObstacles() {
			GD.Print("Response SpawnObstacles\n");

			int noToSpawn = rnd.Next(enemySpawnMin, enemySpawnMax + 2);
			noOfEnemies += noToSpawn;
			for (int i = 0; i < noToSpawn; i++) {
				Enemies obstacle = PickSpawnEntity(mainData.obstacles);
				this.AddChild(obstacle);
			}
		}

		private void SpawnEnemies() {
			GD.Print("Response SpawnEnemies\n");

			int noToSpawn = rnd.Next(enemySpawnMin, enemySpawnMax + 1);
			noOfEnemies += noToSpawn;
			for (int i = 0; i < noToSpawn; i++) {
				Enemies enemy = PickSpawnEntity(mainData.enemies);
				enemy.Connect("_update_score", this, "UpdateScore", new Godot.Collections.Array(mainData.stage.level));
				this.AddChild(enemy);
			}
		}

		private Enemies PickSpawnEntity(Scenes entityList) {
			string chosenEntityScene = entityList[rnd.Next(entityList.Count)];
			PackedScene entityScene = (PackedScene)GD.Load(chosenEntityScene);
			Enemies entity = (Enemies)entityScene.Instance();

			int spawnPosX = rnd.Next((int)-Globals.levelSize.x, (int)Globals.levelSize.x);
			int spawnPosY = rnd.Next((int)-Globals.levelSize.y, (int)Globals.levelSize.y);
			Vector2 spawnPosition = new Vector2(spawnPosX, spawnPosY) + levelCenter;
			entity.GlobalPosition = spawnPosition;

			entity.player = player;
			entity.colour = Colour.levelColour;
			entity.bulletColour = Colour.harmonizingColour;

			entity.Connect("_on_death", this, "CheckIfEnemies");

			return entity;
		}

		private void SpawnBoss() {
			GD.Print("Response SpawnBoss\n");
			SpawnEnemies();
		}

		private void SpawnUpgrades() {
			GD.Print("Response SpawnUpgrades\n");

			PackedScene upgradeMenuScene = (PackedScene)GD.Load("res://scenes/menus/UpgradeMenu.tscn");
			UpgradeMenu upgradeMenu = (UpgradeMenu)upgradeMenuScene.Instance();

			upgradeMenu.levelCenter = levelCenter;
			upgradeMenu.player = player;
			upgradeMenu.upgrades = mainData.upgrades;

			upgradeMenu.Connect("_upgrading_finished", this, "UpgradingFinished");
			upgradeMenu.Connect("_decrease_multiplier", this, "UpdateMultiplier");
			upgradeMenu.Connect("_update_upgrade_ui", uiNode, "UpdateUpgradeDescUi");

			this.AddChild(upgradeMenu);
		}

	#endregion 

 	#region Navigation Functions

		//play game for events without anim name info
		private void PlayGame() {
			mainData.inGame = true;
			mainData.obstacles = obstaclesSections[0];
			mainData.enemies = enemiesSections[0];
			mainData.upgrades = upgradeSections[0];

			_ScoreReady();
			_StageReady();

			PlayGame(string.Empty);
		}

		private void PlayGame(string animName = "") {
			//count down
			SettingsController settings = new SettingsController();
			isStartingCountDown = (bool)settings.GetValue(MenuButtonActions.StartCountDown.ToString(), false);
			if(isStartingCountDown && animName != "SectionTextCountDown") {
				DisplaySectionTextCountDown("PlayGame");
				return;
			}
			
			this.SetProcess(true);
			this.LevelSpin();
			this.NextStage(true);

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
			EmitChangeScene("res://scenes/gameEnvironment/Main.tscn", 5f, restartObj);
		}

		private void EndGame() {
			GameOverObj deathObj = new GameOverObj(mainData.score.score, 0);
			EmitChangeScene("res://scenes/menus/DeathScreen.tscn", 1f, deathObj);
		}

		private void ReturnToMenu() {
			MainGameObj restartObj = new MainGameObj(false);
			EmitChangeScene("res://scenes/gameEnvironment/Main.tscn", 5f, restartObj);
		}

		private void GoToOptions() {
			OptionsObj optionsObj = new OptionsObj(mainData);
			EmitChangeScene("res://scenes/menus/OptionsScreen.tscn", 5f, optionsObj);
		}

		private void GoToLeaderboard() {
			EmitChangeScene("res://scenes/menus/LeaderboardScreen.tscn", 5f);
		}

		//upgrading finished for events without anim name info
		private void UpgradingFinished() {
			UpgradingFinished(string.Empty);
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

			noOfEnemies = 0;
			NextStage();
		}

		//Spawn next stage;
		private void NextStage(bool gameStart = false)
		{
			bool newStage = NextWave(gameStart);
			GameStages currentStage = mainData.stage.currentStage;

			GD.Print("\nnewStage " + newStage);
			GD.Print("currentStage " + currentStage);

			if(newStage) { 
				DisplaySectionText(currentStage.ToString().ToUpper());
				LevelSpin(); 
			}

			switch (currentStage)
			{
				case GameStages.Dodge:
					GD.Print("Call SpawnObstacles");
					SpawnObstacles(); 
					break;
				case GameStages.Fight:
					GD.Print("Call SpawnEnemies");
					SpawnEnemies(); 
					break;
				case GameStages.Boss:
					GD.Print("Call SpawnBoss");
					SpawnBoss(); 
					break;
				case GameStages.Event:
					//Event
					break;
				default: 
					GD.Print("Call SpawnUpgrades");
					IncreaseEnemySpawn();
					UpdateMultiplier(true);
					SpawnUpgrades();
				break;
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
			AnimationPlayer anim  = this.GetNode<AnimationPlayer>("SectionText/AnimationPlayer");
			
			anim.Connect("animation_finished", this, callFunction, null, (uint)ConnectFlags.Oneshot);
			anim.Play("SectionTextCountDown");
		}

		//Spins the level boarders + changes the level colour
		private void LevelSpin() {
			AnimationPlayer anim = levelNode.GetNode<AnimationPlayer>("Room/AnimationPlayer");
			anim.Play("RoomSpin");
			Colour.UpdateGameColours(levelNode, player);
		}

		//Slowly increase the number of enemies each wave
		private void IncreaseEnemySpawn() {
			if(mainData.stage.level%2 == 0) { enemySpawnMax++; } 
			else { enemySpawnMin++; }
		}

		//Destroys all bullets on the screen
		private void DestroyBullets() {
			var children = this.GetChildren();

			foreach (var child in children)
				if(child.GetType() == typeof(BulletController))
					((BulletController)child).QueueFree(); 
		}

		private void ReframePlayer() {
			//If camera locked, move player to center
			Vector2 cameraCenter = mainCamera.GetCameraScreenCenter();
			player.Position = cameraCenter;

			//If camera free, move camera to player
		}

	#endregion 

	#region Testing Functions

	// private void PlaceTestingDot(Vector2 tDotPos) {
	// 	PackedScene testingDot = (PackedScene)GD.Load("res://scenes/TestingDot.tscn");
	// 	Godot.Sprite tDot = (Godot.Sprite)testingDot.Instance();
	// 	tDot.GlobalPosition = tDotPos;
	// 	this.AddChild(tDot);
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
