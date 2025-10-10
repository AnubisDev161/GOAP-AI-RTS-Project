using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Collectables: MonoBehaviour
{
    [SerializeField] private Button collect_button;
    [SerializeField] private int gold_value;
    private Player_Character player = null;

    private void Start()
    {
        collect_button.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("player entered");
            collect_button.gameObject.SetActive(true);
            player = other.GetComponent<Player_Character>();
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("player entered");
            collect_button.gameObject.SetActive(false);
            player = null;
        }
    }
    public void OnCollect_button_clicked()
    {
        if (player != null) {
            player.onUpdate_gold.Invoke(gold_value);
            Destroy(gameObject);
           
        }
    }
}
