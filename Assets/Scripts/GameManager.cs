using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RotationManager rotationManager;
    [SerializeField] private CategoryListManager categoryListManager;

    [SerializeField] private TMP_Text scoreLabel;
    [SerializeField] private TMP_Text clueLabel;
    [SerializeField] private Transform gameGridRoot;
    [SerializeField] private GameObject gameListButtonPrefab;
    [SerializeField] private RadialTimer timer;

    [SerializeField] private GameObject menuRoot;
    [SerializeField] private GameObject gameRoot;
    [SerializeField] private GameObject endGameRoot;
    [SerializeField] private Transform endgameGuessRoot;
    [SerializeField] private GameObject endgameGuessPrefab;

    private float timelimit = 60;
    private List<string> currentCategoryList;
    private List<Tuple<string,bool>> currentGameGuesses;
    private List<string> fullCategoryList;
    private string currentClue;

    private int score = 0;
    void Start()
    {
        menuRoot.SetActive(true);
        gameRoot.SetActive(false);
        endGameRoot.SetActive(false);

        rotationManager.OnRotationChange += OnRotationChange;

        foreach (CategoryList categoryList in categoryListManager.GetGameLists())
        {
            GameObject gameListButton = Instantiate(gameListButtonPrefab, gameGridRoot);
            ListSelectionButton listSelectionButton = gameListButton.GetComponent<ListSelectionButton>();
            listSelectionButton.SetTitle(categoryList.listName);
            listSelectionButton.SetDescription(categoryList.listDescription);
            gameListButton.GetComponent<Button>().onClick.AddListener(() => StartGame(categoryList));
        }
    }

    private void StartGame(CategoryList gameList)
    {
        // Start the game
        currentCategoryList = new List<string>(gameList.list);
        fullCategoryList = new List<string>(gameList.list);
        currentGameGuesses = new List<Tuple<string,bool>>();

        //get first clue
        int startingClueIndex = UnityEngine.Random.Range(0, currentCategoryList.Count);
        currentClue = currentCategoryList[startingClueIndex];
        currentCategoryList.RemoveAt(startingClueIndex);
        StartCoroutine(GameLoop());

        Debug.Log($"Starting game: {gameList.listName}");


    }

    private IEnumerator GameLoop()
    {
        
        menuRoot.SetActive(false);
        gameRoot.SetActive(true);

        float remainingTime = timelimit;
        while (remainingTime > 0)
        {
            yield return null;
            remainingTime -= Time.deltaTime;
            timer.SetTimerPercentage(remainingTime / timelimit);
        }
        endGameRoot.SetActive(true);
        gameRoot.SetActive(false);
        foreach (Tuple<string, bool> guess in currentGameGuesses)
        {
            GameObject guessObject = Instantiate(endgameGuessPrefab, endgameGuessRoot);
            TMP_Text guessText = guessObject.GetComponent<TMP_Text>();
            guessText.text = guess.Item1;
            if (guess.Item2)
            {
                guessText.color = Color.green;
            }
            else
            {
                guessText.color = Color.red;
            }
        }
    }
    public void EndGame()
    {
        while (endgameGuessRoot.childCount > 0)
        {
            Destroy(endgameGuessRoot.GetChild(0).gameObject);
        }
        menuRoot.SetActive(true);
        gameRoot.SetActive(false);
        endGameRoot.SetActive(false);
    }

    private void OnRotationChange(object sender, RotationManager.RotationState e)
    {
        switch (e)
        {
            case RotationManager.RotationState.Down:
                CorrectGuess();
                break;
            case RotationManager.RotationState.Up:
                Pass();
                break;
        }
    }

    private void Pass()
    {
        currentGameGuesses.Add(new Tuple<string, bool>(currentClue, false));
        GetNewClue();
    }

    private void CorrectGuess()
    {
        score++;
        currentGameGuesses.Add(new Tuple<string,bool>(currentClue,true));
        GetNewClue();
        
    }

    private void GetNewClue()
    {
        if (currentCategoryList.Count <= 0)
        {
            currentCategoryList = new List<string>(fullCategoryList);
        }
        int clueIndex = UnityEngine.Random.Range(0, currentCategoryList.Count);
        currentClue = currentCategoryList[clueIndex];
        currentCategoryList.RemoveAt(clueIndex);
    }

    private void Update()
    {
        scoreLabel.text = score.ToString();
        clueLabel.text = currentClue;

    }
}
