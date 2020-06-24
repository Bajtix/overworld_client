using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.L))
            gameObject.SetActive(false);
    }
}
