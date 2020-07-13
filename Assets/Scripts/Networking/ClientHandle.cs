using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        float _seed = _packet.ReadFloat();
        float _noiseScale = _packet.ReadFloat();
        float _multiplier = _packet.ReadFloat();
        int _myId = _packet.ReadInt();

        TerrainSettings.instance.seed = _seed;
        TerrainSettings.instance.noiseScale = _noiseScale;
        TerrainSettings.instance.multiplier = _multiplier;
        //TerrainSettings.instance.Apply();
        Debug.Log("Seed: " + _seed);
        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        // Now that we have the client's id, connect UDP
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            _player.destPos = _position;
        }
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            _player.transform.rotation = _rotation;
        }
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }

    public static void SpawnEntity(Packet _packet)
    {
        Debug.Log("Spawn entity");
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();
        string _modelId = _packet.ReadString();
        int _parentId = _packet.ReadInt();
        Debug.Log("new entity");

        GameManager.instance.SpawnNewEntity(_id, _position, _rotation, _modelId, _parentId);
    }

    public static void EntityPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();
        string _ad = _packet.ReadString();

        GameManager.entities[_id].SetTargets(_position, _rotation, _ad);
    }

    public static void KillEntity(Packet _packet)
    {
        int _id = _packet.ReadInt();
        GameManager.instance.KillEntity(_id);
    }

    public static void ChunkMod(Packet _packet)
    {
        int _type = _packet.ReadInt(); //Chunk Mod type. 0 - add 1 - remove
        Vector3 _pos = _packet.ReadVector3();
        int _id = _packet.ReadInt();
        int _modelId = _packet.ReadInt();

        GameManager.instance.ModChunk(new ChunkMod((ChunkMod.ChunkModType)_type, _pos, _id, _modelId));
    }

    public static void Time(Packet _packet)
    {
        float _time = _packet.ReadFloat();
        float _clouds = _packet.ReadFloat();

        TimeManager.instance.SetWorldTime(_time, _clouds);
    }

    public static void PlayerInfo(Packet _packet)
    {
        int _player = _packet.ReadInt();
        string _toolName = _packet.ReadString();

        GameManager.players[_player].GetComponent<PlayerInventory>().SelectItem(_toolName);
    }

    public static void OpenGUI(Packet _packet)
    {
        string _s = _packet.ReadString();
        bool _open = _packet.ReadBool();
        if (_open)
        {
            UIManager.instance.ShowGUI(_s);
        }
        else
        {
            UIManager.instance.HideGUI(_s);
        }
    }

    public static void LoadInventory(Packet _packet)
    {
        int length = _packet.ReadInt();
        ItemStack[] stacks = new ItemStack[length];
        for (int i = 0; i < length; i++)
        {
            string itemName = _packet.ReadString();
            int count = _packet.ReadInt();
            //PlayerManager p = GameManager.players[Client.instance.myId];
            if (itemName != "none")
            {
                stacks[i] = new ItemStack(GameManager.instance.GetItem(itemName), count);
            }
            else
            {
                stacks[i] = null;
            }
        }

        UIManager.instance.gameObject.GetComponent<InventoryRenderer>().RenderInventory(stacks);
    }

}
