using Newtonsoft.Json;

namespace Godot
{
    public partial class Level : Node2D
    {
        public Level() { }

        internal Camera2D mainCamera;

        internal SectionedScenes enemiesSections; //Paths to enemy scenes
        internal SectionedScenes obstaclesSections; //Paths to obstacle scenes
        internal SectionedScenes upgradeSections; //Paths to upgrade scenes

        [Signal] public delegate void ChangeSceneEventHandler(string scenePath, float animSpeed, string passThroughData); //Event: change the game scene

        public virtual void _LoadLevelParameters(object sceneData) { }

        public void EmitChangeScene(string scenePath, float animSpeed = 1f, object passThroughData = null)
        {
            var settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Objects;
            string jsonData = JsonConvert.SerializeObject(passThroughData, settings);
            this.EmitSignal(SignalName.ChangeScene, scenePath, animSpeed, jsonData);
        }
    }
}