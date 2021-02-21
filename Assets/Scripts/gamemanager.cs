using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class gamemanager : MonoBehaviour
{

    [SerializeField]
    private double money;
    private int time;
    private double moneyPerTime;
    private double score;
    private bool gameOver;

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
    [SerializeField]
    private AudioClip stormClip;

    private GameObject moneyText;
    private GameObject moneyPerTimeText;
    private GameObject timeText;
    private GameObject scoreText;
    private GameObject StationIcon;
    private GameObject cableBuildingIndicator;
    private GameObject stationBuildingIndicator;
    public GameObject endGame;
    public GameObject endGameText;

    // Start is called before the first frame update
    void Start()
    {
        time = 1;
        gameOver = false;

        moneyText = GameObject.Find("Money");
        moneyPerTimeText = GameObject.Find("MoneyPerTime");
        timeText = GameObject.Find("Time");
        scoreText = GameObject.Find("Score");
        StationIcon = GameObject.Find("StationIcon");
        cableBuildingIndicator = GameObject.Find("CableIndicator");
        stationBuildingIndicator = GameObject.Find("StationIndicator");
        StationIcon.SetActive(false);

        audioSource = GetComponent<AudioSource>();

        serviceAreas = GameObject.FindGameObjectsWithTag("ServiceArea");

        setMoney(0);
        StartCoroutine("TimeAdvancement");
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameOver) {
            updateMoneyPerTime();

            serviceAreasCovered = 0;
            foreach(GameObject serviceArea in serviceAreas) {
                if(serviceArea.GetComponent<servicearea>().serviceStations.Count > 0) serviceAreasCovered++;
            }
            score = money * serviceStations.Count * Mathf.Pow(1.5f, serviceAreasCovered);
            scoreText.GetComponent<TextMeshProUGUI>().text = "score: " + score.ToString("N0");

            //stop cable building if you press escape
            if(Input.GetButtonDown("Cancel") || Input.GetMouseButtonDown(1)) {
                StopAllBuilding();
            }

            // make station icon follow mouse if building
            if(stationBuilding) {
                double serviceStationCost = 49 + Mathf.Pow(1.2f, serviceStations.Count);
                StationIcon.SetActive(true);
                Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                target.z = transform.position.z;
                StationIcon.transform.position = target;
                string costString = "";
                if(money >= serviceStationCost) {
                    costString = "<color=\"green\">" + serviceStationCost.ToString("F2") + "</color>";
                }
                else {
                    costString = "<color=\"red\">" + serviceStationCost.ToString("F2") + "</color>";
                }
                StationIcon.transform.Find("Canvas").Find("Text").GetComponent<TextMeshProUGUI>().text = costString;
            }
            else StationIcon.SetActive(false);


            // Service station spawning
            if(Input.GetMouseButtonDown(0) && stationBuilding) {
                double serviceStationCost = 49 + Mathf.Pow(1.2f, serviceStations.Count);
                foreach(GameObject station in serviceStations) {
                    station.GetComponent<servicestation>().setRepairCost((double) 5 * Mathf.Log((float) serviceStationCost, 2f));
                }
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
                            double serviceStationRepairCost = (double) 5 * Mathf.Log((float) serviceStationCost, 2f);
                            
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

            // Station building
            if(Input.GetKeyDown(KeyCode.B)) {
                BuildStation();
            }
            
            if(cableBuilding) {
                cableBuildingIndicator.SetActive(true);
            }
            else {
                cableBuildingIndicator.SetActive(false);
            }

            if(stationBuilding) {
                stationBuildingIndicator.SetActive(true);
            }
            else {
                stationBuildingIndicator.SetActive(false);
            }

            if(time > 100) {
                gameOver = true;
                GameOver();
            }

            ValidStations();
        }
    }

    public double getMoney() {
        return money;
    }

    public bool isBuilding() {
        return stationBuilding || cableBuilding;
    }

    public void StopAllBuilding() {
        foreach (GameObject oneCable in cables) {
            oneCable.GetComponent<cablescript>().buildmode = false;
        }
        cableBuilding = false;
        stationBuilding = false;
    }

    public void setMoney(double moneyChange) {
        money += moneyChange;
        moneyText.GetComponent<TextMeshProUGUI>().text = "$: " + money.ToString("F2");
    }

    public void updateMoneyPerTime() {
        moneyPerTime = 0;
        foreach (GameObject serviceStation in serviceStations) {
            moneyPerTime -= serviceStation.GetComponent<servicestation>().getMaintenanceCost();
        }
        foreach (GameObject serviceArea in serviceAreas) {
            moneyPerTime += serviceArea.GetComponent<servicearea>().returnPerTime();
        }
        string moneyPerTimeString = "$/day: ";
        if(moneyPerTime > 0) {
            moneyPerTimeString += "<color=\"green\">" + "+" + moneyPerTime.ToString("F2") + "</color>";
        }
        else if (moneyPerTime < 0) {
            moneyPerTimeString += "<color=\"red\">" + moneyPerTime.ToString("F2") + "</color>";
        }
        else {
            moneyPerTimeString += "" + 0;
        } 
        //moneyPerTimeString += " / Day";
        moneyPerTimeText.GetComponent<TextMeshProUGUI>().text = moneyPerTimeString;
    }

    public void BuildStation() {
        audioSource.PlayOneShot(clickClip);
        if(!isBuilding()) {
            stationBuilding = true;
        }
        else {
            StopAllBuilding();
        }
    }

    public void BuildCable() {
        audioSource.PlayOneShot(clickClip);
        if(!cableBuilding) {
            stationBuilding = false;
            cableBuilding = true;
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;

            // The UI cost display script needs the cable to be isntantiated from the start
            GameObject temp = Instantiate(cable, target, transform.rotation);
            cables.Add(temp);
        }
        else {
            StopAllBuilding();
        }
    }

    public void EndButton() {
        audioSource.PlayOneShot(clickClip);
        SceneManager.LoadScene("MainMenu");
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

    public void GameOver() {
        endGame.SetActive(true);
        endGame.GetComponent<CanvasGroup>().DOFade(1f, 1f);
        endGameText.GetComponent<TextMeshProUGUI>().text = "score: " + score.ToString("N0");
    }

    IEnumerator TimeAdvancement() {
        while(!gameOver) {
            timeText.GetComponent<TextMeshProUGUI>().text = "days: " + time + "/" + (100);
            time++;
            setMoney(moneyPerTime);

            if(Random.Range(0,100) < 25 && time > 3) {
                GetComponent<StormBehavior>().CreateStorm();
                audioSource.PlayOneShot(stormClip, 0.3f);
            }

            yield return new WaitForSeconds(6);
        }
    }
}