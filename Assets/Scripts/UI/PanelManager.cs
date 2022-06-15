using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    [SerializeField]
    GameObject panel_home;
    [SerializeField]
    GameObject panel_deploy;
    [SerializeField]
    GameObject panel_defend;

    enum PanelState { HOME, DEPLOY, DEFENSE };

    PanelState state;

    private void Start()
    {
        SetPanel(0);
    }

    public void SetPanel(int id)//{0:home,1:deploy,2:defend}
    {
        switch (id)
        {
            case 0:
                {
                    panel_home.SetActive(true);
                    panel_deploy.SetActive(false);
                    panel_defend.SetActive(false);
                    state = PanelState.HOME;
                }
                break;
            case 1:
                {
                    panel_home.SetActive(false);
                    panel_deploy.SetActive(true);
                    panel_defend.SetActive(false);
                    state = PanelState.DEPLOY;
                }
                break;
            case 2:
                {
                    panel_home.SetActive(false);
                    panel_deploy.SetActive(false);
                    panel_defend.SetActive(true);
                    state = PanelState.DEFENSE;
                }
                break;
            default:
                break;
        }
    }

    public void OnButtonPressed(string buttonName)
    {
        switch (state)
        {
            case PanelState.HOME:
                EvaluateHomeState(buttonName);
                break;
            case PanelState.DEPLOY:
                EvaluateDeployState(buttonName);
                break;
            case PanelState.DEFENSE:
                EvaluateDefenseState(buttonName);
                break;
            default:
                break;
        }
    }

    void EvaluateHomeState(string btName)
    {
        switch (btName)
        {
            case "b_deploy":
                {
                    SetPanel(1);
                }
                break;
            case "b_defence":
                { 
                    SetPanel(2);
                }
                break;
            default:
                break;
        }
    }

    void EvaluateDeployState(string btName)
    {
        switch (btName)
        {
            case "b_tank1":
                {

                }
                break;
            case "b_tank2":
                {

                }
                break;
            case "b_tank3":
                {

                }
                break;
            case "b_tank4":
                {

                }
                break;
            case "b_tank5":
                {

                }
                break;
            case "b_deploy_back":
                {
                    SetPanel(0);
                }
                break;
            default:
                break;
        }
    }

    void EvaluateDefenseState(string btName)
    {
        switch (btName)
        {
            case "b_art1":
                {

                }
                break;
            case "b_art2":
                {

                }
                break;
            case "b_art3":
                {

                }
                break;
            case "b_art4":
                {

                }
                break;
            case "b_defend_rem":
                {

                }
                break;
            case "b_defend_back":
                {
                    SetPanel(0);
                }
                break;
            default:
                break;
        }
    }



}
