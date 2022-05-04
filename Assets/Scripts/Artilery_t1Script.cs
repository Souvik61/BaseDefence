using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artilery_t1Script : MonoBehaviour
{
    public float projectileSpeed;
    public float shootDelay;
    public float muzzRotateSpeed;
    [SerializeField]
    Transform[] firePoints;
    public Transform muzzleTransform, bodyTransform;
    [SerializeField]
    FOVObsCheckScript obsCheckScript;
    [SerializeField]
    CommonAssetSO commonAsset;
    AudioSource audioSrc;

    public string currentStateName;

    Artilery_t1StateMachine stateMachine;
    public List<TankScript> enemiesInSight;
    HealthScript healthScript;
    CircleCollider2D selfCollider;

    bool isShooting = false;
    bool isDestroyed;

    private void OnEnable()
    {
        healthScript.OnHealthDepleted += OnArtDestroyed;
        AllEventsScript.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        healthScript.OnHealthDepleted -= OnArtDestroyed;
        AllEventsScript.OnGameOver -= OnGameOver;
    }

    private void Awake()
    {
        isDestroyed = false;
        audioSrc = GetComponent<AudioSource>();
        healthScript = GetComponent<HealthScript>();
        selfCollider = GetComponent<CircleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitStateMachine();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForEnemies();
        stateMachine.Update();
        currentStateName = stateMachine.GetCurrentStateName();
    }

    void InitStateMachine()
    {
        stateMachine = new Artilery_t1StateMachine(this);

        stateMachine.Initialize("IDLE");

        stateMachine.Start();
    }

    void CheckForEnemies()
    {
        enemiesInSight.Clear();//Clear enemy list
        //Add enemies to enemy list from obstacle check script
        if (obsCheckScript.isObstaclesInRange)//If obstacles in range
        {
            foreach (var item in obsCheckScript.obstaclesInRange)
            {
                if (item != null && item.layer == LayerMask.NameToLayer("Tank"))//If obstacles are enemy tanks
                {
                    if (item.GetComponent<TankScript>().GetHealthScript().currentHP > 0)
                    {
                        if (!item.GetComponent<TankScript>().CompareTag(tag))//If target is not in our team
                        { enemiesInSight.Add(item.GetComponent<TankScript>()); }
                    }
                }
            }
        }

    }

    /// <summary>
    /// turn == 1 clockwise, turn == -1 anticlockwise
    /// </summary>
    public void MuzzleRotate(int turn)
    {
        if (!isDestroyed)
        {
            muzzleTransform.Rotate(0, 0, -turn * Time.deltaTime * muzzRotateSpeed, Space.Self);
            float newAngle = Vector2.SignedAngle(bodyTransform.up, muzzleTransform.up);
            if (newAngle > 90)
            {
                muzzleTransform.localRotation = Quaternion.Euler(0, 0, 90);
            }
            if (newAngle < -90)
            {
                muzzleTransform.localRotation = Quaternion.Euler(0, 0, -90);
            }
        }
    }

    public void Shoot()
    {
        if (!isShooting && !isDestroyed)
        { StartCoroutine(nameof(ShootRoutine)); }
    }

    public HealthScript GetHealthScript()
    {
        return healthScript;
    }

    void TakeDamage()
    {
        healthScript.Decrement(25);
    }

    void OnArtDestroyed()
    {
        if (!isDestroyed)
        {
            Instantiate(commonAsset.SmokePrefab, transform.position, Quaternion.identity, transform);
            selfCollider.enabled = false;
            isDestroyed = true;
            StartCoroutine(nameof(DissolveRoutine));
        }
    }

    void OnGameOver()
    {
        stateMachine.ChangeState("GAME_OVR");
    }

    /*
    IEnumerator ShootRoutine()
    {
        isShooting = true;

        //Instantiate projectile 
        GameObject proj = Instantiate(commonAsset.ProjectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().velocity = firePoint.up * projectileSpeed;
        Destroy(proj, 3.0f);//Destroy projectile after 3 seconds

        //Instantiate muzzle flash
        Quaternion rot = firePoint.rotation * Quaternion.Euler(new Vector3(0, 0, 90));
        GameObject mzlFlash = Instantiate(commonAsset.MuzzleFlashPrefab, firePoint.position, rot);
        float size = Random.Range(0.6f, 0.9f);
        mzlFlash.transform.localScale = new Vector2(size, size);
        Destroy(mzlFlash, 0.05f);

        //play shoot audio
        audioSrc.Play();
        //wait before shooting again
        yield return new WaitForSeconds(shootDelay + Random.Range(-1f, 1f));

        isShooting = false;

    }
    */

    IEnumerator ShootRoutine()
    {
        isShooting = true;

        for (int i = 0; i < 2; i++)//for each muzzle
        {
            //Instantiate projectile 
            GameObject proj = Instantiate(commonAsset.ProjectilePrefab, firePoints[i].position, Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().velocity = firePoints[i].up * projectileSpeed;
            Destroy(proj, 3.0f);//Destroy projectile after 3 seconds

            //Instantiate muzzle flash
            Quaternion rot = firePoints[i].rotation * Quaternion.Euler(new Vector3(0, 0, 90));
            GameObject mzlFlash = Instantiate(commonAsset.MuzzleFlashPrefab, firePoints[i].position, rot);
            float size = Random.Range(0.6f, 0.9f);
            mzlFlash.transform.localScale = new Vector2(size, size);
            Destroy(mzlFlash, 0.05f);

            //play shoot audio
            audioSrc.Play();
        }

        //wait for second round
        yield return new WaitForSeconds(0.5f);

        for (int i = 2; i < 4; i++)//for each muzzle
        {
            //Instantiate projectile 
            GameObject proj = Instantiate(commonAsset.ProjectilePrefab, firePoints[i].position, Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().velocity = firePoints[i].up * projectileSpeed;
            Destroy(proj, 3.0f);//Destroy projectile after 3 seconds

            //Instantiate muzzle flash
            Quaternion rot = firePoints[i].rotation * Quaternion.Euler(new Vector3(0, 0, 90));
            GameObject mzlFlash = Instantiate(commonAsset.MuzzleFlashPrefab, firePoints[i].position, rot);
            float size = Random.Range(0.6f, 0.9f);
            mzlFlash.transform.localScale = new Vector2(size, size);
            Destroy(mzlFlash, 0.05f);

            //play shoot audio
            audioSrc.Play();
        }

        yield return new WaitForSeconds(shootDelay + Random.Range(-1f, 1f));

        isShooting = false;

    }

    IEnumerator DissolveRoutine()
    {
        yield return new WaitForSeconds(5);
        float scale = transform.localScale.x;
        while (scale > 0.001f)
        {
            scale *= 0.93f;
            transform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }
        Destroy(gameObject);//Self destroy 0 secs.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("tag_projectile"))
        {
            Destroy(collision.gameObject);//Destroy the projectile
            TakeDamage();
        }
    }
}
