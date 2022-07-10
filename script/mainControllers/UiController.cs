using Godot;
using System;

public class UiController : Control
{
    private Godot.Label scoreUi;
    public int score { private set; get; }
    private int tempScore;
	public int scoreMultiplier { private set; get; }
    

    public override void _Ready()
    {
        ResetMultiplier();
    }

    public override void _PhysicsProcess(float delta) {
        if(score < tempScore) {

            //Need to truncate or power the score
            score = score + 1;
        }
    }

    #region Score 

		private void UpdateScore(int points) {
			tempScore = points * scoreMultiplier;
		}

		private void BreakScoreUpdate() {
			tempScore = score;
		}

        public void DecrementMultiplier () { scoreMultiplier--; }
        public void ResetMultiplier() { scoreMultiplier = 4; }

	#endregion
}
