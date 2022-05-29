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

    public bool isDestroyed;
    
    cmplx_statemachine.WatchTowerStateMachine stateMachine;
    public List<GameObject> enemiesInSight;

    public string currentStateName;

    bool isShooting;

    //Unity methods

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
        healthScript = GetComponent<HealthScript>();
        stateMachine = GetComponent<cmplx_statemachine.WatchTowerStateMachine>();
        obsCheckScript = GetComponentInChildren<FOVObsCheckScript>();   
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
        if (collision.CompareTag("tag_projectile"))
        {
            Destroy(collision.gameObject);
            TakeDamage(collision.transform.position);
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
            else { }//If it is an artilery
            /*
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
                */
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

    void TakeDamage(Vector2 collPoint)
    {

        healthScript.Decrement(25);
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

    void OnGameOver()
    {
        //stateMachine.ChangeState("GAME_OVR");
    }



}


