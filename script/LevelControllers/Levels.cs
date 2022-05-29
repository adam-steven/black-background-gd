using System;
using Newtonsoft.Json; 

namespace Godot
{
    public class Levels : Node2D
    {
        public Levels() {}

        //
        // Summary:
        //     Global sceneData.
        public System.Object sceneData  { get; set; }

        public virtual void LoadLevelParameters(System.Object sceneData) {
            this.sceneData = sceneData;
        }

        [Signal]
        public delegate void change_scene(string scenePath, float animSpeed, string passThroughData);

        public void EmitChangeScene(string scenePath, float animSpeed = 1f, System.Object passThroughData = null) {
            string jsonData = JsonConvert.SerializeObject(passThroughData);
            this.EmitSignal("change_scene", scenePath, animSpeed, jsonData);
        }
    }
}