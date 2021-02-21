using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gamemanager : MonoBehaviour
{

    [SerializeField]
    private double money;

    private int time;

    private double moneyPerTime;

    public float hoverOverLength;

    public float repairTime;

    public float buildTime;

    public GameObject serviceStation;
    public List<GameObject> serviceStations;
    public GameObject cable;
    public List<GameObject> cables;

    public GameObject[] serviceAreas;

    private GameObject serviceSource;

    private bool cableBuilding;
    private bool stationBuilding;

    public double cableCost;
    public double serviceStationCost;
    public double serviceStationMaintenance;
    public double serviceStationRepairCost;

    private GameObject moneyText;
    private GameObject moneyPerTimeText;
    private GameObject timeText;
    private GameObject buildingStatus;

    // Start is called before the first frame update
    void Start()
    {
        time = 1;

        moneyText = GameObject.Find("Money");
        moneyPerTimeText = GameObject.Find("MoneyPerTime");
        timeText = GameObject.Find("Time");
        buildingStatus = GameObject.Find("BuildingStatus");

        serviceAreas = GameObject.FindGameObjectsWithTag("ServiceArea");

        serviceSource = GameObject.Find("ServiceSource");

        setMoney(0);
        updateMoneyPerTime();
        StartCoroutine("TimeAdvancement");
    }

    // Update is called once per frame
    void Update()
    {
        //stop cable building if you press escape
        if(Input.GetButtonDown("Cancel") || Input.GetMouseButtonDown(1)) {
            foreach (GameObject oneCable in cables) {
                oneCable.GetComponent<cablescript>().buildmode = false;
            }
            cableBuilding = false;
            stationBuilding = false;
        }

        if(Input.GetKeyDown(KeyCode.B)) {
            BuildStation();
        }

        // Service station spawning
        if(Input.GetMouseButtonDown(0) && money >= serviceStationCost && stationBuilding) {
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
        if(Input.GetKeyDown(KeyCode.C)) {
            BuildCable();
        }

        if(cableBuilding) buildingStatus.GetComponent<TextMeshProUGUI>().text = "Building: Cable";
        else if(stationBuilding) buildingStatus.GetComponent<TextMeshProUGUI>().text = "Building: Station";
        else buildingStatus.GetComponent<TextMeshProUGUI>().text = "Building: NOTHING";
    }

    public double getMoney() {
        return money;
    }

    public bool isBuilding() {
        return stationBuilding && cableBuilding;
    }

    public void setMoney(double moneyChange) {
        money += moneyChange;
        moneyText.GetComponent<TextMeshProUGUI>().text = "Money: " + money.ToString("F2");
    }

    public void updateMoneyPerTime() {
        moneyPerTime = 0;
        moneyPerTime -= serviceStations.Count * serviceStationMaintenance;
        foreach (GameObject serviceArea in serviceAreas) {
            moneyPerTime += serviceArea.GetComponent<servicearea>().returnPerTime();
        }
        string moneyPerTimeString = "";
        if(moneyPerTime > 0) {
            moneyPerTimeString = "<color=\"green\">" + "+" + moneyPerTime.ToString("F2") + "</color>";
        }
        else if (moneyPerTime < 0) {
            moneyPerTimeString = "<color=\"red\">" + moneyPerTime.ToString("F2") + "</color>";
        }
        moneyPerTimeText.GetComponent<TextMeshProUGUI>().text = moneyPerTimeString;
    }

    public void BuildStation() {
        if(!isBuilding()) {
            stationBuilding = true;
        }
    }

    public void BuildCable() {
        if(!cableBuilding) {
            stationBuilding = false;
            cableBuilding = true;
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;


            // The UI cost display script needs the cable to be isntantiated from the start
            GameObject temp = Instantiate(cable, target, transform.rotation);
            cables.Add(temp);
        }
    }

    public void ValidStations() {
        List<GameObject> validStations;
        foreach(GameObject station in serviceSource.GetComponent<servicestation>().connectedStations) {
            validStations.AddRange(station.GetComponent<servicestation>().ValidStationList(validStations));
        }
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