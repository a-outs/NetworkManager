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
    private Ray ray;
    private RaycastHit2D hit;
    private Vector3 snappedPos;

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

            //if (Input.GetMouseButtonDown(0))
            //{
            //    GameObject objectUnderMouse = GetObjectUnderMouse();
            //    if (objectUnderMouse)
            //    {
                    
            //        // Use the center of the object under the cursor as the position for the node
            //        snappedPos = objectUnderMouse.transform.position;
            //        CreateNode(snappedPos);

            //        // When node placed, get cost and subtract this amount from wallet
            //        double cost = GetCost(snappedPos);
            //        firstPlaced = true;
            //    }
            //}

            GameObject objectUnderMouse = GetObjectUnderMouse();
            if (objectUnderMouse)
            {
                snappedPos = objectUnderMouse.transform.position;
                if (Input.GetMouseButtonDown(0))
                {
                    // Use the center of the object under the cursor as the position for the node
                    CreateNode(snappedPos);

                    // When node placed, get cost and subtract this amount from wallet
                    double cost = GetCost(snappedPos);
                    firstPlaced = true;      
                }
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, snappedPos); // Snapped
            }
            else
            {
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, target); // Real time
            }

            //lineRenderer.SetPosition(lineRenderer.positionCount - 1, target); // Real time
            //lineRenderer.SetPosition(lineRenderer.positionCount - 1, snappedPos); // Snapped

            if (firstPlaced)
            {
                // For displaying cost as the mouse cover moves
                //Debug.Log(GetCost(target));
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

    GameObject GetObjectUnderMouse()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (hit)
        {
            if (hit.collider.CompareTag("ServiceProvider"))
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }
}
