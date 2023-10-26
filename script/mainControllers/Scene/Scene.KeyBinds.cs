
using Godot;
using System;
using static Enums;
using System.Collections.Generic;

public partial class Scene
{
    private static Dictionary<MenuButtonActions, object> actions = new Dictionary<MenuButtonActions, object> {
            {MenuButtonActions.Up, Key.W},
            {MenuButtonActions.Down, Key.S},
            {MenuButtonActions.Left, Key.A},
            {MenuButtonActions.Right, Key.D},
            {MenuButtonActions.Shoot, MouseButton.Left},
            {MenuButtonActions.Block, MouseButton.Right},
            {MenuButtonActions.Pause, Key.Escape}
            };

    private void LoadKeyBinds()
    {
        SettingsController settings = new SettingsController();

        foreach (var action in actions)
        {
            object keyBind = settings.GetValue(action.Key.ToString(), action.Value);
            var actionList = InputMap.ActionGetEvents(action.Key.ToString());

            InputMap.ActionEraseEvent(action.Key.ToString(), (InputEvent)actionList[0]);

            if (keyBind.GetType().Equals(typeof(String)))
            {
                keyBind = DecodeStringBind(keyBind.ToString());
            }

            InputEvent newInput = CreateInputEvent(keyBind);
            InputMap.ActionAddEvent(action.Key.ToString(), newInput);
        }
    }

    private object DecodeStringBind(string keyBindCode)
    {
        char inputType = keyBindCode[0];
        bool codeParseSuccess = Int32.TryParse(keyBindCode.Remove(0, 1), out int code);

        if (codeParseSuccess)
        {
            switch (inputType)
            {
                case 'K': //KeyBoard
                    return (Key)code;
                case 'M': //Mouse
                    return (MouseButton)code;
                    // case 'C': //Controller
                    //     return (JoystickList)code;
            }
        }

        return null;
    }

    private InputEvent CreateInputEvent(object keyBind)
    {
        switch (keyBind)
        {
            case Key Key:
                InputEventKey newKey = new InputEventKey();
                newKey.PhysicalKeycode = Key;
                return newKey;
            case MouseButton MouseButton:
                InputEventMouseButton newMouseBtn = new InputEventMouseButton();
                newMouseBtn.ButtonIndex = MouseButton;
                return newMouseBtn;
            default:
                InputEventKey newEmptyKey = new InputEventKey();
                return newEmptyKey;
        }
    }
}