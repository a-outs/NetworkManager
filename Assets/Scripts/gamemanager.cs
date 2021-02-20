using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gamemanager : MonoBehaviour
{

    [SerializeField]
    private double money;

    int time;

    private double moneyPerTime;

    public GameObject serviceStation;
    public List<GameObject> serviceStations;
    public GameObject cable;
    public List<GameObject> cables;

    public GameObject[] serviceAreas;

    public bool cableBuilding;

    public double cableCost;
    public double serviceStationCost;
    public double serviceStationMaintenance;

    private GameObject moneyText;
    private GameObject moneyPerTimeText;
    private GameObject timeText;

    // Start is called before the first frame update
    void Start()
    {
        time = 1;

        moneyText = GameObject.Find("Money");
        moneyPerTimeText = GameObject.Find("MoneyPerTime");
        timeText = GameObject.Find("Time");

        serviceAreas = GameObject.FindGameObjectsWithTag("ServiceArea");

        setMoney(0);
        StartCoroutine("TimeAdvancement");
    }

    // Update is called once per frame
    void Update()
    {
        //stop cable building if you press escape
        if(Input.GetButtonDown("Cancel")) {
            foreach (GameObject oneCable in cables) {
                oneCable.GetComponent<cablescript>().buildmode = false;
            }
            cableBuilding = false;
        }

        // Service station spawning
        if(money >= serviceStationCost && Input.GetKeyDown(KeyCode.B)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if(hit.collider != null) {
                if(hit.collider.tag == "ServiceArea") { 
                    // everything up to here is checking to see if there is enough money and if the station is on a service area and not ontop of another station
                    Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    target.z = transform.position.z;
                    GameObject tempStation = Instantiate(serviceStation, target, transform.rotation);

                    serviceStations.Add(tempStation);

                    hit.collider.gameObject.GetComponent<servicearea>().addServiceStation(tempStation);
                    tempStation.GetComponent<servicestation>().addServiceArea(hit.collider.gameObject);

                    setMoney(-serviceStationCost);
                    updateMoneyPerTime();
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
        foreach (GameObject serviceArea in serviceAreas) {
            moneyPerTime += serviceArea.GetComponent<servicearea>().returnPerTime();
        }
        string moneyPerTimeString = "";
        if(moneyPerTime > 0) {
            moneyPerTimeString = "+" + moneyPerTime;
        }
        else if (moneyPerTime < 0) {
            moneyPerTimeString = "" + moneyPerTime;
        }
        moneyPerTimeText.GetComponent<TextMeshProUGUI>().text = moneyPerTimeString;
    }

    IEnumerator TimeAdvancement() {
        while(true) {
            timeText.GetComponent<TextMeshProUGUI>().text = "Time: " + time;
            time++;
            setMoney(moneyPerTime);
            yield return new WaitForSeconds(3);
        }
    }
}