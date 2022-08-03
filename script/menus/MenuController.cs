using System;
using static Enums;
using Newtonsoft.Json; 

namespace Godot
{
    public class MenuController : Control
    {
        [Signal] internal delegate void _play_game(); //starts or restarts the game
        [Signal] internal delegate void _options();
        [Signal] internal delegate void _leaderboard();
        [Signal] internal delegate void _main_menu();

        public override void _Ready() {
            Godot.VBoxContainer buttonContainer = this.GetNode<Godot.VBoxContainer>("Buttons");
            Godot.Collections.Array buttons = buttonContainer.GetChildren();

            for (int i = 0; i < buttons.Count; i++)
            {
                if(!buttons[i].GetType().Equals(typeof(MenuButtons))) { continue; }
            
                Godot.Button button = (Godot.Button)buttons[i];
                button.Connect("on_pressed", this, "_OnButtonPress");
            }

            _MenuReady();
        }

        internal virtual void _MenuReady() {}
        internal virtual void _OnButtonPress(MenuButtons button) {}
    }
}