using Godot;

public partial class UiController
{
    private Godot.Label healthUi;

    #region GetElements

		private void GetHealthUi() {
			Godot.BoxContainer rightPanel = this.GetNode<Godot.BoxContainer>("HBoxContainer/HBoxContainer/VBoxContainer2");
			healthUi = rightPanel.GetNode<Godot.Label>("Health"); 
		}

	#endregion

    #region UpdateElements

		public void UpdateHealthUi(int value) {
			if(healthUi == null) { return; }
			healthUi.Text = value.ToString("D4");
		}

	#endregion
}