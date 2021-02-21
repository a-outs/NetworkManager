using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class servicestation : MonoBehaviour
{
    public GameObject serviceArea;
    public List<GameObject> connectedStations;
    public bool connectedToServer;
    public bool broken;
    private bool fixing;
    private bool building;

    private SpriteRenderer spriteRenderer;

    private GameObject infoText;
    private GameObject loadingImage;

    private float transitionLength;

    private gamemanager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<gamemanager>();

        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        infoText = gameObject.transform.Find("Canvas").Find("Info").gameObject;
        loadingImage = gameObject.transform.Find("Canvas").Find("Loading").gameObject;
        if(!connectedToServer) {
            building = true;
            StartCoroutine("StartBuilding");
        }
        else loadingImage.SetActive(false);

        transitionLength = gameManager.hoverOverLength;
        infoText.transform.DOScale(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        checkConnections();

        if (isActive())
        {
            spriteRenderer.color = Color.green;
        }
        else
        {
            spriteRenderer.color = Color.yellow;
            if (broken && !fixing) 
            {
                loadingImage.GetComponent<Image>().fillAmount = 1;
                loadingImage.GetComponent<Image>().color = Color.red;
            }
        }

        if(serviceArea) infoText.GetComponent<TextMeshProUGUI>().text = "Station\nCost: " + gameManager.serviceStationMaintenance + "\nRevenue: " + serviceArea.GetComponent<servicearea>().profitForStation(gameObject);
    }

    void OnMouseEnter() {
        infoText.transform.DOScale(0, 0);
        infoText.transform.DOScale(1f, transitionLength);
    }

    void OnMouseExit() {
        infoText.transform.DOScale(1f, 0);
        infoText.transform.DOScale(0, transitionLength);
    }

    void OnMouseDown() {
        if(broken && !fixing) {
            gameManager.setMoney(-gameManager.serviceStationRepairCost);
            fixing = true;
            StartCoroutine("StartRepairing");
        }
    }

    IEnumerator StartBuilding() {
        float fillAmount = 1;
        while(fillAmount > 0) {
            fillAmount -= 1.0f / gameManager.buildTime * Time.deltaTime;
            loadingImage.GetComponent<Image>().fillAmount = fillAmount;
            yield return null;
        }
        building = false;
        yield break;
    }

    IEnumerator StartRepairing() {
        float fillAmount = 1;
        while(fillAmount > 0) {
            fillAmount -= 1.0f / gameManager.buildTime * Time.deltaTime;
            loadingImage.GetComponent<Image>().fillAmount = fillAmount;
            yield return null;
        }
        fixing = false;
        broken = false;
        yield break;
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

    public bool isActive() {
        return connectedToServer && !broken && !building;
    }

    private void checkConnections()
    {
        int connectedActiveStations = 0;
        if(gameObject.name == "ServiceSource") {
            connectedToServer = true;
            connectedActiveStations++;
        }
        foreach(GameObject station in connectedStations)
        {
            if (station.CompareTag("ServiceStation"))
            {
                if (station.GetComponent<servicestation>().isActive())
                {
                    connectedToServer = true;
                    connectedActiveStations++;
                }
            }
            // If the current station is directly conencted to the "source"
            else if (station.name == "ServiceSource")
            {
                connectedToServer = true;
                connectedActiveStations++;
            }
            gameManager.updateMoneyPerTime();
        }
        if(connectedActiveStations == 0) connectedToServer = false;
    }

    public List<GameObject> ValidStationList(List<GameObject> inputList) {
        List<GameObject> outputList = inputList;
        if(broken || building) {
            return inputList;
        }
        foreach(GameObject station in inputList) {
            if(station == gameObject) return inputList;
        }
        foreach(GameObject station in connectedStations) {
            outputList.AddRange(station.GetComponent<servicestation>().ValidStationList(outputList));
        }
        return outputList;
    }
}
