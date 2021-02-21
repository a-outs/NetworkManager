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
    Vector3 maxCamBound;
    [SerializeField]
    Vector3 minCambound;

    [SerializeField]
    float camSize;

    Vector2 velocity;

    private int maxSize;
    private int minSize;

    void Start() {
        maxSize = 18;
        minSize = 5;
    }
    
    void Update () {
        camSize = GetComponent<Camera>().orthographicSize;

        velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed * Time.deltaTime * ((GetComponent<Camera>().orthographicSize / 100) * zoomedWeight);
        transform.Translate(velocity);

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

        ////float sizeToSet = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        //if (sizeToSet >= maxSize)
        //{
        //    GetComponent<Camera>().orthographicSize = maxSize;
        //}
        //else if (sizeToSet <= minSize)
        //{
        //    GetComponent<Camera>().orthographicSize = minSize;
        //}
        //else
        //{
        //    GetComponent<Camera>().orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        //}
        
    
}
