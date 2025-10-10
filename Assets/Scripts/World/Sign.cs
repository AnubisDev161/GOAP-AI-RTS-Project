using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Sign : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private string location_plan = "default";
    [SerializeField] private TextMeshProUGUI location_description;
    [SerializeField] private Canvas canvas;

    private void Start()
    {
        location_description.text = location_plan;
        canvas.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
      
        if (other.CompareTag("Player"))
        {
            print("player entered");
            canvas.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")){
            canvas.enabled = false;
            print("player exited");
        }
       
    }
}
