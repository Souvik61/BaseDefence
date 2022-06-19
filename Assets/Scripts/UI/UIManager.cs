using UnityEngine.EventSystems;
using UnityEngine;

/// <summary>
/// Responsible for getting UI Events.
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField]
    PanelManager panel;

    public AllEventsScript.ButtonCallback OnUIButtonPressed;
    public AllEventsScript.ButtonCallback OnTouchCallback;

    private void OnEnable()
    {
        panel.OnButtonPressedCallback += OnUIBtPressed;
        AllEventsScript.OnTouchCallback += OnTouchEvent;
    }

    private void OnDisable()
    {
        panel.OnButtonPressedCallback -= OnUIBtPressed;
        AllEventsScript.OnTouchCallback -= OnTouchEvent;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsMouseOverUI())
        {
            OnTouchEvent("tch" + Input.mousePosition);
        }
    }



    public void PromptMessage(string message)
    { 
    
    }

    //UI events

    void OnTouchEvent(string str)//For touch events outside UI
    {
        Debug.Log(str);
        OnTouchCallback?.Invoke(str);
    }

    void OnUIBtPressed(string btName)//On UI button pressed callback
    {
        OnUIButtonPressed?.Invoke(btName);
    }

    //Utilities

    bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
