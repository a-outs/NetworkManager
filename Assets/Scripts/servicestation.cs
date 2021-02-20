using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class servicestation : MonoBehaviour
{
    public GameObject serviceArea;
    public List<GameObject> connectedStations;
    public bool connectedToServer;

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

    public bool addConnection(GameObject connectedStation) {
        // Only add if it already hasn't been added or it is not itself
        if (!connectedStations.Contains(connectedStation) && connectedStation != this.gameObject) //Gygi not gonna like that I used                                    
        {                                                                                         //this. like this but oh well ¯\_('.')_/¯ 
            connectedStations.Add(connectedStation);
            return true;
        }
        return false; 
    }
}
