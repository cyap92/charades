using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameListManager : MonoBehaviour
{
    private List<GameList> gameLists = new List<GameList>();
    void Start()
    {
        CopyJsonFiles(Path.Combine(Application.streamingAssetsPath,"default_game_lists"), Application.persistentDataPath);
        LoadGameLists();
    }

    public List<GameList> GetGameLists()
    {
        return gameLists;
    }
    private void LoadGameLists()
    { 
        foreach (string fileContent in ReadFilesFromFolder(Application.persistentDataPath))
        {
            //Debug.Log(fileContent);
            GameList gameList = JsonConvert.DeserializeObject<GameList>(fileContent);
            gameLists.Add(gameList);
        }
    }

    public List<string> ReadFilesFromFolder(string folderPath)
    {
        List<string> fileContents = new List<string>();

        try
        {
            // Get all files from the specified folder
            string[] files = Directory.GetFiles(folderPath);

            foreach (string file in files)
            {
                // Read the content of each file and add to the list
                string content = File.ReadAllText(file);
                fileContents.Add(content);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return fileContents;
    }

    public void CopyJsonFiles(string sourceFolder, string destinationFolder)
    {
        try
        {
            // Ensure the destination folder exists; create if not
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            // Get all JSON files from the source folder
            string[] jsonFiles = Directory.GetFiles(sourceFolder, "*.json");

            foreach (string file in jsonFiles)
            {
                // Get the file name and combine with destination path
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(destinationFolder, fileName);

                // Copy the file to the destination
                File.Copy(file, destFile, false); // 'true' to overwrite if file already exists
            }

            Console.WriteLine("JSON files copied successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}

[SerializeField]
public struct GameList
{
    public string listName;
    public string listDescription;
    public List<string> list;

    public GameList(string listName, string listDescription, List<string> list)
    {
        this.listName = listName;
        this.listDescription = listDescription;
        this.list = list;
    }
}
