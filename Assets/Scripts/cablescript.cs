using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cablescript : MonoBehaviour
{
    private double length;
    private LineRenderer lineRenderer;

    [SerializeField]
    private GameObject node;

    private bool firstPlaced = false;
    
    public bool buildmode;
    public List<GameObject> nodes;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
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

            if (Input.GetMouseButtonDown(0))
            {
                // Check if the cursor is over a service provider object before placing
                CreateNode(target);

                // When node placed, get cost and subtract this amount from wallet
                int cost = GetCost(target);

                firstPlaced = true;
            }

            lineRenderer.SetPosition(lineRenderer.positionCount - 1, target);

            if (firstPlaced)
            {
                // For displaying cost as the mouse cover moves
                Debug.Log(GetCost(target));
            }

        } else {
            // Resize the list so that the last element would be automatically deleted
            int nodeSize = nodes.Count;
            lineRenderer.positionCount = nodeSize;

        }
    }

    void CreateNode(Vector3 position) {
        GameObject tempNode = Instantiate(node, position, transform.rotation);
        tempNode.transform.SetParent(gameObject.transform);

        nodes.Add(tempNode);
        lineRenderer.positionCount++;
        //lineRenderer.SetPosition(lineRenderer.positionCount - 2, position);
    }

    int GetCost(Vector3 targetPos)
    {
        int size = nodes.Count;
        GameObject latestNode = nodes[size - 1];
        Vector3 latestNodePos = latestNode.transform.position;

        // Calculate distance between current target and last node
        float dist = Vector3.Distance(latestNodePos, targetPos);
        // Get Absolute value of dist
        dist = Mathf.Abs(dist);

        int cost = (int)dist;
        return cost;
    }

    
}
