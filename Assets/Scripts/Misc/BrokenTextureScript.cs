using System.Collections.Generic;
using UnityEngine;

public class BrokenTextureScript : MonoBehaviour
{
    public bool SetBroken {
        set
        {
            if (value)
            {
                for (int i = 0; i < spriteRenderers.Count; i++)
                {
                    spriteRenderers[i].sprite = brokenSprites[i];
                }
            }
            else
            {
                for (int i = 0; i < spriteRenderers.Count; i++)
                {
                    spriteRenderers[i].sprite = okSprites[i];
                }
            }
        }
    }

    public List<SpriteRenderer> spriteRenderers;
    public List<Sprite> brokenSprites;
    public List<Sprite> okSprites;
}
