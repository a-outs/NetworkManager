using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class servicearea : MonoBehaviour
{
    [SerializeField]
    private int residents;

    [SerializeField]
    private string name;

    private float transitionLength;

    public List<GameObject> serviceStations;

    private GameObject statsText;
    private GameObject detailedStats;
    private GameObject detailedStatsText;

    private gamemanager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<gamemanager>();

        statsText = gameObject.transform.Find("Canvas").Find("Stats").gameObject;
        statsText.GetComponent<TextMeshProUGUI>().text = name;
        detailedStats = gameObject.transform.Find("Canvas").Find("DetailedStats").gameObject;
        detailedStatsText = detailedStats.transform.Find("DetailedStatsText").gameObject;
        detailedStats.transform.DOScale(0,0);

        transitionLength = gameManager.hoverOverLength;
    }

    // Update is called once per frame
    void Update()
    {
        detailedStatsText.GetComponent<TextMeshProUGUI>().text = "Residents: " + residents + /*"\nCost: (-)" + costPerTime() + "\nRevenue: " + returnPerTime() +*/ "\nProfit: <b>" + (returnPerTime() - costPerTime()) + "</b>";
    }

    void OnMouseEnter() {
        statsText.transform.DOScale(1,0);
        statsText.transform.DOScale(1.5f,transitionLength);
        detailedStats.transform.DOScale(0,0);
        detailedStats.transform.DOScale(1.5f,transitionLength);
    }

    void OnMouseExit() {
        statsText.transform.DOScale(1.5f,0);
        statsText.transform.DOScale(1,transitionLength);
        detailedStats.transform.DOScale(1.5f,0);
        detailedStats.transform.DOScale(0f,transitionLength);
    }

    public void addServiceStation(GameObject serviceStation) {
        serviceStations.Add(serviceStation);
    }

    // function to return how much money this service area should give per unit of time
    public double returnPerTime() {
        int serviceStationCount = 0;
        foreach(GameObject serviceStation in serviceStations) {
            if(serviceStation.GetComponent<servicestation>().isActive()) serviceStationCount++;
        }
        return (double) (residents * serviceStationCount);
    }

    private double costPerTime() {
        return (double) (serviceStations.Count * gameManager.serviceStationMaintenance); 
    }

    public double profitForStation(GameObject station) {
        return (double) (residents);
    }
}
