using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent (typeof(SphereCollider))]
public class Sensor : MonoBehaviour
{
    [SerializeField] float detectionRadius = 5f;
    [SerializeField] float timerInterval = 1f;
    float currentTimeInterval;
    SphereCollider detectionRange;

    public event Action OnTargetChanged = delegate { };

    public Vector3 TargetPosition => target? target.transform.position : Vector3.zero;  
    public bool IsTargetInRange => TargetPosition != Vector3.zero;


    GameObject target;
    Vector3 lastKnownPosition;
    private void Awake()
    {
        detectionRange = GetComponent<SphereCollider>();
        detectionRange.isTrigger = true;
        detectionRange.radius = detectionRadius;
    }

    private void Update()
    {
        currentTimeInterval += Time.deltaTime;
        if (currentTimeInterval < timerInterval)
        {
            currentTimeInterval = 0;
            onTimerTimeout();
        }   
    }

    private void onTimerTimeout()
    {
        UpdateTargetPosition(target);
    }

    void UpdateTargetPosition(GameObject target = null)
    {
        this.target = target;
        if(IsTargetInRange && (lastKnownPosition != TargetPosition || lastKnownPosition != Vector3.zero))
        {
            lastKnownPosition = TargetPosition;
            OnTargetChanged.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        UpdateTargetPosition(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        UpdateTargetPosition();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsTargetInRange ? Color.red : Color.green;
        Gizmos.DrawWireSphere(TargetPosition, detectionRadius);
    }
}
