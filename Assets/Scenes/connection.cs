using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
public class connection : MonoBehaviourPunCallbacks
{
    public GameObject screen;
    public InputField ip;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void connectclick()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        print("connect to the server");
        PhotonNetwork.JoinLobby();
        screen.SetActive(true);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("disconnect to the server"+cause);
    }
    public override void OnJoinedLobby()
    {
        print("Join the lobby");
    }
    public override void OnLeftLobby()
    {
        print("left the lobby");
    }
    public void btncreateroom()
    {
        PhotonNetwork.CreateRoom(ip.text,new RoomOptions { MaxPlayers=2,IsVisible=true,IsOpen=true});
    }
    public override void OnCreatedRoom()
    {
        print("created room successfull");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("room create failed"+message);
    }
    public void joinrooms()
    {
        PhotonNetwork.JoinRoom(ip.text);
       
    }
    public override void OnJoinedRoom()
    {
        print("joined the room");
        if (PhotonNetwork.CountOfPlayersInRooms == 0)
        {
            PhotonNetwork.NickName = "Player A";
        }
        else
        {
            PhotonNetwork.NickName = "Player B";
        }
        PhotonNetwork.LoadLevel("play");

    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("room join failed" + message);
    }
}
