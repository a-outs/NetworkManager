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

    Vector2 velocity;
    
    void Start() {
        
    }
    
    void Update () {
        
        velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed * Time.deltaTime * ((GetComponent<Camera>().orthographicSize / 100) * zoomedWeight);
        transform.Translate(velocity);

        GetComponent<Camera>().orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
    }
}
