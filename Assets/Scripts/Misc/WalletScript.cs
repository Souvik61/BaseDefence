using UnityEngine;

public class WalletScript : MonoBehaviour
{
    public delegate void AFunc();

    public uint maxCoins = 10000;

    public uint currentCoins;
    public bool elonMuskMode;

    public AFunc OnCoinDecrease;//If coin decrease
    public AFunc OnCoinIncrease;//If health increase

    public void SetCoins(uint coins)
    {
        coins = (uint)Mathf.Clamp(coins, 0, (int)maxCoins);
        currentCoins = coins;
    }

    public void Deposit(uint value)
    {
        currentCoins = (uint)Mathf.Clamp(currentCoins + (int)value, 0, maxCoins);
        OnCoinIncrease?.Invoke();//Coin increase event
    }

    public void Withdraw(uint value)
    {
        currentCoins = (uint)Mathf.Clamp(currentCoins - (int)value, 0, maxCoins);

        if (elonMuskMode && currentCoins == 0) { currentCoins = 1; }//god mode check

        OnCoinDecrease?.Invoke();//Health decrease event
    }
}
