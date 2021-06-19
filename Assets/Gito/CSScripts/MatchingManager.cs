using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using static Niwatori.Utility;

namespace Niwatori
{
    public class MatchingManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Text messageText;
        [SerializeField] private Transform spawnArea;
        private const string gameVersion = "Ver1.0";

        private string roomName = "Room1";

        [SerializeField] private AudioClip kirarin;

        private void Start()
        {
            PhotonNetwork.GameVersion = gameVersion;
            Delay(2.0f, () =>
            {
                PhotonNetwork.ConnectUsingSettings();
            });
        }

        private void DisplayMessageAndGoTitle(string message)
        {
            messageText.text = message;
            Delay(2.0f, () =>
            {
                PhotonNetwork.Disconnect();
                SceneManager.LoadScene("Title");
            });
        }

        public override void OnConnectedToMaster()
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = (byte)GameSettings.maxPlayer;
            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            DisplayMessageAndGoTitle("ルームの作成に失敗しました。\nもう一度お試しください。");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            if (roomName == "Room1")
            {
                roomName = "Room2";
                var roomOptions = new RoomOptions();
                roomOptions.MaxPlayers = (byte)GameSettings.maxPlayer;
                PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions(), TypedLobby.Default);
            }
            else
            {
                DisplayMessageAndGoTitle("現在満員です。申し訳ありませんが、ちょっと待ってから再度お試しください。");
            }
        }

        public override void OnJoinedRoom()
        {
            messageText.text = "ルームへの接続に成功しました！";
            Vector3 position = new Vector3(
                Random.Range(spawnArea.transform.position.x - spawnArea.transform.localScale.x / 2f, spawnArea.transform.position.x + spawnArea.transform.localScale.x / 2f),
                0f,
                Random.Range(spawnArea.transform.position.z - spawnArea.transform.localScale.z / 2f, spawnArea.transform.position.z + spawnArea.transform.localScale.z / 2f));
            IChicken chicken = PhotonNetwork.Instantiate("Chicken", position, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f)).GetComponent<IChicken>();
            PlayerController playerController = chicken.GetMineGameObject().AddComponent<PlayerController>();
            Delay(2.0f, () =>
            {
                chicken.SetIsEnable(true);
                Fader.StartFadeIn(0f, 0.5f);
                messageText.text = string.Format("プレイヤーを待っています。\n{0:d} / {1:d}", PhotonNetwork.CurrentRoom.PlayerCount, GameSettings.maxPlayer);
                if ((int)PhotonNetwork.CurrentRoom.PlayerCount == GameSettings.maxPlayer)
                {
                    MatchingSuccess();
                }
            });
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            messageText.text = string.Format("プレイヤーを待っています。\n{0:d} / {1:d}", PhotonNetwork.CurrentRoom.PlayerCount, GameSettings.maxPlayer);
            if ((int)PhotonNetwork.CurrentRoom.PlayerCount == GameSettings.maxPlayer)
            {
                MatchingSuccess();
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            messageText.text = string.Format("プレイヤーを待っています。\n{0:d} / {1:d}", PhotonNetwork.CurrentRoom.PlayerCount, GameSettings.maxPlayer);
        }

        private void MatchingSuccess()
        {
            AudioManager.PlayOneShot(kirarin);
            messageText.text = string.Format("プレイヤーが揃いました。間も無くゲーム開始です！\n{0:d} / {1:d}", PhotonNetwork.CurrentRoom.PlayerCount, GameSettings.maxPlayer);
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.SetStartTime(unchecked(PhotonNetwork.ServerTimestamp + 6000));
                PhotonNetwork.CurrentRoom.SetNumberOfPlayers(PhotonNetwork.CurrentRoom.PlayerCount);
                PhotonNetwork.CurrentRoom.InitTeamDeath();
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    PhotonNetwork.PlayerList[i].SetPlayerNumber(i);
                    PhotonNetwork.PlayerList[i].SetTeamNumber(Mathf.FloorToInt(i / 2f));
                }
            }
            Delay(3.0f, () =>
            {
                Delay(Mathf.Abs(unchecked(PhotonNetwork.ServerTimestamp - (PhotonNetwork.CurrentRoom.TryGetStartTime(out int t) ? t : 0)) / 1000f), () =>
                {
                    Fader.StartFadeOut(0f, 0.5f, () =>
                    {
                        PhotonNetwork.IsMessageQueueRunning = false;
                        SceneManager.LoadScene("MainSmall");
                    });
                });
            });

        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            if (cause != DisconnectCause.DisconnectByClientLogic)
            {
                DisplayMessageAndGoTitle("接続に失敗しました。\nもう一度お試しください。");
            }
        }
    }
}