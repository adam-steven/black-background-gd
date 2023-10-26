namespace Godot
{
    public partial class DataSaving
    {
        protected internal string fileName = "";

        private static FileSave file = new FileSave();

        public void SetValue(string key, object value)
        {
            Settings data = GetAllValues();
            data[key] = value;

            file.SaveObj(data, fileName);
        }

        public object GetValue(string key, object defaultVal)
        {
            Settings data = GetAllValues();
            return data.ContainsKey(key) ? data[key] : defaultVal;
        }

        public Settings GetAllValues()
        {
            object retrievedObj = file.RetrieveObj(fileName);
            return (retrievedObj is not null) ? (Settings)file.RetrieveObj(fileName) : new Settings();
        }
    }
}