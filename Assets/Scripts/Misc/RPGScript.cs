using UnityEngine;

public class RPGScript : BulletScript
{
    public Transform trail;
    public Rigidbody2D rb;
    public CommonAssetSO commAsset;

    private void Update()
    {
        trail.rotation = Quaternion.LookRotation(-rb.velocity, Vector3.up);
    }

    private void OnDisable()
    {
        if (!gameObject.scene.isLoaded) return;
        // Instantiate objects here
        GameObject gm = new GameObject("test_autodelete");
        var audSrc = gm.AddComponent<AudioSource>();
        audSrc.playOnAwake = false;
        audSrc.PlayOneShot(commAsset.sfx_tankfire_small);
        Destroy(gm, 3);
    }

    private void OnDestroy()
    {
        
    }
}
