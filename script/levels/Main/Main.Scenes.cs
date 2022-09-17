public partial class Main
{
    private void ResetScenes() 
    {
        mainData.obstacles.AddRange(obstaclesSections[0]);
		mainData.enemies.AddRange(enemiesSections[0]);
		mainData.upgrades.AddRange(upgradeSections[0]);
    }

    private void UpdateScenes(int level) 
    {
        if(obstaclesSections.Count > level) {
            mainData.obstacles.AddRange(obstaclesSections[level]);
        }

        if(enemiesSections.Count > level) {
            mainData.enemies.AddRange(enemiesSections[level]);
        }

        if(upgradeSections.Count > level) {
            mainData.upgrades.AddRange(upgradeSections[level]);
        }
    }
}