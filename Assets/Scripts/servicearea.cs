using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class servicearea : MonoBehaviour
{
    [SerializeField]
    private int residents;

    public List<GameObject> serviceStations;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addServiceStation(GameObject serviceStation) {
        serviceStations.Add(serviceStation);
    }

    // function to return how much money this service area should give per unit of time
    public double returnPerTime() {
        return (double) (residents * serviceStations.Count);
    }
}
