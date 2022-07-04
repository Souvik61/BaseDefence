using System.Collections;
using UnityEngine;

public class HPDisplayModuleScript : MonoBehaviour
{
    public HealthScript hpScript;
    public ProgressBarScript healthBar;

    [Range(1,10)]
    public float hpShowDuration;

    bool hBarActive;

    float timer;

    private void OnEnable()
    {
        hpScript.OnHealthIncrease += OnHealthChanged;
        hpScript.OnHealthDecrease += OnHealthChanged;
    }

    private void OnDisable()
    {
        hpScript.OnHealthIncrease -= OnHealthChanged;
        hpScript.OnHealthDecrease -= OnHealthChanged;
    }

    private void Awake()
    {
        timer = hpShowDuration;
    }

    void OnHealthChanged()
    {
        if (!hBarActive)
        {
            StartCoroutine(nameof(HealthbarShowRoutine));
        }
        else
            timer = hpShowDuration;
    }

    IEnumerator HealthbarShowRoutine1()
    {
        healthBar.barVisible = true;
        yield return new WaitForSeconds(2.5f);
        healthBar.barVisible = false;
    }

    IEnumerator HealthbarShowRoutine()
    {
        hBarActive = true;
        healthBar.barVisible = true;
        while (timer > 0)
        {
            healthBar.barProgress = (float)hpScript.currentHP / hpScript.maxHP;

            timer -= Time.deltaTime;
            yield return null;
        }
        timer = hpShowDuration;
        healthBar.barVisible = false;
        hBarActive = false;
    }
}
