using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class servicestation : MonoBehaviour
{
    public GameObject serviceArea;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void addServiceArea(GameObject inServiceArea) {
        serviceArea = inServiceArea;
    }
}
