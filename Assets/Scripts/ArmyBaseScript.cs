using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ArmyBaseScript : MonoBehaviour
{
    public int baseId;
    public string selfTag;

    public ArmyBaseScript targetBase;

    [SerializeField]
    bool troopDeployActive;
    [SerializeField]
    float troopDeployPerSec = 1;

    [SerializeField]
    Transform[] spawnPoints;
    [SerializeField]
    HealthScript selfHealth;

    [SerializeField]
    Transform frontFaceTransform;
    [SerializeField]
    ProgressBarScript progressBar;
    [SerializeField]
    SpriteRenderer armyBaseRenderer;
    [SerializeField]
    Sprite armyBaseDestroyedSp;
    [SerializeField]
    GameObject tankPrefab;
    public CommonAssetSO commAsset;
    [SerializeField]
    List<GameObject> tanksList;

    public bool isDestroyed;

    private void OnEnable()
    {
        selfHealth.OnHealthDepleted += OnHealthZero; 
        
    }

    private void OnDisable()
    {
        selfHealth.OnHealthDepleted -= OnHealthZero;
    }

    private void Awake()
    {
        isDestroyed = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (progressBar)
            progressBar.barProgress = selfHealth.currentHP;

        StartCoroutine(nameof(TroopsDeployRoutine));
    }

    IEnumerator TroopsDeployRoutine()
    {
        while (true)
        {
            if (troopDeployActive && !isDestroyed && IsTargetBaseAlive())
                DeployTroop();
            yield return new WaitForSeconds(1 / troopDeployPerSec);
        }
    }

    private void DeployTroop()
    {
        GameObject tank = Instantiate(tankPrefab);
        tank.transform.position = spawnPoints[Random.Range(0, 2)].position;

        tank.transform.rotation = Quaternion.LookRotation(Vector3.forward, frontFaceTransform.up);

       // tank.GetComponent<NewTankAIScript>().targetBase = this.targetBase;//Give ref. to target base
       // tank.GetComponent<NewTankAIScript>().parentBase = this;

        tank.tag = selfTag;//Set tag to self tag

        tanksList.Add(tank);
    }

    public void TakeDamage()
    {
        selfHealth.Decrement(20);
        // Debug.Log(selfHealth.currentHP);
        progressBar.barProgress = selfHealth.currentHP / (float)selfHealth.maxHP;
        
    }

    protected virtual void OnHealthZero()
    {
        if (!isDestroyed)
        {
            armyBaseRenderer.sprite = armyBaseDestroyedSp;
            Instantiate(commAsset.SmokePrefab, transform.position, Quaternion.identity);

            //Instantiate whiteflag
            Vector3 pos = transform.Find("wf_spawnpoint").position;
            Instantiate(commAsset.WhiteFlagPrefab, pos, Quaternion.identity);

            isDestroyed = true;
            AllEventsScript.OnBaseDestroyed?.Invoke(baseId);
        }
    }

    bool IsTargetBaseAlive()
    {
        return !targetBase.isDestroyed;
    }

}
