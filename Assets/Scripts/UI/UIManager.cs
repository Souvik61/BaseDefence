using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Responsible for getting UI Events.
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField]
    PanelManager panel;
    [SerializeField]
    TMP_Text promptText;

    public AllEventsScript.ButtonCallback OnUIButtonPressed;
    public AllEventsScript.ButtonCallback OnTouchCallback;

    bool isPromptingText;

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
        if (promptText)
        {
            StartCoroutine(nameof(PromptRoutine), message);
        }
    }

    //UI events

    void OnTouchEvent(string str)//For touch events outside UI
    {
        //Debug.Log(str);
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

    //Coroutines

    IEnumerator PromptRoutine(string message)
    {
        isPromptingText = true;

        promptText.text = message;

        //Growing phase

        yield return StartCoroutine(PromptTextGrow());
      
        yield return new WaitForSeconds(1.0f);

        //Shrinking phase

        float b = 1;
        while (b > 0.001f)
        {
            promptText.rectTransform.localScale = new Vector3(promptText.rectTransform.localScale.x, b, 1);
            b -= 4f * Time.deltaTime;
            yield return null;
        }

        promptText.rectTransform.localScale = new Vector3(promptText.rectTransform.localScale.x, 0, 1);
        isPromptingText = false;
    
    }

    IEnumerator PromptTextGrow()
    {
        //Grow to 2 scale
        float a = 1;
        promptText.rectTransform.localScale = new Vector3(promptText.rectTransform.localScale.x, 1, 1);
        while (a <= 2f)
        {
            promptText.rectTransform.localScale = new Vector3(a, a, 1);
            a += 10f * Time.deltaTime;
            yield return null;
        }
        //Wait
        yield return new WaitForSeconds(0.01f);
        //Shrink to 1 scale
        while (a >= 1f)
        {
            promptText.rectTransform.localScale = new Vector3(a, a, 1);
            a -= 10f * Time.deltaTime;
            yield return null;
        }

    }

}
