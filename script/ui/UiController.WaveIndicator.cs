using Godot;

public partial class UiController
{
    private TextureProgress waveIndicatorUi;

    #region GetElements

    private void GetWaveIndicatorUi()
    {
        Godot.BoxContainer rightPanel = this.GetNode<Godot.BoxContainer>("HBoxContainer/HBoxContainer/VBoxContainer");
        waveIndicatorUi = rightPanel.GetNode<TextureProgress>("TextureProgress");
    }

    #endregion

    #region UpdateElements

    public void SetWaveSegments(int noOfSegments)
    {
        if (waveIndicatorUi is null) { return; }
        var waveMaterial = waveIndicatorUi.Material;
        (waveMaterial as ShaderMaterial).SetShaderParam("Segments", (noOfSegments - 1));
    }

    public void SetWaveProgress(double value)
    {
        if (waveIndicatorUi is null) { return; }
        waveIndicatorUi.Value = value;
    }

    #endregion
}