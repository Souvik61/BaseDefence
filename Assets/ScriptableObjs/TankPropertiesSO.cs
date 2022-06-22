﻿using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TankProps", order = 1)]
public class TankPropertiesSO : ScriptableObject
{
    public string tankName;
    public float driveSpeed;
    public float rotateSpeed;
    public float muzzRotateSpeed;
    public float shootDelay;
    public float shootDamage;
    public uint maxHealth;

    //Destroyed state sprites
    public Sprite[] destroyedSpriteArray;

    public AudioClip[] shootSfxList;

    public AudioClip GenerateRandomSfx()
    {
        return shootSfxList[Random.Range(0, shootSfxList.Length)];
    }
}

