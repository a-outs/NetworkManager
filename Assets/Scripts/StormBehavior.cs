using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StormBehavior : MonoBehaviour
{
    private gamemanager gameManager;
    private List<GameObject> serviceStations;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = this.GetComponent<gamemanager>();    
    }

    // Update is called once per frame
    void Update()
    {
        serviceStations = gameManager.serviceStations;
        CheckForStormCreation();
    }

    void CheckForStormCreation()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            print("joe chingas");
            CreateStorm();
        }
    }

    void CreateStorm()
    {
        // Get a random service provider out of the total list
        int randomIndex = Random.Range(0, serviceStations.Count-1);
        //int randomIndex = 1;
        Vector2 providerPos = serviceStations[randomIndex].transform.position;

        // Set the radius of the affected area
        float stormRadius = Random.Range(6, 9);

        // Get all objects within 
        Collider2D[] objectsInRadius = Physics2D.OverlapCircleAll(providerPos, stormRadius);

        if (objectsInRadius != null)
        {
            // Add only colliders that are service providers
            List<GameObject> providerList = new List<GameObject>();
            foreach (Collider2D collider in objectsInRadius)
            {
                if (collider.gameObject.CompareTag("ServiceStation"))
                {
                    providerList.Add(collider.gameObject);
                }
            }
            foreach (GameObject provider in providerList)
            {
                int chanceInt = Random.Range(0, 4);
                // 20% chance of breaking
                if (chanceInt == 0)
                {
                    if (provider.GetComponent<servicestation>().building == false)
                    {
                        provider.GetComponent<servicestation>().broken = true;
                    }
                }
            }
        }
        
    }
}
