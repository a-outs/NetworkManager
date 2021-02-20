using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gamemanager : MonoBehaviour
{

    [SerializeField]
    private double money;

    private double moneyPerTime;

    public GameObject serviceStation;
    public List<GameObject> serviceStations;
    public GameObject cable;
    public List<GameObject> cables;

    public bool cableBuilding;

    public double cableCost;
    public double serviceStationCost;
    public double serviceStationMaintenance;

    private GameObject moneyText;

    // Start is called before the first frame update
    void Start()
    {
        moneyText = GameObject.Find("Money");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel")) {
            foreach (GameObject oneCable in cables) {
                oneCable.GetComponent<cablescript>().buildmode = false;
            }
            cableBuilding = false;
            setMoney(-609);
        }

        // Service station spawning
        if(money >= serviceStationCost && Input.GetKeyDown(KeyCode.B)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if(hit.collider != null) {
                if(hit.collider.tag == "ServiceArea") {
                    Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    target.z = transform.position.z;
                    GameObject tempStation = Instantiate(serviceStation, target, transform.rotation);
                    serviceStations.Add(tempStation);
                    hit.collider.gameObject.GetComponent<servicearea>().addServiceStation(tempStation);
                    tempStation.GetComponent<servicestation>().addServiceArea(hit.collider.gameObject);
                }
            }
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

    public double getMoney() {
        return money;
    }

    public void setMoney(double moneyChange) {
        money += moneyChange;
        moneyText.GetComponent<TextMeshProUGUI>().text = "Money: " + money;
    }

    public void updateMoneyPerTime() {
        moneyPerTime = 0;
        moneyPerTime -= serviceStations.Count * serviceStationMaintenance;
    }
}