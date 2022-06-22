using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankScript : MonoBehaviour,IDamageable
{
    public TankPropertiesSO tankProperty;
    public float projectileSpeed;
    public bool isDestroyed;

    public Transform tankBodyTransform;
    public Transform muzzleTransform;
    [SerializeField]
    Transform firePoint;
    [SerializeField]
    protected ProgressBarScript healthBar;//Health bar
    [SerializeField]
    protected CommonAssetSO commonAsset;
    protected AudioSource audioSrc;
    [SerializeField]
    protected SpriteRenderer[] spriteRenderers;//Used to change to broken textures when tank is destroyed;

    //private
    protected Vector2 nextPosition;
    protected float nextRotation;
    protected HealthScript healthScript;
    protected bool isShooting;
    protected Collider2D selfCollider;
    protected Rigidbody2D rBody;
    protected bool isBeingAttacked;
    protected UnitComponent unitC;
    BrokenTextureScript brokenTexCtrl;

    private void OnEnable()
    {
        healthScript.OnHealthDepleted += OnTankDestroyed;
    }

    private void OnDisable()
    {
        healthScript.OnHealthDepleted -= OnTankDestroyed;
    }

    protected virtual void Awake()
    {
        isBeingAttacked = false;
        tankBodyTransform = transform.Find("tank_body");
        muzzleTransform = transform.Find("tank_body/muzzle");
        unitC = GetComponent<UnitComponent>();
        healthScript = GetComponent<HealthScript>();
        audioSrc = GetComponent<AudioSource>();
        selfCollider = GetComponent<BoxCollider2D>();
        rBody = GetComponent<Rigidbody2D>();
        brokenTexCtrl = GetComponentInChildren<BrokenTextureScript>();

        nextPosition = transform.position;
        nextRotation = rBody.rotation;

    }

    protected virtual void Start()
    {
        if (healthBar != null)
            healthBar.barVisible = false;
    }

    protected virtual void FixedUpdate()
    {
        rBody.MovePosition(nextPosition);
        rBody.MoveRotation(nextRotation);
    }

    /// <summary>
    /// forward 1 -> move forward, forward -1 -> movebackward,turn-> Rotate clockwise if -1 and counterclockwise if 1
    /// </summary>
    public virtual void Move(int forward)
    {
        if (!isDestroyed)
        {
            //rBody.MovePosition(transform.position+ (forward * driveSpeed * transform.up));
            nextPosition = transform.position + (forward * 0.01f * tankProperty.driveSpeed * transform.up);
        }
    }

    public virtual void Rotate(int turn)
    {
        if (!isDestroyed)
            nextRotation = rBody.rotation + turn * 0.01f * tankProperty.rotateSpeed;
    }

    /// <summary>
    /// turn == 1 clockwise, turn == -1 anticlockwise
    /// </summary>
    public void MuzzleRotate(int turn)
    {
        if (!isDestroyed)
        {
            muzzleTransform.Rotate(0, 0, -turn * Time.deltaTime * tankProperty.muzzRotateSpeed, Space.Self);
            float newAngle = Vector2.SignedAngle(tankBodyTransform.up, muzzleTransform.up);
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

    /// <summary>
    /// Shoot!
    /// </summary>
    public virtual void Shoot()
    {
        if (!isShooting && !isDestroyed)
        { StartCoroutine(nameof(ShootRoutine)); }
    }

    public HealthScript GetHealthScript()
    { return healthScript; }

    protected void OnTakeDamage(Vector2 collisionPoint,int dAmount)
    {
        healthScript.Decrement((uint)dAmount);
        //Decrease HP Bar
        if (healthBar != null)
        {
            healthBar.barProgress = healthScript.currentHP / (float)healthScript.maxHP;
            if (!healthBar.barVisible)
            { StartCoroutine(nameof(HealthbarShowRoutine)); }
        }

        //Add hit explosion
        GameObject gm = Instantiate(commonAsset.TankHitPrefab, collisionPoint, Quaternion.identity);
        Destroy(gm, 3);
    }

    void OnTankDestroyed()
    {
        if (!isDestroyed)
        {
            Instantiate(commonAsset.SmokePrefab, transform.position, Quaternion.identity, transform);
            selfCollider.enabled = false;
            isDestroyed = true;

            brokenTexCtrl.SetBroken = true;//Set broken textures

            StartCoroutine(nameof(DissolveRoutine));
        }
    }

    IEnumerator HealthbarShowRoutine()
    {
        healthBar.barVisible = true;
        yield return new WaitForSeconds(2.5f);
        healthBar.barVisible = false;
    }

    public virtual IEnumerator ShootRoutine()
    {
        isShooting = true;

        //Instantiate projectile 
        GameObject proj = Instantiate(commonAsset.ProjectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<BulletScript>().damageAmmount = (int)tankProperty.shootDamage;
        proj.tag = "tag_projectile" + unitC.teamID;
        proj.GetComponent<Rigidbody2D>().velocity = firePoint.up * projectileSpeed;
        Destroy(proj, 3.0f);//Destroy projectile after 3 seconds

        //Instantiate muzzle flash
        Quaternion rot = firePoint.rotation * Quaternion.Euler(new Vector3(0, 0, 90));
        GameObject mzlFlash = Instantiate(commonAsset.MuzzleFlashPrefab, firePoint.position, rot);
        float size = Random.Range(0.6f, 0.9f);
        mzlFlash.transform.localScale = new Vector2(size, size);
        Destroy(mzlFlash, 0.05f);

        //play shoot audio
        audioSrc.PlayOneShot(tankProperty.GenerateRandomSfx());
        //wait before shooting again
        yield return new WaitForSeconds(tankProperty.shootDelay + Random.Range(-1f, 1f));

        isShooting = false;

    }

    IEnumerator DissolveRoutine()
    {
        yield return new WaitForSeconds(10);
        float scale = transform.localScale.x;
        while (scale > 0.001f)
        {
            scale *= 0.93f;
            transform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }
        Destroy(gameObject);//Self destroy 0 secs.
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("tag_projectile"))
        {
            Destroy(collision.gameObject);//Destroy the projectile
            OnTakeDamage(collision.transform.position, collision.GetComponent<BulletScript>().damageAmmount);
        }
    }

    public void OnTakeDamage()
    {
        throw new System.NotImplementedException();
    }
}


