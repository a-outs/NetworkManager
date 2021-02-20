using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cablescript : MonoBehaviour
{

    private double length;

    private LineRenderer lineRenderer;

    [SerializeField]
    private GameObject node;
    
    public bool buildmode;

    public List<GameObject> nodes;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount++;
        length = 0;
        buildmode = true;
        CreateNode(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (buildmode) {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, target);
            if(Input.GetMouseButtonDown(0)) {
                CreateNode(target);
            }
        }
    }

    void CreateNode(Vector3 position) {
        GameObject tempNode = Instantiate(node, position, transform.rotation);
        tempNode.transform.SetParent(gameObject.transform);
        nodes.Add(tempNode);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 2, position);
    }
}
