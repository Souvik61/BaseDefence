using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(HealthScript))]
public class WatchTowerAIScript : MonoBehaviour
{
    public float shootDelay;
    [SerializeField]
    HealthScript healthScript;
    [SerializeField]
    Transform firePoint;
    [SerializeField]
    CommonAssetSO commonAsset;
    [SerializeField]
    FOVObsCheckScript obsCheckScript;
    [SerializeField]
    ProgressBarScript healthBar;
    [SerializeField]
    Collider2D selfCollider;
    UnitComponent unitC;

    public bool isDestroyed;
    
    cmplx_statemachine.WatchTowerStateMachine stateMachine;
    public List<GameObject> enemiesInSight;

    bool isShooting;

    GameObject prev0thEnemy;

    //Unity methods

    private void OnEnable()
    {
        healthScript.OnHealthDepleted += OnHealthZero;
        AllEventsScript.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        healthScript.OnHealthDepleted -= OnHealthZero;
        AllEventsScript.OnGameOver -= OnGameOver;
    }

    private void Awake()
    {
        healthScript = GetComponent<HealthScript>();
        unitC = GetComponent<UnitComponent>();
        stateMachine = GetComponent<cmplx_statemachine.WatchTowerStateMachine>();
        obsCheckScript = GetComponentInChildren<FOVObsCheckScript>();
        selfCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        InitStateMachine();
    }

    private void Update()
    {
        CalculateTargetProperties();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("tag_projectile"))
        {
            Destroy(collision.gameObject);

            if (!collision.CompareTag("tag_projectile" + unitC.teamID))
                TakeDamage(collision.transform.position, collision.GetComponent<BulletScript>().damageAmmount);
        }
    }


    //Other methods

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
                    return !unitC.CompareTag(tag);//if not on same team
                }
            }
        }
        return false;
    }

    void InitStateMachine()
    {
        stateMachine.Initialize("IDLE");
    }

    public void Shoot(Transform target)
    {
        if (!isShooting && !isDestroyed)
        { StartCoroutine(nameof(ShootRoutine), target); }
    }

    protected virtual IEnumerator ShootRoutine(Transform target)
    {
        Vector2 dir = (target.transform.position - transform.position).normalized;

        isShooting = true;

        //Instantiate projectile 
        GameObject proj = Instantiate(commonAsset.ProjectilePrefab, firePoint.position, Quaternion.identity);
        proj.layer = LayerMask.NameToLayer("~WatchTower");
        proj.GetComponent<Rigidbody2D>().velocity = dir * 20;
        Destroy(proj, 3.0f);//Destroy projectile after 3 seconds

        //play shoot audio
        //audioSrc.Play();
        //wait before shooting again
        yield return new WaitForSeconds(shootDelay + Random.Range(-1f, 1f));

        isShooting = false;

    }

    void TakeDamage(Vector2 collPoint,int damAmmount)
    {
        healthScript.Decrement((uint)damAmmount);
        /*
        //Decrease HP Bar
        if (healthBar != null)
        {
            healthBar.barProgress = healthScript.currentHP / (float)healthScript.maxHP;
            if (!healthBar.barVisible)
            { StartCoroutine(nameof(HealthbarShowRoutine)); }
        }
        */
        //Add hit explosion
        GameObject gm = Instantiate(commonAsset.TankHitPrefab, collPoint, Quaternion.identity);
        Destroy(gm, 3);
    }

    void OnHealthZero()
    {
        if (!isDestroyed)
        {
            Instantiate(commonAsset.SmokePrefab, transform.position, Quaternion.identity, transform);
            selfCollider.enabled = false;
            isDestroyed = true; 
            


        }
    }

    void OnGameOver()
    {
        //stateMachine.ChangeState("GAME_OVR");
    }

}


