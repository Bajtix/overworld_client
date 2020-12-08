using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    /// <summary>Sends a packet to the server via TCP.</summary>
    /// <param name="_packet">The packet to send to the sever.</param>
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    /// <summary>Sends a packet to the server via UDP.</summary>
    /// <param name="_packet">The packet to send to the sever.</param>
    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    /// <summary>Lets the server know that the welcome message was received.</summary>
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.usernameField.text);

            SendTCPData(_packet);
        }
    }

    /// <summary>Sends player input to the server.</summary>
    /// <param name="_inputs"></param>
    public static void PlayerMovement(bool[] _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach (bool _input in _inputs)
            {
                _packet.Write(_input);
            }
            _packet.Write(GameManager.players[Client.instance.myId].transform.rotation);

            SendUDPData(_packet);
        }
    }

    public static void Interact(Transform look, KeyCode c) // send an interaction message containing full look rot.
    {
        using (Packet _packet = new Packet((int)ClientPackets.interact)) // TODO: change packet type to its own
        {
            _packet.Write(look.rotation);
            _packet.Write((int)c);
            SendTCPData(_packet);
        }
    }

    public static void SpawnCar(Vector3 car)
    {
        using (Packet _packet = new Packet((int)ClientPackets.spawnCar))
        {
            _packet.Write(car);

            SendTCPData(_packet);
        }
    }

    public static void SendMenuReply(int i,string menuName)
    {
        using (Packet _packet = new Packet((int)ClientPackets.menuResponse))
        {
            _packet.Write(menuName);
            _packet.Write(i);

            SendTCPData(_packet);
        }
    }

    public static void InventoryRequest()
    {
        using (Packet _packet = new Packet((int)ClientPackets.invReq))
        {
            _packet.Write(true);
            SendTCPData(_packet);
        }
    }

    public static void ItemTransfer(int from, int to)
    {
        using (Packet _packet = new Packet((int)ClientPackets.invMod))
        {
            _packet.Write(from);
            _packet.Write(to);
            SendTCPData(_packet);
        }
    }

    public static void ConsoleCommand(string cmd)
    {
        using (Packet _packet = new Packet((int)ClientPackets.consoleCmd))
        {
            _packet.Write(cmd);
            SendTCPData(_packet);
        }
    }

    
    #endregion
}
