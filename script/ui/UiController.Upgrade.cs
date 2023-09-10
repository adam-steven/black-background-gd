using Godot;

public partial class UiController
{
    private Godot.Label upgradeDescUi;

    #region GetElements

    private void GetUpgradeDescUi()
    {
        Godot.BoxContainer rightPanel = this.GetNode<Godot.BoxContainer>("HBoxContainer/HBoxContainer/VBoxContainer2");
        upgradeDescUi = rightPanel.GetNode<Godot.Label>("UpgradeDescription");
    }

    #endregion

    #region UpdateElements

    public void UpdateUpgradeDescUi(string value)
    {
        if (upgradeDescUi is null) { return; }
        upgradeDescUi.Text = value;
    }

    #endregion
}