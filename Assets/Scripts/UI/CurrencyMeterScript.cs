using UnityEngine;
using TMPro;

public class CurrencyMeterScript : MonoBehaviour
{
    public TMP_Text currencyText;
    public WalletScript walletScript;

    private void OnEnable()
    {
        walletScript.OnCoinDecrease += UpdateText;
        walletScript.OnCoinIncrease += UpdateText;
    }

    private void OnDisable()
    {
        walletScript.OnCoinDecrease -= UpdateText;
        walletScript.OnCoinIncrease -= UpdateText;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
    }

    void UpdateText()
    {
        currencyText.text = walletScript.currentCoins.ToString(); 
    }
}
