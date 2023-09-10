using Godot;

public partial class UiController
{
    private Godot.Label healthUi;
    private AnimationPlayer healthAnim;

    #region GetElements

    private void GetHealthUi()
    {
        Godot.BoxContainer rightPanel = this.GetNode<Godot.BoxContainer>("HBoxContainer/HBoxContainer/VBoxContainer2");
        healthUi = rightPanel.GetNode<Godot.Label>("Health");
        healthAnim = healthUi.GetNode<AnimationPlayer>("AnimationPlayer");
    }

    #endregion

    #region UpdateElements

    public void UpdateHealthUi(int value, bool healthIncrease)
    {
        if (healthUi is null) { return; }
        healthUi.Text = value.ToString("D4");

        //If health gain flash ui
        if(healthIncrease) { healthAnim.Play("HealthUiIncrease"); }
    }

    #endregion
}