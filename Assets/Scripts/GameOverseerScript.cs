﻿using UnityEngine;
using TMPro;

public class GameOverseerScript : MonoBehaviour
{

    [SerializeField]
    GameObject gameOverPanel;

    public enum GAMESTATE { READY, RUNNING, OVER };
    public GAMESTATE currentGameState;

    private void OnEnable()
    {
        AllEventsScript.OnBaseDestroyed += OnABaseDestroyed;
    }

    private void OnDisable()
    {
        AllEventsScript.OnBaseDestroyed -= OnABaseDestroyed;
    }

    private void Start()
    {
        currentGameState = GAMESTATE.RUNNING;
    }

    void OnGameOver(int destoyedBaseId)
    {
        Debug.Log(destoyedBaseId);

        AllEventsScript.OnGameOver?.Invoke();//Invoke on game over event

        if (gameOverPanel)//If game over panel available
        {
            //Display game over panel
            Transform gameWinTr = gameOverPanel.transform.Find("GameWinText");
            gameWinTr.GetComponent<TMP_Text>().text = "Base " + Invert(destoyedBaseId) + " wins !";
            gameOverPanel.SetActive(true);
        }

        currentGameState = GAMESTATE.OVER;
    }

    void OnABaseDestroyed(int id)
    {
        if (currentGameState != GAMESTATE.OVER)
            OnGameOver(id);
    }

    int Invert(int input)
    {
        return input == 0 ? 1 : 0;
    }

}
