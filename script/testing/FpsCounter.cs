using Godot;
using System.Threading.Tasks;

public partial class FpsCounter : Label
{
    public override void _Ready()
    {
        StartCounterAsync();
    }

    //Loop spawn bullets with delay for burst effect
    private async void StartCounterAsync()
    {
        while (true)
        {
            double fpsCount = Engine.GetFramesPerSecond();
            this.Text = fpsCount.ToString();
            await Task.Delay(2000);
        }
    }
}
