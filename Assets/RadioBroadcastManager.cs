using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioBroadcastManager : MonoBehaviour
{

    #region Singleton
    public static RadioBroadcastManager instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }
    #endregion

    public RadioStation[] stations;
}
