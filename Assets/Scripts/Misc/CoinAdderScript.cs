using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinAdderScript : MonoBehaviour
{

    public WalletScript playerWallet;
    public CurrencyDefsSO currencyTerms;

    public void OnATankDestroyed(string name)
    {
        playerWallet.Deposit(TankNameToBBPrice(name));
    }

    uint TankNameToBBPrice(string name)
    {
        return currencyTerms.buybackDict[name];
    }

}
