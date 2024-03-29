﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class OneParamEvent : UnityEvent<string> { }

public class CommandCenterScript : MonoBehaviour
{
    public int baseID;

    public ArmyBaseScript_pt1 targetBase;
    public List<GameObject> tankSpawnPoints;
    public List<GameObject> artSpawnPoints;
    [SerializeField]
    HealthScript healthScript;
    [SerializeField]
    CommonAssetSO commonAsset;

    bool isDestroyed;
    GameObject[] currDeployedArtis = new GameObject[3];
    public List<GameObject> currDeployedTanks = new List<GameObject>();

    [SerializeField] OneParamEvent OnATankDestroyed;

    BrokenTextureScript textureScript;

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
        textureScript = GetComponentInChildren<BrokenTextureScript>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(AutoRemoveDeadTroops), 0, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("tag_projectile"))
        {
            Destroy(collision.gameObject);
            TakeDamage(collision.transform.position, collision.GetComponent<BulletScript>().damageAmmount);
        }
    }

    //-------------------------
    //Deploy functions
    //-------------------------

    public void DeployTank(int pos, int tankIndex)//Watch out index starts from 1
    {
        //Deploy tank at position pos
        //Spawn tank
        Transform tank = Instantiate<Transform>(IndexToTankTransform(tankIndex));
        tank.transform.position = tankSpawnPoints[pos].transform.position;

        tank.transform.rotation = Quaternion.LookRotation(Vector3.forward, tankSpawnPoints[pos].transform.right);

        tank.tag = "tag_opponent1";
        tank.GetComponent<UnitComponent>().teamID = GetComponent<UnitComponent>().teamID;//Set spawned tank unit id to self
        tank.GetComponent<TankAIScript3>().endPoint = targetBase.enemyLandingZones[pos];
        tank.GetComponent<TankAIScript3>().targetBase = this.targetBase;
        currDeployedTanks.Add(tank.gameObject);
    }

    //Deprecated
    public void DeployArtillery(int pos)
    {
        if (currDeployedArtis[pos] != null)
            return;

        //Deploy artillery at position pos
        //Spawn artillery
        Transform art = Instantiate<Transform>(commonAsset.PREFAB_ART_1.transform);
        art.transform.position = artSpawnPoints[pos].transform.position;

        art.transform.rotation = Quaternion.LookRotation(Vector3.forward, artSpawnPoints[pos].transform.right);
        art.tag = "tag_opponent1";
        art.GetComponent<UnitComponent>().teamID = GetComponent<UnitComponent>().teamID;
        currDeployedArtis[pos] = art.gameObject;
    }

    public void DeployArtillery(int pos, int artIndex)
    {
        if (currDeployedArtis[pos] != null)
            return;

        //Deploy artillery at position pos
        //Spawn artillery
        Transform art = Instantiate<Transform>(IndexToArtTransform(artIndex));
        art.transform.position = artSpawnPoints[pos].transform.position;

        art.transform.rotation = Quaternion.LookRotation(Vector3.forward, artSpawnPoints[pos].transform.right);
        art.tag = "tag_opponent1";
        art.GetComponent<UnitComponent>().teamID = GetComponent<UnitComponent>().teamID;
        currDeployedArtis[pos] = art.gameObject;
    }

    public GameObject GetArtilleryAt(int pos)
    {
        return currDeployedArtis[pos];
    }

    public void RemoveArtilleryAt(int pos)
    {
        if (currDeployedArtis[pos] == null)
            return;
        Destroy(currDeployedArtis[pos]);
        currDeployedArtis[pos] = null;
    }

    public bool IsArtilleryAreaAvailable(int pos)
    {
        return currDeployedArtis[pos] == null;
    }

    public bool IsTankSpwnAreaAvailable(int index)
    {
        Vector2 size = tankSpawnPoints[index].GetComponent<SpriteRenderer>().bounds.size;
        var res = Physics2D.OverlapBox(tankSpawnPoints[index].transform.position, size, 0, LayerMask.GetMask("Tank"));

        Bounds bounds = new Bounds(tankSpawnPoints[index].transform.position, size);

        HelperScript.DrawBoundDebug(bounds, Color.red);
        return res == null;
    }

    void TakeDamage(Vector2 collPoint, int dAmmount)
    {
        healthScript.Decrement((uint)dAmmount);
    
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
            //Instantiate(commonAsset.RedCross, transform.position, Quaternion.identity, transform);
            isDestroyed = true;
            if (textureScript)
                textureScript.SetBrokenFunc(true);//added
            
            AllEventsScript.OnBaseDestroyed?.Invoke(baseID);
        }
    }

    Transform IndexToTankTransform(int index)//Watch out! index starts from 1
    {
        switch (index - 1)
        {
            case 0:
                return commonAsset.TANK_1.transform;
            case 1:
                return commonAsset.TANK_2.transform;
            case 2:
                return commonAsset.TANK_3.transform;
            case 3:
                return commonAsset.TANK_4.transform;
            case 4:
                return commonAsset.TANK_5.transform;
            default:
                return null;
        }
    }

    Transform IndexToArtTransform(int index)
    {
        switch (index)
        {
            case 0:
                return commonAsset.PREFAB_ART_1.transform;
            case 1:
                return commonAsset.PREFAB_ART_2.transform;
            case 2:
                return commonAsset.PREFAB_ART_3.transform;
            case 3:
                return commonAsset.PREFAB_ART_4.transform;
            default:
                return null;
        }
    }

    void OnGameOver()
    {
        //stateMachine.ChangeState("GAME_OVR");
    }

    void AutoRemoveDeadTroops()
    {
        for (int i = currDeployedTanks.Count - 1; i >= 0; i--)
        {
            if (currDeployedTanks[i] == null || currDeployedTanks[i].GetComponent<HealthScript>().currentHP == 0)//If element null or dead
            {
                var a = currDeployedTanks[i];
                currDeployedTanks.RemoveAt(i);
                OnATankDestroyed?.Invoke(a.GetComponent<UnitComponent>().unitName);
            }
        }
    }
}
