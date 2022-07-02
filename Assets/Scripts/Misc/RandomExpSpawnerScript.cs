using UnityEngine;

public class RandomExpSpawnerScript : MonoBehaviour
{
    public Transform lowerLeftCorner;
    public Transform upperRightCorner;
    public float delay;

    public CommonAssetSO commAsset;
    public GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnExplosion), delay, delay);
    }

    void SpawnExplosion()
    {
        Vector2 a = new Vector2();
        a.x = Random.Range(lowerLeftCorner.position.x, upperRightCorner.position.x);
        a.y = Random.Range(lowerLeftCorner.position.y, upperRightCorner.position.y);
        var b = Instantiate(explosionPrefab);
        b.transform.position = a;
    
    }
}
