﻿using System.Collections.Generic;
using UnityEngine;

public class CommandCenterScript : MonoBehaviour
{
    public int baseID;

    public ArmyBaseScript_pt1 targetBase;
    public List<GameObject> tankSpawnPoints;
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
            TakeDamage(collision.transform.position, collision.GetComponent<BulletScript>().damageAmmount);
        }
    }

    public void DeployTank(int pos)
    {
        //Deploy tank at position pos
        //Spawn tank
        Transform tank = Instantiate<Transform>(commonAsset.TANK_1.transform);
        tank.transform.position = tankSpawnPoints[pos].transform.position;

        tank.transform.rotation = Quaternion.LookRotation(Vector3.forward, tankSpawnPoints[pos].transform.right);

        tank.tag = "tag_opponent1";
        tank.GetComponent<TankAIScript3>().endPoint = targetBase.enemyLandingZones[pos];
        tank.GetComponent<TankAIScript3>().targetBase = this.targetBase;
        tank.GetComponent<UnitComponent>().teamID = 0;

    }

    void TakeDamage(Vector2 collPoint,int dAmmount)
    {

        healthScript.Decrement((uint)dAmmount);
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
