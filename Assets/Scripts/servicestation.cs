using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class servicestation : MonoBehaviour
{
    public GameObject serviceArea;
    public List<GameObject> connectedStations;
    public bool connectedToServer;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isConnected() || connectedToServer)
        {
            spriteRenderer.color = Color.yellow;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
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

    //private bool setStatusToConnected()
    //{

    //}
    private bool isConnected()
    {
        foreach(GameObject station in connectedStations)
        {
            if (station.CompareTag("ServiceStation"))
            {
                bool connected = station.GetComponent<servicestation>().connectedToServer;
                if (connected)
                {
                    connectedToServer = true;
                }
            }
            // If the current station is directly conencted to the "source"
            else if (station.CompareTag("ServiceSource"))
            {
                connectedToServer = true;
            }
        }
        return false;
    }
}
