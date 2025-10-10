
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Tavern: MonoBehaviour 
{
    private BoxCollider BoxCollider;
    public Button mercenary_buy_button;
    public Button long_range_mercenary_buy_button;
    public Component mercenary_spawn_pos;
    public GameObject unit_prefab;
    public GameObject long_range_unit_prefab;

    public GameObject player = null;
    [SerializeField] private int mercenary_price = 50;
    [SerializeField] private int long_range_mercenary_price = 70;


    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            mercenary_buy_button.gameObject.SetActive(true);
            long_range_mercenary_buy_button.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            player = null;
            mercenary_buy_button.gameObject.SetActive(false);
            long_range_mercenary_buy_button.gameObject.SetActive(false);
        }
    }
    public void OnBuy_mercenary_button_clicked()
    {
        if (player != null && player.GetComponent<Player_Character>().player_UI.can_buy(mercenary_price)){
            print("mercenary bought");
            GameObject newEnemy = Instantiate(unit_prefab, mercenary_spawn_pos.transform.position, Quaternion.identity);
            var new_enemy_unit_component = newEnemy.GetComponent<Unit>();
            new_enemy_unit_component.factionType = Target.factionTypes.player;
            new_enemy_unit_component.GetComponent<Unit>().commando_unit = player.GetComponent<Unit>();
       
        }
        
    }

    public void OnBuy_long_range_mercenary_button_clicked()
    {
        if (player != null && player.GetComponent<Player_Character>().player_UI.can_buy(long_range_mercenary_price))
        {
            print("long range mercenary bought");
            GameObject newEnemy = Instantiate(long_range_unit_prefab, mercenary_spawn_pos.transform.position, Quaternion.identity);
            var new_enemy_unit_component = newEnemy.GetComponent<Unit>();
            new_enemy_unit_component.factionType = Target.factionTypes.player;
            new_enemy_unit_component.GetComponent<Unit>().commando_unit = player.GetComponent<Unit>();

        }
    }

  
}
