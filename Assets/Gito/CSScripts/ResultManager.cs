using UnityEngine;
using TMPro;
using DG.Tweening;
using static Niwatori.Utility;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace Niwatori
{
    public class ResultManager : MonoBehaviour
    {
        [SerializeField] private TextMeshPro chicken0Label, chicken1Label;
        [SerializeField] private float cameraMoveDistance = 2.0f, cameraMoveDuration = 3.0f, sceneTransitionTime = 6.0f;
        [SerializeField] private AudioClip buon;

        public void SetWinnerPlayerName(string playerName0, string playerName1)
        {
            chicken0Label.text = playerName0;
            chicken1Label.text = playerName1;
        }

        private void Start()
        {
            PhotonNetwork.Disconnect();
            AudioManager.PlayOneShot(buon);
            Vector3 f = Camera.main.transform.position + Camera.main.transform.forward * cameraMoveDistance;
            f.y = Camera.main.transform.position.y;
            Camera.main.transform.DOMove(f, cameraMoveDuration).SetEase(Ease.OutQuad);
            Fader.StartFadeOutAndLoadScene(sceneTransitionTime, 1.0f, "Title");
        }
    }
}
