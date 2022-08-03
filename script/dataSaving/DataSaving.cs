using System;
using static Enums;
using Newtonsoft.Json; 

namespace Godot
{
    public class DataSaving
    {
        protected internal string fileName = "";

        private static FileSave file = new FileSave();

        public void SetValue(string key, object value) {
            SaveObj data = GetAllValues();
            data[key] = value;

            file.SaveObj(data, fileName);
        }

        public object GetValue(string key, object defaultVal) {
            SaveObj data = GetAllValues();
            return (data.ContainsKey(key)) ? data[key] : defaultVal;
        }

        public SaveObj GetAllValues() {
            object retrievedObj = file.RetrieveObj(fileName);
            return (retrievedObj != null) ? (SaveObj)file.RetrieveObj(fileName) : new SaveObj();
        }
    }
}