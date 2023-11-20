using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnedPlayerPrefab;
    public GameObject PlayerRacket;
    public XRRayInteractor righthand;
    //private GameObject ballPrefab;
    //private GameObject spawnedRacket_Zwei_Prefab;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
       // spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", transform.position, transform.rotation);
       // PlayerRacket = PhotonNetwork.Instantiate("NetworkRacket", transform.position, transform.rotation);
       // righthand.startingSelectedInteractable = PlayerRacket.GetComponent<XRGrabNetworkInteractable>();
       // righthand.SelectRacket();


        //ballPrefab = PhotonNetwork.Instantiate ("Ball", new Vector3 (0,3,0), Quaternion.identity);
        //spawnedRacket_Zwei_Prefab = PhotonNetwork.Instantiate ("Racket(2)", transform.position, Quaternion.identity);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
        //PhotonNetwork.Destroy(ballPrefab);

    }
}
