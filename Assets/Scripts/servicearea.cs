using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Experimental.U2D;

public class servicearea : MonoBehaviour
{
    [SerializeField]
    private int residents;

    public int totalResidents;

    [SerializeField]
    private string name;

    private float transitionLength;

    public List<GameObject> serviceStations;

    private GameObject stats;
    private GameObject statsText;
    private GameObject detailedStats;
    private GameObject detailedStatsText;

    private gamemanager gameManager;

    private Color defaultColor;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<gamemanager>();

        stats = gameObject.transform.Find("Canvas").Find("Stats").gameObject;
        statsText = stats.transform.Find("StatsText").gameObject;
        statsText.GetComponent<TextMeshProUGUI>().text = name;
        detailedStats = gameObject.transform.Find("Canvas").Find("DetailedStats").gameObject;
        detailedStatsText = detailedStats.transform.Find("DetailedStatsText").gameObject;
        detailedStats.transform.DOScale(0,0);

        transitionLength = gameManager.hoverOverLength;

        defaultColor = new Color(0.4f, 0.55f, 1f,((float) residents/(float) totalResidents)*4f);
        GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = defaultColor;
    }

    // Update is called once per frame
    void Update()
    {
        detailedStatsText.GetComponent<TextMeshProUGUI>().text = "Residents: " + residents + /*"\nCost: (-)" + costPerTime() + "\nRevenue: " + returnPerTime() +*/ "\nProfit: <b>" + (returnPerTime() - costPerTime()).ToString("F2") + "</b>";
    }

    void OnMouseEnter() {
        stats.transform.DOScale(1,0);
        stats.transform.DOScale(1.5f,transitionLength);
        detailedStats.transform.DOScale(0,0);
        detailedStats.transform.DOScale(1.5f,transitionLength);
        Color newColor = defaultColor;
        newColor.a = 1f;
        GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = newColor;
    }

    void OnMouseExit() {
        stats.transform.DOScale(1.5f,0);
        stats.transform.DOScale(1,transitionLength);
        detailedStats.transform.DOScale(1.5f,0);
        detailedStats.transform.DOScale(0f,transitionLength);
        GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = defaultColor;
    }

    public void addServiceStation(GameObject serviceStation) {
        serviceStations.Add(serviceStation);
    }

    // function to return how much money this service area should give per unit of time
    public double returnPerTime() {
        double revenue = 0;
        foreach(GameObject serviceStation in serviceStations) {
            if(serviceStation.GetComponent<servicestation>().isActive()) revenue += serviceStation.GetComponent<servicestation>().revenue;
        }
        return revenue;
    }

    private double costPerTime() {
        double moneyPerTime = 0;
        foreach (GameObject serviceStation in serviceStations) {
            moneyPerTime += serviceStation.GetComponent<servicestation>().getMaintenanceCost();
        }
        return (double) (moneyPerTime); 
    }

    public int getResidents() { return residents; }

    public int getBuiltCount() { return serviceStations.Count; }
}
