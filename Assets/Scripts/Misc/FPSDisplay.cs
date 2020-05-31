using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private TMPro.TextMeshProUGUI tmp;

    void Start()
    {
        tmp = GetComponent<TMPro.TextMeshProUGUI>();
        StartCoroutine("FPSUpdate");
    }

    public IEnumerator FPSUpdate()
    {
        tmp.text = $"{(1 / Time.deltaTime).ToString("0.0")} FPS";
        yield return new WaitForSeconds(0.1f);
        StartCoroutine("FPSUpdate");
    }
}
