using UnityEngine;

public class Worker_View_Range : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var potenial_new_target = other.GetComponent<Resource>();
        var parent_unit = GetComponentInParent<Worker>();
        update_unit_target(parent_unit, potenial_new_target, false);
    }
    private void OnTriggerExit(Collider other)
    {
        var potenial_new_target = other.GetComponent<Resource>();
        var parent_unit = GetComponentInParent<Worker>();
        update_unit_target(parent_unit, potenial_new_target, true);
    }
    private void update_unit_target(Worker parent_unit, Resource potential_new_target, bool remove_potential_target_form_target_list)
    {
        if (potential_new_target == null || parent_unit == null)
        {
            return;
        }
       
        if (remove_potential_target_form_target_list)
        {
            parent_unit.targets_in_range.Remove(potential_new_target.gameObject);
        }
        else
        {
            parent_unit.targets_in_range.Add(potential_new_target.gameObject);
        }
        if (!parent_unit.order_placed)
        {

            parent_unit.find_closest_target_in_range();

        }

        
    }
}
