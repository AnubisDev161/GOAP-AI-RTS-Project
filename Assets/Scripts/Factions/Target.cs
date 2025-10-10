using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    public static Action<GameObject, Faction> newTargetAvailable;
    public static Action<GameObject> removeUnitFromList;
    public factionTypes factionType;
    public Faction faction;
    public bool dead { get;  set; }
    public enum factionTypes
    {
        neutral,
        player = 10,
        CPU = 20,
    }
}
