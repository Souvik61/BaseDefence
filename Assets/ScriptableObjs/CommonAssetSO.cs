using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CommonAsset", order = 1)]
public class CommonAssetSO : ScriptableObject
{
    public GameObject SmokePrefab;
    public GameObject WhiteFlagPrefab;
    public GameObject MuzzleFlashPrefab;
    public GameObject ProjectilePrefab;
    public GameObject TankHitPrefab;

    //Tank prefabs
    public GameObject TANK_1;
    public GameObject TANK_2;
    public GameObject TANK_3;

    //Audio clips
    public AudioClip TankShoot;
    public AudioClip Bgm;

    //Miscelleneous
    public GameObject RedCross;

    public AudioClip GenerateRandomTankShootSfx()
    {
        return TankShoot;
    }

}
