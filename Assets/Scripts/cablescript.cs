using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class cablescript : MonoBehaviour
{
    private double length;
    private LineRenderer lineRenderer;

    [SerializeField]
    private GameObject node;
    private GameObject lastStationPlaced;
    
    private bool firstTimePlacing = true;
    private Ray ray;
    private RaycastHit2D hit;
    private Vector3 snappedPos;
    
    public bool buildmode;
    public List<GameObject> nodes;
    public double currentCost;

    public gamemanager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.alignment = LineAlignment.TransformZ;
        lineRenderer.positionCount++;
        length = 0;
        buildmode = true;
        
        //CreateNode(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (buildmode)
        {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;

            GameObject objectUnderMouse = GetObjectUnderMouse();
            if (objectUnderMouse)
            {
                PlaceNewStation(objectUnderMouse);
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, snappedPos); // Snapped
            }
            else
            {
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, target); // Free flow
            }

            UpdateCableCost(target);

        } else {
            // Resize the list so that the last element would be automatically deleted
            int nodeSize = nodes.Count;
            lineRenderer.positionCount = nodeSize;

            // On exit, firstTimePlacing needs to be reset because
            // the next time it enters buildmode it will be as the same thing as
            // building for the first time
            firstTimePlacing = true;
        }
    }

    void ConnectStations(GameObject objectUnderMouse)
    {
        if (firstTimePlacing)
        {
            lastStationPlaced = objectUnderMouse;
        }
        else
        {
            lastStationPlaced.GetComponent<servicestation>().addConnection(objectUnderMouse);
            objectUnderMouse.GetComponent<servicestation>().addConnection(lastStationPlaced);
        }
    }

    void PlaceNewStation(GameObject objectUnderMouse)
    {
        snappedPos = objectUnderMouse.transform.position;
        if (Input.GetMouseButtonDown(0))
        {
            // Connect the current station with the last station,
            // and update the current station as the lastStationPlaced
            ConnectStations(objectUnderMouse);
            lastStationPlaced = objectUnderMouse;

            // Use the center of the object under the cursor as the position for the node
            CreateNode(snappedPos);

            // When node placed, get cost and subtract this amount from wallet
            double cost = GetCost(snappedPos);

            firstTimePlacing = false;
        }
    }

    void CreateNode(Vector3 position) {
        GameObject tempNode = Instantiate(node, position, transform.rotation);
        tempNode.transform.SetParent(gameObject.transform);

        nodes.Add(tempNode);
        lineRenderer.positionCount++;
        //lineRenderer.SetPosition(lineRenderer.positionCount - 2, position);
    }

    double GetCost(Vector3 targetPos)
    {
        int size = nodes.Count;
        GameObject latestNode = nodes[size - 1];
        Vector3 latestNodePos = latestNode.transform.position;

        // Calculate distance between current target and last node
        float dist = Vector3.Distance(latestNodePos, targetPos);
        // Get Absolute value of dist
        dist = Mathf.Abs(dist);

        double cost = (double)dist;
        return cost;
    }

    void UpdateCableCost(Vector3 target)
    {
        if (!firstTimePlacing)
        {
            currentCost = GetCost(target);
        }
    }

    GameObject GetObjectUnderMouse()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (hit)
        {
            if (hit.collider.CompareTag("ServiceStation"))
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }
}
