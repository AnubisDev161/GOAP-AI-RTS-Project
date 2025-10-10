using UnityEngine;
using UnityEngine.Events;
public class Player_Character: Unit 
{
    public Player_UI player_UI;
    public Unity_Update_Gold_Event onUpdate_gold;

}

[System.Serializable]
public class Unity_Update_Gold_Event: UnityEvent<int> { }

