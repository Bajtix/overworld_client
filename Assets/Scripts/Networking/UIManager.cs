using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public GameObject loadscreen;
    public GameObject loadMsg;
    public GameObject loadingTerrain;

    public TMPro.TextMeshProUGUI version;
    public InputField usernameField;
    public InputField ipField;

    public TMPro.TextMeshProUGUI renderDistanceLbl;

    private float loadingCountdown;
    public bool loading;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

        version.text = Application.version;
    }

    private void Update()
    {
        if (loading)
            loadingCountdown += Time.deltaTime;

        if (loadingCountdown > 20)
        {
            Client.instance.Disconnect();
            Debug.Log("Timed out");
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    /// <summary>Attempts to connect to the server.</summary>
    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        Client.instance.ip = ipField.text;
        try
        {
            Client.instance.ConnectToServer();
        }
        catch
        {
            Client.instance.Disconnect();
            Debug.Log("Could not join " + ipField.text);
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        loadMsg.SetActive(true);
        loading = true;
    }

    public void EndLoading()
    {
        loadscreen.SetActive(false);
        loadingTerrain.SetActive(false);
    }

    public void ChangeGFXQuality(int i)
    {
        QualitySettings.SetQualityLevel(i);
        
        Debug.Log("Changed quality to " + QualitySettings.GetQualityLevel());
    }

    public void SetRenderDistance(float to)
    {
        TerrainSettings.instance.renderDistance = Mathf.RoundToInt(to);
        renderDistanceLbl.text = "Render distance: " + to;
        RenderSettings.fogDensity = 0.03f / to;
        Debug.Log("Changed Render distance to " + Mathf.RoundToInt(to));
    }
}
