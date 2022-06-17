using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    PanelManager panel;

    public AllEventsScript.ButtonCallback OnUIButtonPressed;

    private void OnEnable()
    {
        panel.OnButtonPressedCallback += OnUIBtPressed;
    }

    private void OnDisable()
    {
        panel.OnButtonPressedCallback -= OnUIBtPressed;
    }

    void OnUIBtPressed(string btName)//On UI button pressed callback
    {
        OnUIButtonPressed?.Invoke(btName);
    }

}
