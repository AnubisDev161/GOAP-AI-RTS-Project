using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_UI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int player_gold = 0;
    [SerializeField] private TextMeshProUGUI gold_label;
    void Start()
    {
        gold_label.text = ("gold: " + player_gold);
    }

    // Update is called once per frame
   
    public void update_gold_amount(int gold)
    {
        player_gold += gold;
        gold_label.text =("gold: "+ player_gold);
    }

    public bool can_buy(int gold)
    {
        if (player_gold - gold >= 0)
        {
            update_gold_amount( - gold);
            return true;
        }
        print("player has not enough money");
        return false;
    }
}
