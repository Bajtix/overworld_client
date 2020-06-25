using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter_Container : MonoBehaviour
{
    private void Update()
    {
        if (GetComponent<Entity>().additionalData == null || GetComponent<Entity>().additionalData == "") 
        {
            GetComponent<LineRenderer>().SetPosition(0, transform.position);
            GetComponent<LineRenderer>().SetPosition(1, transform.position);
            return;
        }
        Vector3 helicopter = GameManager.entities[int.Parse(GetComponent<Entity>().additionalData)].transform.position;

        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        GetComponent<LineRenderer>().SetPosition(1, helicopter);
    }
}
