using System.Collections.Generic;
using Godot;

public static class FileManager
{
    //Gets the scenes in a given path
    public static List<string> GetScenes(string path) {
        List<string> files = new List<string>();
		Directory dir = new Directory();
		dir.Open(path);
		dir.ListDirBegin();

		while (true) {
			string file = dir.GetNext();
			if(file == "") break;
			if(file.BeginsWith(".")) continue;
            if(!file.EndsWith(".tscn")) continue;
		
			files.Add(file);
		}
	
		dir.ListDirEnd();
		return files;
    }
}