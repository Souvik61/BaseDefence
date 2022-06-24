using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ProgressBarScript : MonoBehaviour
{
    public Image barImg;
    public GameObject[] uiElements;

    private bool isVisible;
    private float progress;
    /// <summary>
    /// Range [0-1]
    /// </summary>
    public float barProgress {
        get { return progress; }
        set
        {
            progress = value;
            barImg.fillAmount = progress;
        }
    } 

    public bool barVisible
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
        barVisible = false;
    }

}
