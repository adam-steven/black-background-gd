using System;
using static Enums;
using Newtonsoft.Json; 

namespace Godot
{
    public class MenuController : Control
    {
        [Signal] public delegate void _play_game();
        [Signal] public delegate void _options();
        [Signal] public delegate void _leaderboard();
        [Signal] public delegate void _main_menu();
        [Signal] public delegate void upgrading_finished();

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

        public virtual void _MenuReady() {}
        public virtual void _OnButtonPress(MenuButtons button) {}
    }
}