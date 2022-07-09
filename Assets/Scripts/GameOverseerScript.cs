using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

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

        currentGameState = GAMESTATE.OVER;

        AllEventsScript.OnGameOver?.Invoke();//Invoke on game over event

        if (gameOverPanel)//If game over panel available
        {
            //Display game over panel
            Transform gameWinTr = gameOverPanel.transform.Find("GameWinText");
            //gameWinTr.GetComponent<TMP_Text>().text = "Base " + Invert(destoyedBaseId) + " wins !";
            gameWinTr.GetComponent<TMP_Text>().text = "You " + IdToName(destoyedBaseId);
            gameOverPanel.SetActive(true);
        }
        StartCoroutine(nameof(AutoBackRoutine));
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

    string IdToName(int id)
    {
        return id == 0 ? "Lose" : "Win!";
    }

    IEnumerator AutoBackRoutine()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);
    }

    //callbacks
    public void OnBackButtonPressed()
    {
        StopCoroutine(nameof(AutoBackRoutine));
        SceneManager.LoadScene(0);
    }

}
