using Godot;
using System.Threading.Tasks;

public class FpsCounter : Label
{
    public override void _Ready()
    {
        StartCounter();
    }

    //Loop spawn bullets with delay for burst effect
	private async void StartCounter() {
        while (true)
        {
            float fpsCount = Engine.GetFramesPerSecond();
            this.Text = fpsCount.ToString();
            await Task.Delay(2000);
        }
	}
}
