using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cmplx_statemachine;

public class TankAIScript3 : MonoBehaviour
{
    public ArmyBaseScript targetBase;

    [SerializeField]
    FOVObsCheckScript obsCheckScript;

    cmplx_statemachine.TankAIStateMachine stateMachine;
    public List<GameObject> enemiesInSight;

    public string currentStateName;

    TankScript tankController;

    private void OnEnable()
    {
        AllEventsScript.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        AllEventsScript.OnGameOver -= OnGameOver;
    }

    private void Awake()
    {
        stateMachine = GetComponent<cmplx_statemachine.TankAIStateMachine>();
    }

    private void Start()
    {
        InitStateMachine();
    }

    private void Update()
    {
        CalculateState();
        CalculateTargetProperties();
    }

    private void OnDestroy()
    {
        //stateMachine.Exit();
    }

    void CalculateTargetProperties()
    {
        enemiesInSight.Clear();//Clear enemy list
        //Add enemies to enemy list from obstacle check script
        if (obsCheckScript.isObstaclesInRange)//If obstacles in range
        {
            foreach (var item in obsCheckScript.obstaclesInRange)
            {
                if (CheckIfAnObsIsEnemy(item))//If item is an enemy add to enemy list
                {
                    enemiesInSight.Add(item);
                }
            }
        }

    }

    bool CheckIfAnObsIsEnemy(GameObject item)
    {
        TankScript othTank;
        if (item != null && item.layer == LayerMask.NameToLayer("Tank"))//If item is in tank layer
        {
            item.TryGetComponent<TankScript>(out othTank);
            if (othTank != null)//if tankscript not equal to null//If it is a tank
            {
                if (othTank.GetHealthScript().currentHP > 0)//if tankhealth>0
                {
                    if (!othTank.CompareTag(tag))//if not on same team
                    {
                        return true;
                    }
                }

            }
            else//If it is an artilery
            {
                if (item.GetComponent<ArtileryScript>() != null)//If artilery type 1
                {
                    if (item.GetComponent<ArtileryScript>().GetHealthScript().currentHP > 0)
                    {
                        if (!item.CompareTag(tag))//if not on same team
                        {
                            return true;

                        }
                    }
                }
                else//If artilery type 2
                {
                    if (item.GetComponent<Artilery_t1Script>().GetHealthScript().currentHP > 0)
                    {
                        if (!item.CompareTag(tag))//if not on same team
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    void CalculateState()
    {


    }

    void InitStateMachine()
    {
       // stateMachine = new TankAIStateMachine2(this);

        stateMachine.Initialize("APPR_BASE");

    }

    void OnGameOver()
    {
        //stateMachine.ChangeState("GAME_OVR");
    }

}
