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
    public WalletScript walletScript;
    public CurrencyDefsSO currencyTerms;

    enum HighlightedState { NONE, DEPLOY, DEFENCE, REMOVE };
    HighlightedState currHighlightedState;

    string commBuffer;

    int toBeSpawnedTankIndex;
    int toBeSpawnedArtIndex;

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
        toBeSpawnedTankIndex = -1;
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
            toBeSpawnedTankIndex = (int)char.GetNumericValue(str[str.Length - 1]);
            currHighlightedState = HighlightedState.DEPLOY;
            StartCoroutine(nameof(DeployStateCoroutine));
        }
    }

    void OnDefenceArtilleryCommand(string str)
    {
        if (currHighlightedState == HighlightedState.NONE)
        {
            toBeSpawnedArtIndex = (int)char.GetNumericValue(str[str.Length - 1]);
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
                item.GetComponent<SpriteRenderer>().color = Color.green;
            }

            //While in deploy active state --start

            //If touched on an area
            //Deploy tank
            if (commBuffer.Contains("tch"))
            {
                if (commBuffer == "tch_deploy_0")
                {
                    BuyAndDeployTank(0, toBeSpawnedTankIndex);
                }
                else if (commBuffer == "tch_deploy_1")
                {
                    BuyAndDeployTank(1, toBeSpawnedTankIndex);
                }
                else if (commBuffer == "tch_deploy_2")
                {
                    BuyAndDeployTank(2, toBeSpawnedTankIndex);
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
        toBeSpawnedTankIndex = -1;
        currHighlightedState = HighlightedState.NONE;
    }

    IEnumerator DefenceStateCoroutine()
    {
        while (true)
        {
            Debug.Log("Defence state");

            for (int i = 0; i < ccScript.artSpawnPoints.Count; i++)
            {
                if (IsArtilleryAreaAvailable(i))
                {
                    ccScript.artSpawnPoints[i].GetComponent<SpriteRenderer>().color = Color.green;
                    ccScript.artSpawnPoints[i].GetComponent<SpriteRenderer>().enabled = true;
                }
            }
            //While in deploy active state --start



            //If touched on an area
            //Deploy artillery
            if (commBuffer.Contains("tch"))
            {
                if (commBuffer == "tch_art_0")
                {
                    ccScript.DeployArtillery(0, toBeSpawnedArtIndex);
                }
                else if (commBuffer == "tch_art_1")
                {
                    ccScript.DeployArtillery(1, toBeSpawnedArtIndex);
                }
                else if (commBuffer == "tch_art_2")
                {
                    ccScript.DeployArtillery(2, toBeSpawnedArtIndex);
                }
                break;
            }

            if (commBuffer == "b_defend_back")//back button pressed
            {
                break;
            }

            //While in deploy active state --end
            yield return null;
        }

        foreach (var item in ccScript.artSpawnPoints)
        {
            item.GetComponent<SpriteRenderer>().enabled = false;
        }
        currHighlightedState = HighlightedState.NONE;
    }

    IEnumerator RemoveStateCoroutine()
    {
        for (int i = 0; i < ccScript.artSpawnPoints.Count; i++)
        {
            if (!IsArtilleryAreaAvailable(i))
            {
                ccScript.artSpawnPoints[i].GetComponent<SpriteRenderer>().enabled = true;
                ccScript.artSpawnPoints[i].GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
        while (true)
        {

            //If touched on an area
            //Remove artillery
            if (commBuffer.Contains("tch"))
            {
                if (commBuffer == "tch_art_0")
                {
                    ccScript.RemoveArtilleryAt(0);
                }
                else if (commBuffer == "tch_art_1")
                {
                    ccScript.RemoveArtilleryAt(1);
                }
                else if (commBuffer == "tch_art_2")
                {
                    ccScript.RemoveArtilleryAt(2);
                }
                break;
            }

            yield return null;
        }

        for (int i = 0; i < ccScript.artSpawnPoints.Count; i++)
        {
            ccScript.artSpawnPoints[i].GetComponent<SpriteRenderer>().enabled = false;
            ccScript.artSpawnPoints[i].GetComponent<SpriteRenderer>().color = Color.white;
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

    bool IsArtilleryAreaAvailable(int a)
    {
        var sP = ccScript.artSpawnPoints[a];
        Collider2D[] coll = Physics2D.OverlapBoxAll(ccScript.artSpawnPoints[a].transform.position, sP.GetComponent<SpriteRenderer>().bounds.size, 0);

        //Check if any Tank collider is there
        foreach (var item in coll)
            if (item.gameObject.layer == LayerMask.NameToLayer("Tank"))
                return false;
            
        return true;
    }

    bool BuyTank(int index)
    {
        uint cost = currencyTerms.costList[index-1];

        if (cost > walletScript.currentCoins)
        {
            return false;
        }
        else if (cost <= walletScript.currentCoins)
        {
            walletScript.Withdraw(cost);
            return true;
        }
        return true;
    }

    void BuyAndDeployTank(int pos,int tankIndex)
    {
        if (BuyTank(tankIndex))//if tank bought succesfully
        {
            ccScript.DeployTank(pos, tankIndex);
        }
        else
            uiManager.PromptMessage("No coins bro!");

    }

}
