using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class Unit_Opponent_In_View_Range_Check: MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Target>(out Target target))
        {
            var parent_unit = GetComponentInParent<Unit>();
            update_unit_target(parent_unit, target, false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Target>(out Target target))
        {
            var parent_unit = GetComponentInParent<Unit>();
            update_unit_target(parent_unit, target, true);
        }
    }

    private void update_unit_target(Unit parent_unit, Target potential_new_target,  bool remove_potential_target_form_target_list)
    {
        if (potential_new_target == null || parent_unit == null)
        {
            return;
        }

        if (potential_new_target.faction != parent_unit.faction && potential_new_target != parent_unit)
        {
            if (remove_potential_target_form_target_list)
            {
                parent_unit.targets_in_range.Remove(potential_new_target.gameObject);
            }
            else
            {
                parent_unit.targets_in_range.Add(potential_new_target.gameObject);
            }

            if (parent_unit.target == null)
            {
              
               parent_unit.find_closest_target_in_range();
                
            }
        }
    }
}

