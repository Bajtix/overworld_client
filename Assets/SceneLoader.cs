using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int sceneid;

    public CanvasGroup fader;

    public TMPro.TextMeshProUGUI progress;

    private AsyncOperation loadTask;

    private void Start()
    {
        LeanTween.alphaCanvas(fader, 0, 1).setOnComplete(()=> {
            loadTask = SceneManager.LoadSceneAsync(sceneid);
            loadTask.allowSceneActivation = false;
        });
        

    }

    private void FixedUpdate()
    {
        if (loadTask == null)
        {
            progress.text = "Waiting...";
            return;
        }
        else
            progress.text = $"{loadTask.progress * 100:00.0}%";

        if(loadTask.progress >= 0.9f)
            LeanTween.alphaCanvas(fader, 1, 1).setOnComplete(() => { LeanTween.delayedCall(1, () => loadTask.allowSceneActivation = true); });
    }
}
