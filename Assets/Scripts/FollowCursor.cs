using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor: MonoBehaviour
{
    public Vector3 offset;
    //public int currentCost;

    //private TextMeshProUGUI mText;
    // Start is called before the first frame update
    void Start()
    {
        //mText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition + offset;
        gameObject.transform.position = mousePos + offset;

    }

    //public void MoveObject()
    //{
    //    Vector3 pos = Input.mousePosition + offset;
    //    pos.z = basisObject.position.z;
    //    movingObject.position = cam.ScreenToWorldPoint(pos);
    //}
}
