using UnityEngine;

public class InfoBox : MonoBehaviour
{
    public GameObject infoBox;
    public bool visible = false;

    public void ShowInfo(string info)
    {
        if (info == "")
        {
            Hide();
            return;
        }
        infoBox.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = info;
        if (!visible)
        {
            infoBox.SetActive(true);
            LeanTween.scale(infoBox, new Vector3(1, 1), 0.2f);
            
            visible = true;
        }
    }

    public void Hide()
    {
        
        LeanTween.scale(infoBox, new Vector3(0, 0), 0.2f).setOnComplete(() => { infoBox.SetActive(false); visible = false; });
    }
}
