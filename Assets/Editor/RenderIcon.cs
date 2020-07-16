using UnityEngine;
using UnityEditor;
using System.IO;

public class RenderIcon : EditorWindow
{

    GameObject toRender;
    Transform rt;
    Camera cam;
    [MenuItem("Tools/ItemRenderer")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(RenderIcon)).Show();
    }

    private void OnGUI()
    {
        toRender = (GameObject)EditorGUILayout.ObjectField("Object to render ",toRender,typeof(GameObject),false);

        rt = (Transform)EditorGUILayout.ObjectField("Transform ",rt,typeof(Transform),true);
        cam = (Camera)EditorGUILayout.ObjectField("Renderer",cam,typeof(Camera),true);
        if(GUILayout.Button("Render"))
        {
            GameObject g = GameObject.Instantiate(toRender, rt.position, rt.rotation);
            Texture2D tex = RTImage(cam);
            byte[] bytes = tex.EncodeToPNG();
            Directory.CreateDirectory(Application.dataPath + "/Renders");
            File.WriteAllBytes(Application.dataPath + "/Renders/"+toRender.name + ".png",bytes);
            DestroyImmediate(g);
        }
    }

    Texture2D RTImage(Camera camera)
    {

        RenderTexture t = new RenderTexture(512, 512, 24);
        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        camera.targetTexture = t;
        var currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;

        // Render the camera's view.
        camera.Render();

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();

        // Replace the original active Render Texture.
        RenderTexture.active = currentRT;
        return image;
    }
}
