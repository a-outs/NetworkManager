using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamemanager : MonoBehaviour
{

    public double money;

    public GameObject serviceStation;
    public GameObject cable;

    public List<GameObject> cables;
    public bool cableBuilding;

    public double cableCost;
    public double serviceStationCost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel")) {
            foreach (GameObject oneCable in cables) {
                oneCable.GetComponent<cablescript>().buildmode = false;
            }
            cableBuilding = false;
        }

        // Service station spawning
        if(money >= serviceStationCost && Input.GetKeyDown(KeyCode.B)) {

        }

        // Cable Building
        if(Input.GetKeyDown(KeyCode.C) && !cableBuilding) {
            cableBuilding = true;
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;
            GameObject temp = Instantiate(cable, target, transform.rotation);
            cables.Add(temp);
        }
    }
}
