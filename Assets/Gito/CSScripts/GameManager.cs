using UnityEngine;
using UnityEngine.UI;
using Bolt;
using Photon.Pun;
using Photon.Realtime;
using Niwatori;
using System.Collections;
using DG.Tweening;
using static Niwatori.CustomPropertiesTag;
using static Niwatori.Utility;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private int frameRate = 30;
    private GameObject playerChicken;
    [SerializeField] private Transform[] spawnPoints;
    private AIController[] aIControllers;
    [SerializeField] private Transform[] cameraPoints;
    [SerializeField] private RectTransform readyRT, startRT, finishRT;

    [SerializeField] private AudioClip buo;

    private IChicken[] chickens;

    private bool[] liveTeams = new bool[GameSettings.maxPlayer / 2];
    private string[] winnerPlayerName = new string[2];

    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        Application.targetFrameRate = frameRate;
        StartCoroutine(StartGameFlow());
    }

    private IEnumerator StartGameFlow()
    {
        int startTimeStamp = PhotonNetwork.CurrentRoom.TryGetStartTime(out int timeStamp) ? timeStamp : 0;
        int gameStartTimeStamp = unchecked(startTimeStamp + 10000);
        int animStartTimeStam = unchecked(startTimeStamp + 5000);
        GameObject playerChicken = PhotonNetwork.Instantiate("Chicken", spawnPoints[PhotonNetwork.LocalPlayer.GetPlayerNumber()].transform.position, spawnPoints[PhotonNetwork.LocalPlayer.GetPlayerNumber()].transform.rotation);
        playerChicken.AddComponent<PlayerController>();
        if (PhotonNetwork.IsMasterClient)
        {
            int numberOfPlayer = PhotonNetwork.CurrentRoom.GetNumberOfPlayers();
            aIControllers = new AIController[GameSettings.maxPlayer - numberOfPlayer];
            for (int i = numberOfPlayer; i < GameSettings.maxPlayer; i++)
            {
                GameObject aiChicken = PhotonNetwork.Instantiate("Chicken", spawnPoints[i].transform.position, spawnPoints[i].transform.rotation);
                aIControllers[i - numberOfPlayer] = aiChicken.AddComponent<AIController>();
                Debug.Log(Mathf.FloorToInt(i / 2f));
                aIControllers[i - numberOfPlayer].SetNumberAndTeamNumber(i, Mathf.FloorToInt(i / 2f));
                aIControllers[i - numberOfPlayer].SetNameLabel("AI " + aIControllers[i - numberOfPlayer].photonView.ViewID);
            }
        }
        yield return new WaitForSeconds(Mathf.Abs(unchecked(PhotonNetwork.ServerTimestamp - animStartTimeStam) / 1000f));
        Fader.StartFadeIn(0f, 0.3f);
        for (int i = 0; i < 8; i += 2)
        {
            AudioManager.PlayOneShot(buo);
            Camera.main.transform.DOMove(cameraPoints[i].position, 0f);
            Camera.main.transform.DORotate(cameraPoints[i].eulerAngles, 0f);
            Camera.main.transform.DOMove(cameraPoints[i + 1].position, 0.5f).SetEase(Ease.OutQuad);
            yield return new WaitForSeconds(0.5f);
        }
        Camera.main.transform.DOMove(cameraPoints[8].position, 0.5f).SetEase(Ease.OutQuad);
        Camera.main.transform.DORotate(cameraPoints[8].eulerAngles, 0.5f).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(1.0f);
        readyRT.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1.0f);
        readyRT.DOScale(Vector3.zero, 0.2f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.5f);
        startRT.DOScale(Vector3.one * 2f, 0.5f).SetEase(Ease.Linear);
        startRT.GetComponent<Text>().DOFade(0f, 0.5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.1f);
        playerChicken.GetComponent<IChicken>().SetIsEnable(true);
        GameObject[] chickenObjs = GameObject.FindGameObjectsWithTag("Chicken");
        chickens = new IChicken[chickenObjs.Length];
        for (int i = 0; i < chickenObjs.Length; i++)
        {
            chickens[i] = chickenObjs[i].GetComponent<IChicken>();
            chickens[i].DoFindPairChickenAndSet();
        }
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < aIControllers.Length; i++)
            {
                aIControllers[i].StartAI();
            }
        }
        yield return new WaitForSeconds(0.4f);
        startRT.gameObject.SetActive(false);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            bool b = false;
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (PhotonNetwork.PlayerList[i].GetTeamNumber() == otherPlayer.GetTeamNumber())
                {
                    b = true;
                }
            }
            if (!b)
            {
                PhotonNetwork.CurrentRoom.TeamDeath(otherPlayer.GetTeamNumber());
            }
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        int liveTeams = 0;
        for (int i = 0; i < GameSettings.maxPlayer / 2; i++)
        {
            if (!PhotonNetwork.CurrentRoom.GetTeamDeath(i))
            {
                liveTeams++;
            }
        }
        if (liveTeams == 1)
        {
            int winTeamNumber = 0;
            for (winTeamNumber = 0; winTeamNumber < GameSettings.maxPlayer / 2; winTeamNumber++)
            {
                if (!PhotonNetwork.CurrentRoom.GetTeamDeath(winTeamNumber)) break;
            }
            int j = 0;
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (PhotonNetwork.PlayerList[i].GetTeamNumber() == winTeamNumber)
                {
                    winnerPlayerName[j] = PhotonNetwork.PlayerList[i].NickName;
                    j++;
                }

            }
            FinishGame();
        }
    }

    private void FinishGame()
    {
        StartCoroutine(FinishFlow());
    }

    private IEnumerator FinishFlow()
    {
        // for (int i = 0; i < chickens.Length; i++)
        // {
        //     if(chickens[i] != null){
        //         chickens[i].SetIsEnable(false);
        //     }
        // }
        finishRT.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1.5f);
        finishRT.DOScale(Vector3.zero, 0.5f).SetEase(Ease.Linear);
        Fader.StartFadeOut(0.5f, 1.0f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.sceneLoaded += SendWinnerPlayer;
        SceneManager.LoadScene("Result");
    }

    private void SendWinnerPlayer(Scene next, LoadSceneMode mode)
    {
        GameObject.Find("ResultManager").GetComponent<ResultManager>().SetWinnerPlayerName(winnerPlayerName[0], winnerPlayerName[1]);
        SceneManager.sceneLoaded -= SendWinnerPlayer;
    }
}
