using Godot;
using System.Collections.Generic;

public static class SettingsController
{
    private static string settingsFileName = "user://bb_stn_config.ini";
    private static string settingsSection = "bb_settings";

    public static void SetValue(string key, object value) {
        Godot.ConfigFile config = new ConfigFile();
        config.SetValue(settingsSection, key, value);
        config.Save(settingsFileName);  
    }

    public static object GetValue(string key, object defaultVal) {
        Godot.ConfigFile config = new ConfigFile();
        Error err = config.Load(settingsFileName);

        //If fail brake
        if(err != Error.Ok) { return null; }

        return config.GetValue(settingsSection, key, defaultVal);
    }

    public static List<KeyValuePair<string, object>> GetAllValues() {
        Godot.ConfigFile config = new ConfigFile();
        Error err = config.Load(settingsFileName);

        //If fail brake
        if(err != Error.Ok) { return null; }

        string[] settingKeys = config.GetSectionKeys(settingsSection);
        List<KeyValuePair<string, object>> settingsList = new List<KeyValuePair<string, object>>();

        for (int i = 0; i < settingKeys.Length; i++)
        {
            var keyValue = config.GetValue(settingsSection, settingKeys[i]);
            settingsList.Add(new KeyValuePair<string, object>(settingKeys[i], keyValue));
        }

        return settingsList;
    }
}