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
    public bool building;
    private bool fixing;

    private SpriteRenderer spriteRenderer;

    private GameObject info;
    private GameObject infoText;
    private GameObject loadingImage;

    public double maintenanceCost;
    public double repairCost;
    public double revenue;

    [SerializeField]
    private Sprite defaultSprite;
    [SerializeField]
    private Sprite brokenSprite;
    [SerializeField]
    private Sprite disconnectedSprite;
    [SerializeField]
    private Sprite highlightedSprite;

    private float transitionLength;

    private gamemanager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<gamemanager>();

        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        info = gameObject.transform.Find("Canvas").Find("Info").gameObject;
        infoText = info.transform.Find("InfoText").gameObject;
        loadingImage = gameObject.transform.Find("Canvas").Find("Loading").gameObject;
        if(!connectedToServer) {
            building = true;
            StartCoroutine("StartBuilding");
        }
        else loadingImage.SetActive(false);

        transitionLength = gameManager.hoverOverLength;
        info.transform.DOScale(0, 0);
    }

    // Update is called once per frame
    void Update()
    {

        if(gameObject.name != "ServiceSource") {
            if (isActive())
            {
                spriteRenderer.sprite = defaultSprite;
            }
            else
            {
                spriteRenderer.sprite = disconnectedSprite;
                if (broken && !fixing) 
                {
                    spriteRenderer.sprite = brokenSprite;
                    loadingImage.GetComponent<Image>().fillAmount = 1;
                    loadingImage.GetComponent<Image>().color = Color.red;
                }
            }
        }

        if(serviceArea) {
            if(broken) infoText.GetComponent<TextMeshProUGUI>().text = "Station\nCost: " + maintenanceCost.ToString("F2") + "\nRevenue: " + revenue.ToString("F2") + "\nRepair $: " + repairCost.ToString("F2");
            else infoText.GetComponent<TextMeshProUGUI>().text = "Station\nCost: " + maintenanceCost.ToString("F2") + "\nRevenue: " + revenue.ToString("F2");
        }
    }

    void OnMouseEnter() {
        info.transform.DOScale(0, 0);
        info.transform.DOScale(1f, transitionLength);
    }

    void OnMouseExit() {
        info.transform.DOScale(1f, 0);
        info.transform.DOScale(0, transitionLength);
    }

    void OnMouseDown() {
        if(broken && !fixing && gameManager.getMoney() >= repairCost) {
            gameManager.setMoney(-repairCost);
            fixing = true;
            StartCoroutine("StartRepairing");
        }
    }

    public void setCosts(double maintCost, double repCost) {
        maintenanceCost = maintCost;
        repairCost = repCost;
        revenue = (double) serviceArea.GetComponent<servicearea>().getResidents()/1000 * Mathf.Pow(1.2f, (float) -serviceArea.GetComponent<servicearea>().getBuiltCount());
    }

    public void setRepairCost(double repCost) {
        repairCost = repCost;
    }

    public double getMaintenanceCost() {
        if(building || broken) return 0;
        else return maintenanceCost;
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
        else {
            outputList.Add(gameObject);
        }
        foreach(GameObject station in connectedStations) {
            if(!outputList.Contains(station)) {
                outputList = station.GetComponent<servicestation>().ValidStationList(outputList);
            }
        }
        return outputList;
    }
}
