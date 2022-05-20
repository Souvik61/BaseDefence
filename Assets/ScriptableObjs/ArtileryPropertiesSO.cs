using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ArtProps", order = 1)]
public class ArtileryPropertiesSO : ScriptableObject
{
    public string artileryName;
    public float shootDelay;
    public float shootDamage;
    public uint maxHealth;

    //Destroyed state sprites
    public Sprite[] destroyedSpriteArray;
}

