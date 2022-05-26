using System;

namespace Godot
{
    public class Levels : Node2D
    {
        public Levels() {}

        //
        // Summary:
        //     Global sceneData.
        public System.Object sceneData  { get; set; }

        [Signal]
        public delegate void change_scene();

        public void LoadLevelParameters(System.Object sceneData) {
            this.sceneData = sceneData;

            if(this.sceneData == null) return;
                GD.Print(((MainGameObj)this.sceneData).isQuickReset);
        }
    }
}