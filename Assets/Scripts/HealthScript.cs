using UnityEngine;

///<summary>
/// Represents the current vital statistics of some game entity.
/// </summary>
public class HealthScript : MonoBehaviour
{
    public delegate void AFunc();

    /// <summary>
    /// The maximum hit points for the entity.
    /// </summary>
    public int maxHP = 100;

    /// <summary>
    /// Indicates if the entity should be considered 'alive'.
    /// </summary>
    public bool IsAlive => currentHP > 0;

    public int currentHP;
    public bool godMode;

    public AFunc OnHealthDepleted;

    /// <summary>
    /// Increment the HP of the entity.
    /// </summary>
    public void Increment(uint value)
    {
        currentHP = Mathf.Clamp(currentHP + (int)value, 0, maxHP);
    }

    /// <summary>
    /// Decrement the HP of the entity. Will trigger a HealthIsZero event when
    /// current HP reaches 0.
    /// </summary>
    public void Decrement(uint value)
    {
        currentHP = Mathf.Clamp(currentHP - (int)value, 0, maxHP);

        if (godMode && currentHP == 0) { currentHP = 1; }

        if (currentHP == 0)
        {
            OnHealthDepleted?.Invoke(); 
        }
    }

    /// <summary>
    /// Decrement the HP of the entitiy until HP reaches 0.
    /// </summary>
    public void Die()
    {
        while (currentHP > 0) Decrement(1);
    }

    void Awake()
    {
        currentHP = maxHP;
    }
}

