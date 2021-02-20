using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class servicestation : MonoBehaviour
{
    public GameObject serviceArea;

    public List<GameObject> connectedStations;

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

    public void addConnection(GameObject connectedStation) {
        connectedStations.Add(connectedStation);
    }
}
