using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject[] menus;

    private void Start()
    {
        LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 1, 1).setDelay(1);
    }
    public void ShowMenu(int index)
    {
        if (!menus[index].activeSelf)
        {
            foreach (GameObject g in menus)
            {
                LeanTween.scale(g, new Vector3(1, 0.1f, 1), 0f);
                g.SetActive(false);
            }

            menus[index].SetActive(true);
            LeanTween.scale(menus[index], new Vector3(1, 1, 1), 0.2f);
        }
        else
        {
            LeanTween.scale(menus[index], new Vector3(1, 0.1f, 1), 0.1f).setOnComplete(() =>
                menus[index].SetActive(false));
        }
    }
}
