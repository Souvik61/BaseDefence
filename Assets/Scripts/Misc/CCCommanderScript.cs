using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CommandCenterScript))]
public class CCCommanderScript : MonoBehaviour
{
    [SerializeField]
    UIManager uiManager;

    public List<GameObject> tankSpawnPoints;

    enum HighlightedState { NONE, DEPLOY, DEFENCE, REMOVE };
    HighlightedState currHighlightedState;

    private void OnEnable()
    {
        uiManager.OnUIButtonPressed += OnUICommand;
    }

    private void OnDisable()
    {
        uiManager.OnUIButtonPressed -= OnUICommand;
    }

    private void Update()
    {
        
    }

    void OnUICommand(string str)//Listens to UI commands
    {
        //If any tank select command is issued
        if (str.Contains("b_tank"))
        {
            OnDeployTankCommand(str);
        }
        else if (str.Contains("b_art"))//If any artillery select command is issued
        {
            OnDefenceArtilleryCommand(str);
        }
        else if (str.Contains("b_defend_rem"))//If remove command is issued
        {
            OnRemoveCommand(str);
        }
        else if (str.Contains("tap"))
        {
            OnScreenTap(str);
        }
    
    }

    //Commands

    void OnDeployTankCommand(string str)
    {
        currHighlightedState = HighlightedState.DEPLOY;
        StartCoroutine(nameof(DeployStateCoroutine));
    }

    void OnDefenceArtilleryCommand(string str)
    {
        currHighlightedState = HighlightedState.DEFENCE;
        StartCoroutine(nameof(DefenceStateCoroutine));
    }

    void OnRemoveCommand(string str)
    {
        currHighlightedState = HighlightedState.REMOVE;
        StartCoroutine(nameof(RemoveStateCoroutine));
    }

    void OnScreenTap(string str)
    { 
        
    }

    //-----------------------
    //Coroutines-------------
    //-----------------------

    IEnumerator DeployStateCoroutine()
    {
        while (true)
        {



            yield return null;
        }
    }

    IEnumerator DefenceStateCoroutine()
    {
        while (true)
        {



            yield return null;
        }
    }

    IEnumerator RemoveStateCoroutine()
    {
        while (true)
        {



            yield return null;
        }
    }

    //-------------------
    //Others
    //-------------------


    public void ActiveHighLightTankSpawnPoints(int index)
    {
        tankSpawnPoints[index].GetComponent<SpriteRenderer>().enabled = true;
    }

    public void DeActiveHighLightTankSpawnPoints(int index)
    {
        tankSpawnPoints[index].GetComponent<SpriteRenderer>().enabled = false;
    }
}
