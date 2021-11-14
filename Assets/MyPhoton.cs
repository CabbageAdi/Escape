using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;

public class MyPhoton : MonoBehaviourPunCallbacks
{ 
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, null);
    }

    public override void OnJoinedRoom()

    {
        GameObject newPlayer = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow = newPlayer.transform; 
    }
}
