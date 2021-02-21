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
    private double score;

    public float hoverOverLength;

    public float repairTime;
    public float buildTime;

    public GameObject serviceStation;
    public List<GameObject> serviceStations;
    public GameObject cable;
    public List<GameObject> cables;

    public GameObject[] serviceAreas;
    public int serviceAreasCovered;

    public GameObject serviceSource;

    [SerializeField]
    List<GameObject> validStations;

    private bool cableBuilding;
    private bool stationBuilding;

    public double cableCost;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip connectClip;
    [SerializeField]
    private AudioClip stationClip;
    [SerializeField]
    private AudioClip clickClip;

    private GameObject moneyText;
    private GameObject moneyPerTimeText;
    private GameObject timeText;
    private GameObject scoreText;
    private GameObject buildingStatus;
    private GameObject StationIcon;

    // Start is called before the first frame update
    void Start()
    {
        time = 1;

        moneyText = GameObject.Find("Money");
        moneyPerTimeText = GameObject.Find("MoneyPerTime");
        timeText = GameObject.Find("Time");
        scoreText = GameObject.Find("Score");
        buildingStatus = GameObject.Find("BuildingStatus");
        StationIcon = GameObject.Find("StationIcon");
        StationIcon.SetActive(false);

        audioSource = GetComponent<AudioSource>();

        serviceAreas = GameObject.FindGameObjectsWithTag("ServiceArea");

        setMoney(0);
        StartCoroutine("TimeAdvancement");
    }

    // Update is called once per frame
    void Update()
    {
        updateMoneyPerTime();

        serviceAreasCovered = 0;
        foreach(GameObject serviceArea in serviceAreas) {
            if(serviceArea.GetComponent<servicearea>().serviceStations.Count > 0) serviceAreasCovered++;
        }
        score = money * serviceStations.Count * Mathf.Pow(1.5f, serviceAreasCovered);
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString("F2");

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

        // make station icon follow mouse if building
        if(stationBuilding) {
            double serviceStationCost = 49 + Mathf.Pow(1.2f, serviceStations.Count);
            StationIcon.SetActive(true);
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;
            StationIcon.transform.position = target;
            StationIcon.transform.Find("Canvas").Find("Text").GetComponent<TextMeshProUGUI>().text = "<color=\"red\">" + serviceStationCost.ToString("F2") + "</color>";
        }
        else StationIcon.SetActive(false);


        // Service station spawning
        if(Input.GetMouseButtonDown(0) && stationBuilding) {
            double serviceStationCost = 49 + Mathf.Pow(1.2f, serviceStations.Count);
            if(money >= serviceStationCost) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
                if(hit.collider != null) {
                    if(hit.collider.tag == "ServiceArea") { 
                        // everything up to here is checking to see if there is enough money and if the station is on a service area and not ontop of another station
                        audioSource.PlayOneShot(stationClip);

                        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        target.z = transform.position.z;
                        GameObject tempStation = Instantiate(serviceStation, target, transform.rotation);
                        
                        double serviceStationMaintenance = 5 * Mathf.Pow(1.05f, serviceStations.Count);
                        double serviceStationRepairCost = (double) Mathf.Log((float) serviceStationCost, 2f);
                        
                        serviceStations.Add(tempStation);

                        hit.collider.gameObject.GetComponent<servicearea>().addServiceStation(tempStation);
                        tempStation.GetComponent<servicestation>().addServiceArea(hit.collider.gameObject);

                        tempStation.GetComponent<servicestation>().setCosts(serviceStationMaintenance, serviceStationRepairCost);

                        setMoney(-serviceStationCost);
                    }
                }
            }
        }

        // Cable Building
        if(Input.GetKeyDown(KeyCode.C)) {
            BuildCable();
        }

        ValidStations();

        if(cableBuilding) buildingStatus.GetComponent<TextMeshProUGUI>().text = "Building: Cable";
        else if(stationBuilding) buildingStatus.GetComponent<TextMeshProUGUI>().text = "Building: Station";
        else buildingStatus.GetComponent<TextMeshProUGUI>().text = "Building: NOTHING";
    }

    public double getMoney() {
        return money;
    }

    public bool isBuilding() {
        return stationBuilding || cableBuilding;
    }

    public void setMoney(double moneyChange) {
        money += moneyChange;
        moneyText.GetComponent<TextMeshProUGUI>().text = money.ToString("F2");
    }

    public void updateMoneyPerTime() {
        moneyPerTime = 0;
        foreach (GameObject serviceStation in serviceStations) {
            moneyPerTime -= serviceStation.GetComponent<servicestation>().getMaintenanceCost();
        }
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
        //moneyPerTimeString += " / Day";
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

    public void PlayCableNoise() {
        audioSource.PlayOneShot(connectClip);
    }

    public void ValidStations() {
        validStations = new List<GameObject>();
        validStations.Add(serviceSource);
        foreach(GameObject station in serviceSource.GetComponent<servicestation>().connectedStations) {
            if(!validStations.Contains(station)) {
                validStations = station.GetComponent<servicestation>().ValidStationList(validStations);
            }
        }

        foreach(GameObject station in serviceStations) {
            if(validStations.Contains(station)) {
                station.GetComponent<servicestation>().connectedToServer = true;
            }
            else {
                station.GetComponent<servicestation>().connectedToServer = false;
            }
        }
    }

    public void StationToggler() {
        
    }

    IEnumerator TimeAdvancement() {
        while(true) {
            timeText.GetComponent<TextMeshProUGUI>().text = time + "/" + (100) + " days";
            time++;
            setMoney(moneyPerTime);
            yield return new WaitForSeconds(6);
        }
    }
}