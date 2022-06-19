using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for recieving UI events and send commands to command center.
/// </summary>
[RequireComponent(typeof(CommandCenterScript))]
public class CCCommanderScript : MonoBehaviour
{
    [SerializeField]
    UIManager uiManager;

    public CommandCenterScript ccScript;

    enum HighlightedState { NONE, DEPLOY, DEFENCE, REMOVE };
    HighlightedState currHighlightedState;

    string commBuffer;

    private void OnEnable()
    {
        uiManager.OnUIButtonPressed += OnUICommand;
        uiManager.OnTouchCallback += OnScreenTap;//Subscribe to all events touch callback
    }

    private void OnDisable()
    {
        uiManager.OnUIButtonPressed -= OnUICommand;
        uiManager.OnTouchCallback -= OnScreenTap;
    }

    private void Awake()
    {
        commBuffer = "";
    }

    //------------------------------
    //UI Callbacks
    //------------------------------

    void OnUICommand(string str)//Listens to UI commands
    {
        commBuffer = str;
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
    }

    void OnScreenTap(string str)//Bound to allevents touch callback
    {
        commBuffer = str;
    }

    //Commands

    void OnDeployTankCommand(string str)
    {
        if (currHighlightedState == HighlightedState.NONE)
        {
            currHighlightedState = HighlightedState.DEPLOY;
            StartCoroutine(nameof(DeployStateCoroutine));
        }
    }

    void OnDefenceArtilleryCommand(string str)
    {
        if (currHighlightedState == HighlightedState.NONE)
        {
            currHighlightedState = HighlightedState.DEFENCE;
            StartCoroutine(nameof(DefenceStateCoroutine));
        }
    }

    void OnRemoveCommand(string str)
    {
        if (currHighlightedState == HighlightedState.NONE)
        {
            currHighlightedState = HighlightedState.REMOVE;
            StartCoroutine(nameof(RemoveStateCoroutine));
        }
    }


    //-----------------------
    //Coroutines-------------
    //-----------------------

    IEnumerator DeployStateCoroutine()
    {
        while (true)
        {
            Debug.Log("Deploy state");

            foreach (var item in ccScript.tankSpawnPoints)
            {
                item.GetComponent<SpriteRenderer>().enabled = true;
            }

            //While in deploy active state --start

            //If touched on an area
            //Deploy tank
            if (commBuffer.Contains("tch"))
            {
                if (commBuffer == "tch_deploy_0")
                {
                    ccScript.DeployTank(0);
                }
                else if (commBuffer == "tch_deploy_1")
                {
                    ccScript.DeployTank(1);
                }
                else if (commBuffer == "tch_deploy_2")
                {
                    ccScript.DeployTank(2);
                }
                break;
            }

            if (commBuffer == "b_deploy_back")//back button pressed
            {
                break;
            }

            //While in deploy active state --end
            yield return null;
        }

        foreach (var item in ccScript.tankSpawnPoints)
        {
            item.GetComponent<SpriteRenderer>().enabled = false;
        }
        currHighlightedState = HighlightedState.NONE;
    }

    IEnumerator DefenceStateCoroutine()
    {
        while (true)
        {
            Debug.Log("Defence state");

            foreach (var item in ccScript.artSpawnPoints)
            {
                item.GetComponent<SpriteRenderer>().enabled = true;
            }

            //While in deploy active state --start



            //If touched on an area
            //Deploy tank
            if (commBuffer.Contains("tch"))
            {
                if (commBuffer == "tch_deploy_0")
                {
                    ccScript.DeployTank(0);
                }
                else if (commBuffer == "tch_deploy_1")
                {
                    ccScript.DeployTank(1);
                }
                else if (commBuffer == "tch_deploy_2")
                {
                    ccScript.DeployTank(2);
                }
                break;
            }

            if (commBuffer == "b_deploy_back")//back button pressed
            {
                break;
            }

            //While in deploy active state --end
            yield return null;
        }

        foreach (var item in ccScript.tankSpawnPoints)
        {
            item.GetComponent<SpriteRenderer>().enabled = false;
        }
        currHighlightedState = HighlightedState.NONE;
        currHighlightedState = HighlightedState.NONE;
    }

    IEnumerator RemoveStateCoroutine()
    {
        while (true)
        {


            
            yield return null;
        }
        currHighlightedState = HighlightedState.NONE;
    }

    //-------------------
    //Others
    //-------------------

    public void HighLightTankSpawnPoints(int index, bool value)
    {
        ccScript.tankSpawnPoints[index].GetComponent<SpriteRenderer>().enabled = value;
    }

}
