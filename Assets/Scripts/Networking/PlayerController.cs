using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform camTransform;
    //public Transform handTarget;
    public bool f1 = false;
    private void Start()
    {
        //camTransform = transform.Find("Camera");
        UIManager.instance.loading = false;
    }

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.F1))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;

            f1 = !f1;

            UIManager.instance.cmd.consolePanel.SetActive(f1);
        }

        if (f1) return;
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Client.instance.Disconnect();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        

        if (Cursor.lockState == CursorLockMode.Locked)
        {

            if (Input.GetKeyDown(KeyCode.E))
                ClientSend.Interact(camTransform, KeyCode.E);

            if (Input.GetKeyDown(KeyCode.Mouse0))
                ClientSend.Interact(camTransform, KeyCode.Mouse0);

            if (Input.GetKeyDown(KeyCode.Mouse1))
                ClientSend.Interact(camTransform, KeyCode.Mouse1);

            if (Input.GetKeyDown(KeyCode.R))
                ClientSend.Interact(camTransform, KeyCode.R);

            if(Input.GetKeyDown(KeyCode.I))
                ClientSend.InventoryRequest();

            if (Input.GetKeyDown(KeyCode.Q))
                ClientSend.Interact(camTransform, KeyCode.Q);

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
                ClientSend.Interact(camTransform, KeyCode.PageUp);

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
                ClientSend.Interact(camTransform, KeyCode.PageDown);
        }

        if (Input.GetKeyDown(KeyCode.I) && Cursor.lockState != CursorLockMode.Locked)
        {
            UIManager.instance.GetComponent<InventoryRenderer>().HideInventory();
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (UIManager.instance.loadMsg.activeInHierarchy)
        {
            if(Physics.Raycast(transform.position,new Vector3(0,-1,0),2))
            {
                UIManager.instance.EndLoading();
            }
        }
        
        //Debug.Log($"FRAME DEBUGGER: Running at {1/Time.deltaTime} FPS");
        
    }

    private void FixedUpdate()
    {
        ChunkManagement();      
        if(!f1)
        SendInputToServer();
    }

    private void ChunkManagement()
    {
        var chunkPos = ChunkManager.ChunkAt(transform.position.x, transform.position.z);

        for (int i = -TerrainSettings.instance.renderDistance; i < TerrainSettings.instance.renderDistance; i++)
        {
            for (int j = -TerrainSettings.instance.renderDistance; j < TerrainSettings.instance.renderDistance; j++)
            {
                ChunkManager.instance.AddChunk(chunkPos.x + i, chunkPos.y + j);
            }
        }


        for (int i = -TerrainSettings.instance.renderDistance; i < TerrainSettings.instance.renderDistance; i++)
        {
            ChunkManager.instance.RemoveChunk(chunkPos.x + i, chunkPos.y + TerrainSettings.instance.renderDistance + 1);
        }

        for (int i = -TerrainSettings.instance.renderDistance; i < TerrainSettings.instance.renderDistance; i++)
        {
            ChunkManager.instance.RemoveChunk(chunkPos.x + i, chunkPos.y - TerrainSettings.instance.renderDistance - 1);
        }

        for (int i = -TerrainSettings.instance.renderDistance - 1; i < TerrainSettings.instance.renderDistance + 1; i++)
        {
            ChunkManager.instance.RemoveChunk(chunkPos.x + -TerrainSettings.instance.renderDistance - 1, chunkPos.y + i);
        }

        for (int i = -TerrainSettings.instance.renderDistance - 1; i < TerrainSettings.instance.renderDistance + 1; i++)
        {
            ChunkManager.instance.RemoveChunk(chunkPos.x + TerrainSettings.instance.renderDistance + 1, chunkPos.y + i);
        }
    }

    /// <summary>Sends player input to the server.</summary>
    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.Space),
            false
        };

        ClientSend.PlayerMovement(_inputs);
    }
}
