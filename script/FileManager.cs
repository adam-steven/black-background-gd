using System.Collections.Generic;
using Godot;

public static class FileManager
{
    //Gets the scenes in a given path
    public static Scenes GetScenes(string path)
    {
        Scenes files = new Scenes();
        Directory dir = new Directory();
        dir.Open(path);
        dir.ListDirBegin();

        while (true)
        {
            string file = dir.GetNext();
            if (file == "") break;
            if (file.BeginsWith(".")) continue;
            if (!file.EndsWith(".tscn")) continue;

            files.Add($"{path}{file}");
        }

        dir.ListDirEnd();
        return files;
    }

    //Gets the scenes in a given path separated by the folders they are in
    public static SectionedScenes GetScenesViaFolders(string path)
    {
        SectionedScenes sectionedFiles = new SectionedScenes();
        Directory dir = new Directory();
        dir.Open(path);
        dir.ListDirBegin();

        while (true)
        {
            string file = dir.GetNext();
            if (file == "") break;
            if (file.BeginsWith(".")) continue;
            if (!dir.CurrentIsDir()) continue;

            Scenes files = GetScenes($"{path}{file}/");
            sectionedFiles.Add(files);
        }

        dir.ListDirEnd();
        return sectionedFiles;
    }
}