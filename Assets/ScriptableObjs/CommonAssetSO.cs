using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CommonAsset", order = 1)]
public class CommonAssetSO : ScriptableObject
{
    
    public GameObject SmokePrefab;
    public GameObject WhiteFlagPrefab;
    public GameObject MuzzleFlashPrefab;
    public GameObject ProjectilePrefab;
    public GameObject ProjectilePrefab_RPG;
    public GameObject TankHitPrefab;

    [Header("Tank Prefabs")]
    //Tank prefabs
    public GameObject TANK_1;
    public GameObject TANK_2;
    public GameObject TANK_3;
    public GameObject TANK_4;
    public GameObject TANK_5;

    [Header("Artillery Prefabs")]
    //Artillery prefabs
    public GameObject PREFAB_ART_1;
    public GameObject PREFAB_ART_2;
    public GameObject PREFAB_ART_3;

    //Miscelleneous
    [Header("Miscellaneous Prefabs")]
    public GameObject RedCross;

    [Header("Audio Clips")]
    //Audio clips
    public AudioClip TankShoot;
    public AudioClip Bgm;
    [Header("Tank Sfx")]
    public AudioClip sfx_tankfire_small;
    [Header("Tank Sfx type1")]
    public AudioClip sfx_tankfire1;
    public AudioClip sfx_tankfire2;
    public AudioClip sfx_tankfire3;
    [Header("Tank Sfx type2")]
    public AudioClip sfx_tankfiret2_1;
    public AudioClip sfx_tankfiret2_2;
    public AudioClip sfx_tankfiret2_3;
    [Header("Tank Sfx type cool")]
    public AudioClip sfx_tankfire_cool1;
    public AudioClip sfx_tankfire_cool2;
    [Header("Artillery Sfx")]
    public AudioClip sfx_artfire1_t1;
    public AudioClip sfx_artfire1_t2;
    public AudioClip sfx_artfire2_t2;
    public AudioClip sfx_artfire3_t2;

    public AudioClip GRTank1ShootSfx()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                return sfx_tankfire1;
            case 1:
                return sfx_tankfire2;
            case 2:
                return sfx_tankfire3;
            default:
                break;
        }
        return null;
    }

    public AudioClip GRTank2ShootSfx()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                return sfx_tankfiret2_1;
            case 1:
                return sfx_tankfiret2_2;
            case 2:
                return sfx_tankfiret2_3;
            default:
                break;
        }
        return null;
    }

    public AudioClip GRTankCoolShootSfx()
    {
        switch (Random.Range(0, 2))
        {
            case 0:
                return sfx_tankfire_cool1;
            case 1:
                return sfx_tankfire_cool2;
            default:
                break;
        }
        return null;
    }

    public AudioClip GRArtt1ShootSfx()
    {
        return sfx_artfire1_t1;
    }

    public AudioClip GRArtt2ShootSfx()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                return sfx_artfire1_t2;
            case 1:
                return sfx_artfire2_t2;
            case 2:
                return sfx_artfire3_t2;
            default:
                break;
        }
        return null;
    }


}
