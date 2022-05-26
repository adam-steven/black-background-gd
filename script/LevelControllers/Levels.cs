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

        public virtual void LoadLevelParameters(System.Object sceneData) {
            this.sceneData = sceneData;
        }
    }
}