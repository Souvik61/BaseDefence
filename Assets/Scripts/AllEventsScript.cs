using UnityEngine;

// Class to contain all event functions
public class AllEventsScript : MonoBehaviour
{
    public delegate void BlankFunc();

    // public static Func1 OnArrestInitiated;
    public delegate void Func1(int id);
    public static Func1 OnBaseDestroyed;
    public static BlankFunc OnGameOver;

}