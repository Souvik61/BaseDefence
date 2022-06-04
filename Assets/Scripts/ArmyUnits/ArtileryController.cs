﻿using System.Collections;
using UnityEngine;

public class ArtileryController : MonoBehaviour
{
    public ArtileryPropertiesSO selfProperties;

    HealthScript healthScript;
    [SerializeField]
    ProgressBarScript healthBar;
    Collider2D selfCollider;
    [SerializeField]
    Transform firePoint;
    [SerializeField]
    protected CommonAssetSO commonAsset;
    [SerializeField]
    protected AudioSource audioSrc;

    public float projectileSpeed;
    public float muzzleRotateSpeed;

    //private
    bool isDestroyed;
    protected bool isShooting;
    public Transform muzzleTransform;


    private void OnEnable()
    {
        healthScript.OnHealthDepleted += OnHealthZero;
    }

    private void OnDisable()
    {
        healthScript.OnHealthDepleted -= OnHealthZero;
    }

    private void Awake()
    {
        muzzleTransform = transform.Find("artbody_rotate");
        healthScript = GetComponent<HealthScript>();
        selfCollider = GetComponent<Collider2D>();
    }

    //------------------
    //Utilities
    //------------------

    /// <summary>
    /// Rotate the muzzle
    /// </summary>
    /// <param name="turn"></param>
    public void MuzzleRotate(int turn)
    {
        if (!isDestroyed)
        {
            muzzleTransform.Rotate(0, 0, -turn * Time.deltaTime * muzzleRotateSpeed, Space.Self);
            //float newAngle = Vector2.SignedAngle(tankBodyTransform.up, muzzleTransform.up);
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

    //-----------
    //Events
    //-----------

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("tag_projectile"))
        {
            Destroy(collision.gameObject);//Destroy the projectile
            OnTakeDamage(collision.transform.position);
        }
    }

    void OnTakeDamage(Vector2 collPoint)
    {
        healthScript.Decrement(25);
        /*
        //Decrease HP bar
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

            SetBrOKenTextures();

        }

        //StartCoroutine(nameof(DissolveRoutine));
    }

    //-------------
    //Helpers
    //-------------

    public virtual IEnumerator ShootRoutine()
    {
        isShooting = true;

        //Instantiate projectile 
        GameObject proj = Instantiate(commonAsset.ProjectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<BulletScript>().damageAmmount = (int)selfProperties.shootDamage;
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
        yield return new WaitForSeconds(selfProperties.shootDelay + Random.Range(-1f, 1f));

        isShooting = false;
    }

    void SetBrOKenTextures()
    {
        //Set broken textures
        //for (int i = 0; i < spriteRenderers.Length; i++)
        //{
        //    spriteRenderers[i].sprite = artileryProperties.destroyedSpriteArray[i];
        //}
    }

}