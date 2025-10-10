using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Transform target;
    [SerializeField] private Target.factionTypes parent_faction = Target.factionTypes.neutral;
    [SerializeField] private float damage = 70f;

    private Vector3 trajectory_start_point;
    private Rigidbody rb;
    [SerializeField] float initial_velocity;
    [SerializeField] float angle;
    private float max_throw_force = 0f;


    // Update is called once per frame
   
    
    public void initialize_projectile(Transform _target, float _damage, float _move_speed, Target.factionTypes _parent_faction)
    {
       rb = GetComponent<Rigidbody>();
       trajectory_start_point = transform.position;
       target = _target;
       max_throw_force = _move_speed;
       damage = _damage;
       parent_faction = _parent_faction;
       launch();
    }

    public void launch()
    {
        Vector3 vector_to_target =  new Vector3(target.position.x,trajectory_start_point.y,target.position.z) - trajectory_start_point;

        float delta_y = target.position.y - trajectory_start_point.y;
        float delta_x_z = vector_to_target.magnitude;

        
        float gravity = Mathf.Abs(Physics.gravity.y);
        float throw_strength = Mathf.Clamp(Mathf.Sqrt(gravity * (delta_y + Mathf.Sqrt(Mathf.Pow(delta_y, 2) + Mathf.Pow(delta_x_z, 2)))), 0.01f, max_throw_force);

        float angle = Mathf.PI / 2f - (0.5f * (Mathf.PI / 2 - (delta_y / delta_x_z)));

        Vector3 initial_velocity = Mathf.Cos(angle) * throw_strength * vector_to_target.normalized + Mathf.Sin(angle) * throw_strength * Vector3.up;

        rb.useGravity = true;
        rb.isKinematic = false;
        rb.linearVelocity = initial_velocity;
        rb.angularVelocity = initial_velocity;

    }
    private void OnCollisionEnter(Collision collision)
    {
        var other_unit = collision.gameObject.GetComponent<Unit>();
        if (other_unit != null && other_unit.factionType != parent_faction)
        {
            other_unit.update_health(-damage, GetComponentInParent<Unit>());
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}


