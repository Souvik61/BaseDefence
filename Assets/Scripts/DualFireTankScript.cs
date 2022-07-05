using System.Collections;
using UnityEngine;

public class DualFireTankScript : NewTankScript
{
    [SerializeField]
    Transform[] firePoints;

    public override IEnumerator ShootRoutine()
    {
        isShooting = true;

        for (int i = 0; i < 2; i++)
        {
            //Instantiate projectile 
            GameObject proj = Instantiate(commonAsset.ProjectilePrefab, firePoints[i].position, Quaternion.identity);
            proj.GetComponent<BulletScript>().damageAmmount = (int)(10 * tankProperty.sDamage);
            proj.tag = "tag_projectile" + unitC.teamID;
            proj.GetComponent<Rigidbody2D>().velocity = firePoints[i].up * projectileSpeed;
            Destroy(proj, 3.0f);//Destroy projectile after 3 seconds

            //Instantiate muzzle flash
            Quaternion rot = firePoints[i].rotation * Quaternion.Euler(new Vector3(0, 0, 90));
            GameObject mzlFlash = Instantiate(commonAsset.MuzzleFlashPrefab, firePoints[i].position, rot);
            float size = Random.Range(0.6f, 0.9f);
            mzlFlash.transform.localScale = new Vector2(size, size);
            Destroy(mzlFlash, 0.05f);

            //play shoot audio
            audioSrc.PlayOneShot(tankProperty.GenerateRandomSfx());
            //wait before shooting again
            yield return new WaitForSeconds(0.2f);

        }
        yield return new WaitForSeconds(tankProperty.shootDelay + Random.Range(-1f, 1f));

        isShooting = false;

    }
}
