using System.Collections;
using UnityEngine;

public class ArtilleryDoubleFire : ArtileryControlBase
{
    public Transform[] firePoints;

    public override IEnumerator ShootRoutine()
    {
        isShooting = true;

        for (int i = 0; i < 2; i++)
        {
            //Instantiate projectile 
            GameObject proj = Instantiate(commonAsset.ProjectilePrefab, firePoints[i].position, Quaternion.identity);
            proj.GetComponent<BulletScript>().damageAmmount = (int)(10 * selfProperties.sDamage);
            proj.GetComponent<Rigidbody2D>().velocity = firePoints[i].up * projectileSpeed;
            Destroy(proj, 3.0f);//Destroy projectile after 3 seconds

            //Instantiate muzzle flash
            Quaternion rot = firePoints[i].rotation * Quaternion.Euler(new Vector3(0, 0, 90));
            GameObject mzlFlash = Instantiate(commonAsset.MuzzleFlashPrefab, firePoints[i].position, rot);
            float size = Random.Range(0.6f, 0.9f);
            mzlFlash.transform.localScale = new Vector2(size, size);
            Destroy(mzlFlash, 0.05f);
            //play shoot audio
            audioSrc.PlayOneShot(commonAsset.GRArtt2ShootSfx());
            yield return new WaitForSeconds(0.1f);
        }

        //wait before shooting again
        yield return new WaitForSeconds(selfProperties.shootDelay + Random.Range(-1f, 1f));

        isShooting = false;
    }

}
