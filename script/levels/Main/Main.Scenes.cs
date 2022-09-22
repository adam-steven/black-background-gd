public partial class Main
{
    private void ResetScenes()
    {
        mainData.Obstacles.AddRange(obstaclesSections[0]);
        mainData.Enemies.AddRange(enemiesSections[0]);
        mainData.Upgrades.AddRange(upgradeSections[0]);
    }

    private void UpdateScenes(int level)
    {
        if (obstaclesSections.Count > level)
        {
            mainData.Obstacles.AddRange(obstaclesSections[level]);
        }

        if (enemiesSections.Count > level)
        {
            mainData.Enemies.AddRange(enemiesSections[level]);
        }

        if (upgradeSections.Count > level)
        {
            mainData.Upgrades.AddRange(upgradeSections[level]);
        }
    }
}