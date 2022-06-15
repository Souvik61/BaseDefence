using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    AudioSource audSrc;

    public CommonAssetSO commonAsset;

    private void Awake()
    {
        audSrc = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audSrc.clip = commonAsset.Bgm;
        audSrc.loop = true;
        audSrc.Play();
    }

}
