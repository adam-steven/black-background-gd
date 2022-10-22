using System;
using static Enums;
using Newtonsoft.Json;

namespace Godot
{
    public class MenuController : Control
    {
        //Button locations other that the default button location
        [Export] public string[] uniqueContainers = new string[0];

        [Signal] internal delegate void _play_game(); //starts or restarts the game
        [Signal] internal delegate void _options();
        [Signal] internal delegate void _leaderboard();
        [Signal] internal delegate void _main_menu();

        public override void _Ready()
        {
            Godot.VBoxContainer buttonContainer = this.GetNode<Godot.VBoxContainer>("Buttons");
            Godot.Collections.Array buttons = buttonContainer.GetChildren();

            //Get buttons in the unique containers
            for (int i = 0; i < uniqueContainers.Length; i++)
            {
                string containerPath = uniqueContainers[i];
                Godot.Container uniqueButtonContainer = this.GetNode<Godot.Container>(containerPath);
                buttons += uniqueButtonContainer.GetChildren();
            }

            for (int i = 0; i < buttons.Count; i++)
            {
                if (!buttons[i].GetType().Equals(typeof(MenuBtn))) { continue; }

                Godot.Button button = (Godot.Button)buttons[i];
                Godot.Collections.Array pressedBtn = new Godot.Collections.Array(button);
                button.Connect("button_down", this, "_OnButtonDown", pressedBtn);
                button.Connect("button_up", this, "_OnButtonUp", pressedBtn);
                button.Connect("pressed", this, "_OnButtonPress", pressedBtn);
            }

            _MenuReady();
        }

        internal virtual void _MenuReady() { }
        internal virtual void _OnButtonDown(MenuBtn button) { }
        internal virtual void _OnButtonUp(MenuBtn button) { }
        internal virtual void _OnButtonPress(MenuBtn button) { }
    }
}