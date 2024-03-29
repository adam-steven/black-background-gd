using Newtonsoft.Json;

namespace Godot
{
    public class Level : Node2D
    {
        public Level() { }

        internal Camera2D mainCamera;

        internal SectionedScenes enemiesSections; //Paths to enemy scenes
        internal SectionedScenes obstaclesSections; //Paths to obstacle scenes
        internal SectionedScenes upgradeSections; //Paths to upgrade scenes

        public virtual void _LoadLevelParameters(System.Object sceneData) { }

        [Signal] internal delegate void _change_scene(string scenePath, float animSpeed, string passThroughData);

        public void EmitChangeScene(string scenePath, float animSpeed = 1f, System.Object passThroughData = null)
        {
            var settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Objects;
            string jsonData = JsonConvert.SerializeObject(passThroughData, settings);
            this.EmitSignal("_change_scene", scenePath, animSpeed, jsonData);
        }
    }
}