using System.Collections.Generic;
using Godot;

public static class FileManager
{
    //Gets the scenes in a given path
    public static Scenes GetScenes(string path)
    {
        Scenes files = new Scenes();
        DirAccess dir = DirAccess.Open(path);
        dir.ListDirBegin();

        while (true)
        {
            string file = dir.GetNext();
            if (file == "") break;
            if (file.StartsWith(".")) continue;
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
        DirAccess dir = DirAccess.Open(path);
        dir.ListDirBegin();

        while (true)
        {
            string file = dir.GetNext();
            if (file == "") break;
            if (file.StartsWith(".")) continue;
            if (!dir.CurrentIsDir()) continue;

            Scenes files = GetScenes($"{path}{file}/");
            sectionedFiles.Add(files);
        }

        dir.ListDirEnd();
        return sectionedFiles;
    }
}