using UnityEngine;

public class Long_Range_Unit: Unit
{
    [SerializeField] private GameObject projectile_prefab;
    [SerializeField] private float projectile_speed;
    [SerializeField] private AnimationCurve trajectory_curve;
    [SerializeField] private Transform projectile_spawn_position;
    public void long_range_attack()
    {
        transform.LookAt(target.transform.position);
        var projectile = Instantiate(projectile_prefab, projectile_spawn_position.position, projectile_prefab.transform.rotation).GetComponent<Projectile>();

        projectile.initialize_projectile(target.transform, damage, projectile_speed, factionType);
    }   

    public override void OnAwake()
    {
        unit_attack_state = new Unit_Long_Range_Attack_State(this, stateMachine);
    }

}
