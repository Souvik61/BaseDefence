using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ArtProps", order = 1)]
public class ArtileryPropertiesSO : ScriptableObject
{
    public string artileryName;
    public float shootDelay;
    public float shootDamage;
    public uint maxHealth;
    public uint sDamage;//Damage and attack values ranges from [1-10]
    public uint armour;

    //Destroyed state sprites
    public Sprite[] destroyedSpriteArray;
}

