using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StormBehavior : MonoBehaviour
{
    public GameObject stormVisual;

    private gamemanager gameManager;
    private List<GameObject> serviceStations;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = this.GetComponent<gamemanager>();

    }

    // Update is called once per frame
    void Update()
    {
        serviceStations = gameManager.serviceStations;
        CheckForStormCreation();
    }

    void CheckForStormCreation()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            CreateStorm();
        }
    }

    void CreateStorm()
    {
        Vector2 providerPos = new Vector2(0, 0);
        Collider2D[] objectsInRadius = null;
        // Need to check for this so that storms won't be triggered
        // When there is only one serviceStation object
        if (serviceStations.Count >= 1)
        {
            // Get a random service provider out of the total list
            int randomIndex = Random.Range(0, serviceStations.Count - 1);
            //int randomIndex = 1;

            providerPos = serviceStations[randomIndex].transform.position;

            // Set the radius of the affected area
            float stormRadius = Random.Range(6, 9);

            // Get all objects within 
            objectsInRadius = Physics2D.OverlapCircleAll(providerPos, stormRadius);

        }

        if (objectsInRadius != null)
        {
            // Add only colliders that are service providers
            List<GameObject> providerList = new List<GameObject>();
            foreach (Collider2D collider in objectsInRadius)
            {
                if (collider.gameObject.CompareTag("ServiceStation"))
                {
                    providerList.Add(collider.gameObject);
                }
            }
            foreach (GameObject provider in providerList)
            {
                int chanceInt = Random.Range(0, 4);
                // 20% chance of breaking
                if (chanceInt == 0)
                {
                    if (provider.GetComponent<servicestation>().building == false)
                    {
                        provider.GetComponent<servicestation>().broken = true;
                    }
                }
            }
        }
        StartCoroutine("DisplayStormEffect");
    }

    IEnumerator DisplayStormEffect()
    {
        stormVisual.SetActive(true);
        stormVisual.GetComponent<Image>().DOFade(.7f, 1f);
        yield return new WaitForSeconds(1f);
        stormVisual.GetComponent<Image>().DOFade(0, 1f);
        yield return new WaitForSeconds(1f);
        stormVisual.SetActive(false);
        yield break;
    }
}
