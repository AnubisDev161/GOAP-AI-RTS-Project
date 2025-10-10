using UnityEngine;

public interface IDamageable
{
    void update_health(float increment, Unit attacker);

    void die();

    float max_health {  get; set; }

    float current_health {  get; set; }
}
