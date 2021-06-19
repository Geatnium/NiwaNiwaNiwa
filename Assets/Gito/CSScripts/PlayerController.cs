using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;

namespace Niwatori
{
    public class PlayerController : MonoBehaviourPunCallbacks
    {
        private IChicken chicken;

        private Transform gameUI;
        private GaugeButton dashButton, recoverButton, healthButton;

        private void Start()
        {
            chicken = GetComponent<IChicken>();
            chicken.SetNameLabel(photonView.Owner.NickName);
            if (photonView.IsMine)
            {
                chicken.AddDownInputEvent(EInput.Resurrect, () =>
                {
                    if (chicken.GetIsResurrectAble())
                    {
                        chicken.OnResurectPair();
                    }
                });
                chicken.SetIsPlayer(true);
                chicken.SetNumber(PhotonNetwork.LocalPlayer.GetPlayerNumber());
                chicken.SetTeamNumber(PhotonNetwork.LocalPlayer.GetTeamNumber());
                chicken.SetNameColor(ENameColor.Player);
                gameUI = transform.Find("GameUI");
                gameUI.gameObject.SetActive(true);
                dashButton = gameUI.Find("Dash").GetComponent<GaugeButton>();
                recoverButton = gameUI.Find("Recover").GetComponent<GaugeButton>();
                healthButton = gameUI.Find("Health").GetComponent<GaugeButton>();
                dashButton.SetMaxValue(chicken.GetFloatVariable("MaxStamina"));
                recoverButton.SetMaxValue(chicken.GetFloatVariable("ResurectionTime"));
                healthButton.SetMaxValue(chicken.GetIntVariable("MaxHealth"));
            }
        }

        public void OnUIButtonDown(EInput input)
        {
            chicken.OnDownInput(input);
        }

        public void OnUIButtonUp(EInput input)
        {
            chicken.OnUpInput(input);
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                for (int i = 0; i < Enum.GetValues(typeof(EInput)).Length; i++)
                {
                    if (Input.GetButtonDown(((EInput)i).ToString()))
                    {
                        chicken.OnDownInput((EInput)i);
                    }
                    if (Input.GetButtonUp(((EInput)i).ToString()))
                    {
                        chicken.OnUpInput((EInput)i);
                    }
                }
                if (chicken != null)
                {
                    dashButton.SetValue(chicken.GetFloatVariable("Stamina"));
                    healthButton.SetValue(chicken.GetIntVariable("Health"));
                    if (chicken.GetPairChicken() != null)
                    {
                        recoverButton.SetValue(chicken.GetPairChicken().GetFloatVariable("ResurectionProggress"));
                        if (chicken.GetIsResurrectAble())
                        {
                            recoverButton.SetHighlight();
                        }
                        else
                        {
                            recoverButton.SetNormalColor();
                        }
                    }
                }
            }
        }
    }
}
