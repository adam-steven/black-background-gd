using Godot;
using System;
using System.Threading.Tasks;

public static class SettingsController
{
    private static string settingsFileName = "user://bb_stn_config.ini";

    public static void SetValue(string section, string key, object value) {
        Godot.ConfigFile config = new ConfigFile();
        config.SetValue(section, key, value);
        config.Save(settingsFileName);  
    }

    public static object GetValue(string section, string key) {
        Godot.ConfigFile config = new ConfigFile();
        Error err = config.Load(settingsFileName);

        //If fail brake
        if(err != Error.Ok) { return null; }

        return config.GetValue(section, key);
    }

    public static object GetAllValues() {
        Godot.ConfigFile config = new ConfigFile();
        Error err = config.Load(settingsFileName);

        //If fail brake
        if(err != Error.Ok) { return null; }

        return null;
    }
}