using UnityEngine;


[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private float panSpeed = 10f;
    [SerializeField]
    private Vector2 panLimit =
    new Vector2(30f, 35f);
    [SerializeField] private float scrollSpeed = 1000f;
    [SerializeField]
    private Vector2 scrollLimit =
    new Vector2(5f, 10f);
    private Vector3 initialPosition = Vector3.zero;
    private Camera camera = null;

    private void Start()
    {
        initialPosition = transform.position;
        camera = GetComponent<Camera>();
    }
    private void UpdateZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
       
        scroll = scroll * scrollSpeed * Time.deltaTime;
        camera.orthographicSize += scroll;
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize,
        scrollLimit.x, scrollLimit.y);
        //Debug.Log(scroll);
    }

    private void UpdatePan()
    {
        Vector3 position = transform.position;
        if (Input.GetKey(KeyCode.W))
        {
            position.z += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            position.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            position.x += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            position.x -= panSpeed * Time.deltaTime;
        }

        position.x = Mathf.Clamp(position.x,
        -panLimit.x + initialPosition.x,
        panLimit.x + initialPosition.x);

        position.z = Mathf.Clamp(position.z,
        -panLimit.y + initialPosition.z,
        panLimit.y + initialPosition.z);

        transform.position = position;
    }

    private void Update()
    {
        UpdateZoom();
        UpdatePan();
    }
}
