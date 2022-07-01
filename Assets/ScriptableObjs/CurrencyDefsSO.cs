using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CurrencyDefs", order = 1)]
public class CurrencyDefsSO : ScriptableObject
{
    [Header("Tank 1")]
    public string tankName1;
    public uint cost1;

    [Header("Tank 2")]
    public string tankName2;
    public uint cost2;

    [Header("Tank 3")]
    public string tankName3;
    public uint cost3;
    
    [Header("Tank 4")]
    public string tankName4;
    public uint cost4;

    [Header("Tank 5")]
    public string tankName5;
    public uint cost5;

    public uint[] costList;

    public Dictionary<string,uint> costDict;
    public Dictionary<string,uint> buybackDict;

    [System.Serializable]
    public struct TankCurrency
    {
        public string name;
        public uint cost;
        public uint buybackCost;
    }
    
    [Header("Tank Currencies List")]
    public TankCurrency[] tankCurrencies;

    private void OnEnable()
    {
        costDict = new Dictionary<string, uint>();
        buybackDict = new Dictionary<string, uint>();

        //Validate dictionary
        costDict.Add(tankName1, cost1);
        costDict.Add(tankName2, cost2);
        costDict.Add(tankName3, cost3);
        costDict.Add(tankName4, cost4);
        costDict.Add(tankName5, cost5);

        foreach (var item in tankCurrencies)
        {
            buybackDict.Add(item.name, item.buybackCost);
        }

    }


}
