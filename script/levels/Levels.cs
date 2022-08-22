using Newtonsoft.Json; 

namespace Godot
{
    public class Levels : Node2D
    {
        public Levels() {}

        internal Camera2D mainCamera;

        public virtual void LoadLevelParameters(System.Object sceneData) {}

        [Signal] internal delegate void change_scene(string scenePath, float animSpeed, string passThroughData);

        public void EmitChangeScene(string scenePath, float animSpeed = 1f, System.Object passThroughData = null) {
            var settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Objects;
            string jsonData  = JsonConvert.SerializeObject(passThroughData, settings);
            GD.Print(jsonData);
            this.EmitSignal("change_scene", scenePath, animSpeed, jsonData);
        }
    }
}