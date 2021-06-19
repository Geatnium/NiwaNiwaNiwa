using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Niwatori.Utility;

namespace Niwatori
{
    public class TitleManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private InputField nameField;

        private int startTimeStamp;

        [SerializeField] private AudioClip kokekokko;

        private void Start()
        {
            Application.targetFrameRate = 30;
            PhotonNetwork.SendRate = 30;
            PhotonNetwork.SerializationRate = 2;
            PhotonNetwork.OfflineMode = true;
            PhotonNetwork.NickName = "";
            PhotonNetwork.LocalPlayer.SetPlayerNumber(0);
            PhotonNetwork.LocalPlayer.SetTeamNumber(0);
            PhotonNetwork.ConnectUsingSettings();
            Delay(6.0f, () =>
            {
                AudioManager.PlayOneShot(kokekokko);
            });
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinOrCreateRoom(Random.Range(0, int.MaxValue).ToString(), new RoomOptions(), TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            GetComponent<RandomSpawnChicken>().Spawn();
        }

        public void OnSinglePlay()
        {
            PhotonNetwork.NickName = nameField.text;
            PhotonNetwork.CurrentRoom.SetNumberOfPlayers(1);
            PhotonNetwork.CurrentRoom.InitTeamDeath();
            PhotonNetwork.CurrentRoom.SetStartTime(PhotonNetwork.ServerTimestamp);
            Fader.StartFadeOutAndLoadScene(0.5f, 1.0f, "MainSmall");
        }

        public void OnOnlinePlay()
        {
            Fader.StartFadeOut(0.5f, 1.0f, () =>
            {
                PhotonNetwork.NickName = nameField.text;
                PhotonNetwork.DestroyAll();
                PhotonNetwork.Disconnect();
            });
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            if (cause == DisconnectCause.DisconnectByClientLogic)
            {
                PhotonNetwork.OfflineMode = false;
                SceneManager.LoadScene("OnlineLobby");
            }
        }
    }
}
