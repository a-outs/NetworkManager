using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateCost: MonoBehaviour
{
    public GameObject cable;
    public TextMeshProUGUI costText;

    private cablescript cableScript;
    private double currentCost;
    // Start is called before the first frame update
    void Start()
    {
        //mText = gameObject.GetComponent<TextMeshProUGUI>();

        cableScript = cable.GetComponent<cablescript>();
        currentCost = cableScript.currentCost;
    }

    // Update is called once per frame
    void Update()
    {
        currentCost = cableScript.currentCost;
        costText.text = currentCost.ToString();
    }

    //public void MoveObject()
    //{
    //    Vector3 pos = Input.mousePosition + offset;
    //    pos.z = basisObject.position.z;
    //    movingObject.position = cam.ScreenToWorldPoint(pos);
    //}
}
