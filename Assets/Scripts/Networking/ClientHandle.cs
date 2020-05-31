using System.Collections;
using System.Collections.Generic;
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
        int _modelId = _packet.ReadInt();

        GameManager.instance.SpawnNewEntity(_id, _position,_modelId);
    }

    public static void EntityPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();
        string _ad = _packet.ReadString();

        GameManager.entities[_id].SetTargets(_position, _rotation,_ad);
    }

    
}
