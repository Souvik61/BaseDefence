using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
[RequireComponent(typeof(HealthScript))]
public class ArtileryScript : MonoBehaviour
{
    public ArtileryPropertiesSO artileryProperties;
    public float projectileSpeed;
    [SerializeField]
    Transform firePoint;
    [SerializeField]
    ProgressBarScript healthBar;//Health bar
    [SerializeField]
    FOVObsCheckScript obsCheckScript;
    [SerializeField]
    CommonAssetSO commonAsset;
    AudioSource audioSrc;
    [SerializeField]
    SpriteRenderer[] spriteRenderers;//Used to change to broken textures when tank is destroyed



    public string currentStateName;

    ArtileryStateMachine stateMachine;
    public List<TankScript> enemiesInSight;
    HealthScript healthScript;
    BoxCollider2D selfCollider;

    bool isShooting = false;
    bool isDestroyed;

    private void OnEnable()
    {
        healthScript.OnHealthDepleted += OnArtDestroyed;
    }

    private void OnDisable()
    {
        healthScript.OnHealthDepleted -= OnArtDestroyed;
    }

    private void Awake()
    {
        isDestroyed = false;
        audioSrc = GetComponent<AudioSource>();
        healthScript = GetComponent<HealthScript>();
        selfCollider = GetComponent<BoxCollider2D>();
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
        stateMachine = new ArtileryStateMachine(this);

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

    public void Shoot()
    {
        if (!isShooting && !isDestroyed)
        { StartCoroutine(nameof(ShootRoutine)); }
    }

    public HealthScript GetHealthScript()
    {
        return healthScript;  
    }

    void OnTakeDamage(Vector2 collPoint)
    {
        healthScript.Decrement(25);
        
        //Decrease HP bar
        if (healthBar != null)
        {
            healthBar.barProgress = healthScript.currentHP / (float)healthScript.maxHP;
            if (!healthBar.barVisible)
            { StartCoroutine(nameof(HealthbarShowRoutine)); }
        }
        
        //Add hit explosion
        GameObject gm = Instantiate(commonAsset.TankHitPrefab, collPoint, Quaternion.identity);
        Destroy(gm, 3);
    }

    void OnArtDestroyed()
    {
        if (!isDestroyed)
        {
            Instantiate(commonAsset.SmokePrefab, transform.position, Quaternion.identity, transform);
            selfCollider.enabled = false;
            isDestroyed = true;

            //Set broken textures
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].sprite = artileryProperties.destroyedSpriteArray[i];
            }

            StartCoroutine(nameof(DissolveRoutine));
        }
    }

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
        yield return new WaitForSeconds(artileryProperties.shootDelay + Random.Range(-1f, 1f));

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

    IEnumerator HealthbarShowRoutine()
    {
        healthBar.barVisible = true;
        yield return new WaitForSeconds(2.5f);
        healthBar.barVisible = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("tag_projectile"))
        {
            Destroy(collision.gameObject);//Destroy the projectile
            OnTakeDamage(collision.transform.position);
        }
    }

}
*/
