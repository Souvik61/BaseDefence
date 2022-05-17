using UnityEngine;

public class ArmyBaseCollisionScript : MonoBehaviour
{
    [SerializeField]
    ArmyBaseScript armyScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("tag_projectile"))
        {
            Destroy(collision.gameObject);
            armyScript.TakeDamage();
        }
    }
}
