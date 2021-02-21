using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameracontroller : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    float zoomedWeight;

    [SerializeField]
    float zoomSpeed;

    [SerializeField]
    float camSize;

    Vector3 velocity;

    [SerializeField]
    private Vector2 camMax;
    [SerializeField]
    private Vector2 camMin;

    [SerializeField]
    private float maxSize;
    [SerializeField]
    private float minSize;

    void Start() {
    }
    
    void Update () {
        MoveCamera();
        ResizeCamera();
    }

    void MoveCamera()
    {
        camSize = GetComponent<Camera>().orthographicSize;
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed * Time.deltaTime * ((GetComponent<Camera>().orthographicSize / 100) * zoomedWeight);

        Vector3 nextPos = transform.position + velocity;
        if (nextPos.x >= camMax.x)
        {
            nextPos.x = camMax.x;
        }
        else if (nextPos.x <= camMin.x)
        {
            nextPos.x = camMin.x;
        }
        else if (nextPos.y >= camMax.y)
        {
            nextPos.y = camMax.y;
        }
        else if (nextPos.y <= camMin.y)
        {
            nextPos.y = camMin.y;
        }
        else
        {
            transform.Translate(velocity);
        }
    }

    void ResizeCamera()
    {
        float camSizeChange = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        if ((camSize - camSizeChange) >= maxSize)
        {
            camSize = maxSize;
        }
        else if ((camSize - camSizeChange) <= minSize)
        {
            camSize = minSize;
        }
        else
        {
            GetComponent<Camera>().orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        }
    }

}
