using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RotationManager rotationManager;
    [SerializeField] private GameListManager gameListManager;
    [SerializeField] private TMP_Text scoreLabel;
    [SerializeField] private Transform gameGridRoot;
    [SerializeField] private GameObject gameListButtonPrefab;

    private int score = 0;
    void Start()
    {
        rotationManager.OnRotationChange += OnRotationChange;

        foreach (GameList gameList in gameListManager.GetGameLists())
        {
            GameObject gameListButton = Instantiate(gameListButtonPrefab, gameGridRoot);
            ListSelectionButton listSelectionButton = gameListButton.GetComponent<ListSelectionButton>();
            listSelectionButton.SetTitle(gameList.listName);
            listSelectionButton.SetDescription(gameList.listDescription);
            gameListButton.GetComponent<Button>().onClick.AddListener(() => StartGame(gameList));
        }
    }

    private void StartGame(GameList gameList)
    {
        // Start the game
    }

    private void OnRotationChange(object sender, RotationManager.RotationState e)
    {
        switch (e)
        {
            case RotationManager.RotationState.Down:
                if (score > 0)
                    score--;
                break;
            case RotationManager.RotationState.Up:
                score++;
                break;
        }
    }

    private void Update()
    {
        scoreLabel.text = score.ToString();
    }
}
