
using Godot;
using System;
using static Enums;
using System.Collections.Generic;

public partial class Scene
{
    private static Dictionary<MenuButtonActions, object> actions = new Dictionary<MenuButtonActions, object> {
            {MenuButtonActions.Up, KeyList.W},
            {MenuButtonActions.Down, KeyList.S},
            {MenuButtonActions.Left, KeyList.A},
            {MenuButtonActions.Right, KeyList.D},
            {MenuButtonActions.Shoot, ButtonList.Left},
            {MenuButtonActions.Block, ButtonList.Right},
            {MenuButtonActions.Pause, KeyList.Escape}
            };

    private void LoadKeyBinds()
    {
        SettingsController settings = new SettingsController();

        foreach (var action in actions)
        {
            object keyBind = settings.GetValue(action.Key.ToString(), action.Value);
            var actionList = InputMap.GetActionList(action.Key.ToString());

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
                    return (KeyList)code;
                case 'M': //Mouse
                    return (ButtonList)code;
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
            case KeyList keyList:
                InputEventKey newKey = new InputEventKey();
                newKey.PhysicalScancode = (uint)(int)keyBind;
                return newKey;
            case ButtonList buttonList:
                InputEventMouseButton newMouseBtn = new InputEventMouseButton();
                newMouseBtn.ButtonIndex = (int)keyBind;
                return newMouseBtn;
            default:
                InputEventKey newEmptyKey = new InputEventKey();
                return newEmptyKey;
        }
    }
}