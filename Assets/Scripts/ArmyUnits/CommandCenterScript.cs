using UnityEngine;

public class CommandCenterScript : MonoBehaviour
{
    public int baseID;
    [SerializeField]
    HealthScript healthScript;
    [SerializeField]
    CommonAssetSO commonAsset;

    bool isDestroyed;

    private void OnEnable()
    {
        healthScript.OnHealthDepleted += this.OnHealthZero;
    }

    private void OnDisable()
    {
        healthScript.OnHealthDepleted -= this.OnHealthZero;
    }

    private void Awake()
    {
        healthScript = GetComponent<HealthScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("tag_projectile"))
        {
            Destroy(collision.gameObject);
            TakeDamage(collision.transform.position);
        }
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

    void OnHealthZero()
    {
        //Do someting
        Debug.Log("Command center destroyed");
        if (!isDestroyed)
        {
            Instantiate(commonAsset.RedCross, transform.position, Quaternion.identity, transform);
            isDestroyed = true;
            AllEventsScript.OnBaseDestroyed?.Invoke(baseID);
        }
    }

    void OnGameOver()
    {
        //stateMachine.ChangeState("GAME_OVR");
    }
}
