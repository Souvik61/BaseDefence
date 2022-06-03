using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tank AI Component
/// </summary>
public class TankAIScript3 : MonoBehaviour
{
    [SerializeField]
    cmplx_statemachine.TankAIStateMachine stateMachine;

    public ArmyBaseScript_pt1 targetBase;
    [HideInInspector]
    public UnitComponent unitComp;
    [SerializeField]
    FOVObsCheckScript obsCheckScript;
    public List<GameObject> enemiesInSight;
    
    //private
    private GameObject prev0thEnemy;

    TankScript tankController;

    public Compass compass;

    SeekerModuleScript seekerModule;

    public SeekerModuleScript GetSeekerModule
    {
        get { return seekerModule; }
    }

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
        //stateMachine = GetComponentInChildren<cmplx_statemachine.TankAIStateMachine>();
        unitComp = GetComponent<UnitComponent>();
        seekerModule = GetComponentInChildren<SeekerModuleScript>();
    }

    private void Start()
    {
        InitStateMachine();
    }

    private void Update()
    {
        //CalculateState();
        CalculateTargetProperties();
    }

    void CalculateTargetProperties()
    {
        enemiesInSight.Clear();//Clear enemy list
        //Add enemies to enemy list from obstacle check script
        if (obsCheckScript.isObstaclesInRange)//If obstacles in range
        {
            foreach (var item in obsCheckScript.obstaclesInRange)
            {
                if (CheckIfObsIsAnActiveEnemy(item))//If item is an enemy add to enemy list
                {
                    enemiesInSight.Add(item);
                }
            }
        }

        //Check with previous enemies
        //Place common enemy in 0th position
        if (prev0thEnemy == null) return;

        int index = enemiesInSight.IndexOf(prev0thEnemy);

        if (index != -1)//If 0th enemy already exists in array
        {
            var gm = enemiesInSight[0];
            enemiesInSight[0] = enemiesInSight[index];//Swap with current
            enemiesInSight[index] = gm;
        }
        prev0thEnemy = enemiesInSight[0];
    }

    /*
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
            else //If it is an artilery
            {
                //return CheckIfAnArtilery();
            
            }
        }
        return false;
    }

    /*
    bool CheckIfAnArtilery(GameObject item)
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
        return false;
    }
    */

    bool CheckIfObsIsAnActiveEnemy(GameObject item)
    {
        if (item)
        {
            var unitC = item.GetComponent<UnitComponent>();
            if (unitC && item.GetComponent<HealthScript>().currentHP > 0)//If it is an unit with health>0
            {
                if (unitC.unitType == UnitType.TANK || unitC.unitType == UnitType.ARTILERY)//If it is an tank or artilery
                {
                    //If same team or other and alive
                    return unitC.teamID != unitComp.teamID;//if not on same team
                }
            }
        }
        return false;
    }

    void InitStateMachine()
    {
       // stateMachine = new TankAIStateMachine2(this);

        //stateMachine.Initialize("APPR_BASE");
        stateMachine.Initialize("APPR_BASE");

    }

    void OnGameOver()
    {
        stateMachine.ChangeState("GAME_OVR");
    }

}
