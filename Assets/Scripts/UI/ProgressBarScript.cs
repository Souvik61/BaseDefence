using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ProgressBarScript : MonoBehaviour
{
    public Image barImg;
    public GameObject[] uiElements;

    [SerializeField, Range(0, 1)]
    public float barProgress;

    public bool isVisible;

    public bool IsBarVisible
    {
        get
        {
            return isVisible;
        }
        set
        {
            foreach (var gm in uiElements)
            {
                gm.SetActive(value);
            }
            isVisible = value;
        }
    }

    private void Awake()
    {
        isVisible = false;
    }

    private void Update()
    {
        barImg.fillAmount = barProgress;
    }

}
