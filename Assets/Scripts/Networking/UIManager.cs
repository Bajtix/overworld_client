using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{

    [System.Serializable]
    public class DIRGUI : SerializableDictionaryBase<string, GameObject> { }
    

    public static UIManager instance;

    public GameObject startMenu;
    public GameObject loadscreen;
    public GameObject loadMsg;
    public GameObject loadingTerrain;

    public TMPro.TextMeshProUGUI version;
    public InputField usernameField;
    public InputField ipField;

    public TMPro.TextMeshProUGUI renderDistanceLbl;
    public TMPro.TextMeshProUGUI senseLbl;

    private float loadingCountdown;
    public bool loading;
    public float Sensitivity = 100f;

    public TMPro.TMP_Dropdown resDropdown;
    public TMPro.TMP_Dropdown qualityDropdown;

    private Resolution[] resolutions;

    public DIRGUI guis;
    public InfoBox infoBox;

    public AudioMixer settings;
    public AnimationCurve volumeProgression;

    public Console cmd;
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

        version.text = "v" + Application.version;
    }
    private void Start()
    {
        SetRenderDistance(3);
        resolutions = Screen.resolutions;
        

        List<TMPro.TMP_Dropdown.OptionData> l = new List<TMPro.TMP_Dropdown.OptionData>();
        foreach(Resolution r in resolutions)
        {
            l.Add(new TMPro.TMP_Dropdown.OptionData(r.width + " x " + r.height));
        }
        resDropdown.AddOptions(l);
        List<TMPro.TMP_Dropdown.OptionData> w = new List<TMPro.TMP_Dropdown.OptionData>();
        
        foreach(string s in QualitySettings.names)
        {
            w.Add(new TMPro.TMP_Dropdown.OptionData(s));
        }
        if(qualityDropdown.options != null)
        qualityDropdown.options.Clear();
        qualityDropdown.AddOptions(w);

        qualityDropdown.value = QualitySettings.GetQualityLevel();
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
        Cursor.lockState = CursorLockMode.Locked;

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

    public void ShowGUI(string name)
    {
        Debug.Log("open menu " + name);
        Cursor.lockState = CursorLockMode.Confined;
        guis[name].SetActive(true);
        Debug.Log("opened menu");
    }

    public void HideGUI(string name)
    {
        Cursor.lockState = CursorLockMode.Locked;
        guis[name].SetActive(false);
    }

    public void SetSense(float s)
    {
        Sensitivity = s;
        senseLbl.text = "Sensivity: " + s/200;
    }

    public void SetRes(int i)
    {
        Screen.SetResolution(resolutions[i].width, resolutions[i].height, false);
    }

    public void SetVolume(float i)
    {
        settings.SetFloat("Volume", i * volumeProgression.Evaluate(i));
    }

    public void ToggleFS()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
