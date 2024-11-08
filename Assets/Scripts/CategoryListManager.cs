using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class CategoryListManager : MonoBehaviour
{
    private List<CategoryList> categoryLists = new List<CategoryList>();

    void Awake()
    {
        //CopyJsonFiles(Path.Combine(Application.streamingAssetsPath,"default_game_lists"), Application.persistentDataPath);
        LoadGameLists();
    }

    public List<CategoryList> GetGameLists()
    {
        return categoryLists;
    }

    private void LoadGameLists()
    {
        List<string> fileContents = ReadFilesFromFolder();
        foreach (string fileContent in fileContents)
        {
            CategoryList gameList = JsonConvert.DeserializeObject<CategoryList>(fileContent);
            categoryLists.Add(gameList);
        }
    }

    private List<string> ReadFilesFromFolder()
    {
        List<string> ret = new List<string>();
        TextAsset[] gameLists = Resources.LoadAll<TextAsset>("GameLists/");
        foreach(TextAsset gameList in gameLists)
        {
            //Debug.Log(gameList.text);
            ret.Add(gameList.text);
        }
        return ret;
    }

    /*
    public IEnumerator ReadFilesFromFolder()
    {
        fileContents = new List<string>();
#if UNITY_ANDROID && !UNITY_EDITOR
        string folderPath = Path.Combine("jar:file://" + Application.dataPath + "!/assets", "default_game_lists");
#else
        string folderPath = Path.Combine( Application.streamingAssetsPath, "default_game_lists");
#endif
        string[] files = Directory.GetFiles(folderPath);

        foreach (string filePath in files)
        {
            // UnityWebRequest requires the path to be in URL format
            string url = Path.Combine(folderPath, Path.GetFileName(filePath));

            if (url.EndsWith(".json"))
            {
                using (UnityWebRequest request = UnityWebRequest.Get(url))
                {
                    // Wait for the request to complete
                    yield return request.SendWebRequest();

                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        // Add file content to the list
                        fileContents.Add(request.downloadHandler.text);
                        Debug.Log($"Content of {Path.GetFileName(filePath)}:\n{request.downloadHandler.text}");
                    }
                    else
                    {
                        Debug.LogError($"Failed to read file {Path.GetFileName(filePath)}: {request.error}");
                    }
                }
            }
        }

        // All files read successfully, do something with fileContents if needed
        // For example, you could combine them, store them, etc.
    }
    */
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
                File.Copy(file, destFile, true); // 'true' to overwrite if file already exists
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
public struct CategoryList
{
    public string listName;
    public string listDescription;
    public List<string> list;

    public CategoryList(string listName, string listDescription, List<string> list)
    {
        this.listName = listName;
        this.listDescription = listDescription;
        this.list = list;
    }
}
