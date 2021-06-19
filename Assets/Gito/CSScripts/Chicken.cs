using UnityEngine;
using UnityEngine.Events;
using System;
using Bolt;
using Photon.Pun;
using Photon.Realtime;

namespace Niwatori
{
    public class Chicken : MonoBehaviourPunCallbacks, IChicken
    {
        public int number, teamNumber;
        private string thisName;
        private bool isPlayer = false;
        private bool[] isInputs;
        [SerializeField] private InputEvent downInputEvent;
        [SerializeField] private InputEvent upInputEvent;
        private VariableDeclarations variables;
        private IChicken pairChicken;
        private bool isResurectAble = false;

        private void Start()
        {
            variables = Variables.Object(gameObject);
            isInputs = new bool[Enum.GetValues(typeof(EInput)).Length];
            downInputEvent = new InputEvent();
            upInputEvent = new InputEvent();
            AddDownInputEvent(EInput.Attack, () =>
            {
                photonView.RPC(nameof(OnDownAttack_P), RpcTarget.AllViaServer);
            });
        }

        public void SetNumber(int number)
        {
            photonView.RPC(nameof(SetNumber_P), RpcTarget.AllBufferedViaServer, number);
        }

        [PunRPC]
        private void SetNumber_P(int number)
        {
            this.number = number;
        }

        public void SetTeamNumber(int teamNumber)
        {
            photonView.RPC(nameof(SetTeamNumber_P), RpcTarget.AllBufferedViaServer, teamNumber);
        }

        [PunRPC]
        private void SetTeamNumber_P(int teamNumber)
        {
            this.teamNumber = teamNumber;
            if (!isPlayer)
            {
                if (teamNumber == PhotonNetwork.LocalPlayer.GetTeamNumber())
                {
                    SetNameColor(ENameColor.Pair);
                }
                else
                {
                    SetNameColor(ENameColor.Enemy);
                }
            }
        }

        public void SetIsPlayer(bool isPlayer)
        {
            this.isPlayer = isPlayer;
            SetNameColor(ENameColor.Player);
        }

        public void SetNameLabel(string name)
        {
            photonView.RPC(nameof(SetNameLabel_P), RpcTarget.AllBufferedViaServer, name);
        }

        [PunRPC]
        private void SetNameLabel_P(string name)
        {
            GetComponentInChildren<NameLabel>().SetNameLabel(name);
            thisName = name;
        }

        public string GetName()
        {
            return thisName;
        }

        public void SetNameColor(ENameColor nameColor)
        {
            GetComponentInChildren<NameLabel>().SetColor(nameColor);
        }

        public void DoFindPairChickenAndSet()
        {
            GameObject[] chickens = GameObject.FindGameObjectsWithTag("Chicken");
            for (int i = 0; i < chickens.Length; i++)
            {
                if (chickens[i] != gameObject)
                {
                    IChicken c = chickens[i].GetComponent<IChicken>();
                    if (teamNumber == c.GetTeamNumber())
                    {
                        pairChicken = c;
                        break;
                    }
                }
            }
        }

        public GameObject GetMineGameObject()
        {
            return gameObject;
        }

        public IChicken GetPairChicken()
        {
            return pairChicken;

        }

        public int GetNumber()
        {
            return number;
        }

        public int GetTeamNumber()
        {
            return teamNumber;
        }

        public int GetIntVariable(string variableName)
        {
            return (int)variables.Get(variableName);
        }

        public float GetFloatVariable(string variableName)
        {
            return (float)variables.Get(variableName);
        }

        public bool GetBoolVariable(string variableName)
        {
            return (bool)variables.Get(variableName);
        }

        public object GetVariableNoCast(string variableName)
        {
            return variables.Get(variableName);
        }

        public void AddDownInputEvent(EInput input, UnityAction call)
        {
            downInputEvent.AddListener(input, call);
        }

        public void AddUpInputEvent(EInput input, UnityAction call)
        {
            upInputEvent.AddListener(input, call);
        }

        public void SetIsEnable(bool enable)
        {
            photonView.RPC(nameof(SetIsEnable_P), RpcTarget.AllBufferedViaServer, enable);
        }

        [PunRPC]
        private void SetIsEnable_P(bool enable)
        {
            CustomEvent.Trigger(gameObject, "SetIsEnable", enable);
        }

        public void OnDownInput(EInput input)
        {
            photonView.RPC(nameof(OnDownInput_P), RpcTarget.AllViaServer, input);
        }

        [PunRPC]
        private void OnDownInput_P(EInput input)
        {
            isInputs[(int)input] = true;
            downInputEvent.Invoke(input);
        }

        public void OnUpInput(EInput input)
        {
            photonView.RPC(nameof(OnUpInput_P), RpcTarget.AllViaServer, input);
        }

        [PunRPC]
        private void OnUpInput_P(EInput input)
        {
            isInputs[(int)input] = false;
            upInputEvent.Invoke(input);
        }

        public void OnTakeDamage(int damage)
        {
            photonView.RPC(nameof(OnTakeDamage_P), RpcTarget.AllViaServer, damage);
        }

        [PunRPC]
        private void OnTakeDamage_P(int damage)
        {
            CustomEvent.Trigger(gameObject, "OnTakeDamage", damage);
        }

        public bool GetIsResurrectAble()
        {
            return isResurectAble;
        }

        public void OnResurectPair()
        {
            photonView.RPC(nameof(OnResurectPair_P), RpcTarget.AllViaServer);
        }

        [PunRPC]
        private void OnResurectPair_P()
        {
            CustomEvent.Trigger(gameObject, "OnResurectPair");
        }

        [PunRPC]
        private void OnDownAttack_P()
        {
            CustomEvent.Trigger(gameObject, "OnAttack");
        }

        public bool GetIsInput(EInput input)
        {
            return isInputs[(int)input];
        }

        public float GetHorizontal()
        {
            float f = 0f;
            if (GetIsInput(EInput.Right))
            {
                f = 1f;
            }
            if (GetIsInput(EInput.Left))
            {
                f = -1f;
            }
            if (GetIsInput(EInput.Right) && GetIsInput(EInput.Left))
            {
                f = 0f;
            }
            return f;
        }

        public float GetVertical()
        {
            float f = 0f;
            if (GetIsInput(EInput.Forward))
            {
                f = 1f;
            }
            if (GetIsInput(EInput.Back))
            {
                f = -1f;
            }
            if (GetIsInput(EInput.Forward) && GetIsInput(EInput.Back))
            {
                f = 0f;
            }
            return f;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void OnTeamDeath()
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.CurrentRoom.TeamDeath(teamNumber);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (photonView.IsMine)
            {
                if (other.gameObject.CompareTag("ChickenAttack"))
                {
                    IAttackCollider attackCollider = other.gameObject.GetComponent<IAttackCollider>();
                    if (attackCollider.GetTeamNumber() != teamNumber)
                    {
                        OnTakeDamage(attackCollider.GetPower());
                    }
                }
            }
        }

        private bool CheckPairResurectAble()
        {
            if (pairChicken == null)
            {
                return false;
            }
            if (!pairChicken.GetBoolVariable("IsDeath"))
            {
                return false;
            }
            float distance = Vector3.Distance(pairChicken.GetPosition(), transform.position);
            if (distance < 1.0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if(photonView.IsMine)
            {
                int leftPlayerTeamNumber = otherPlayer.GetTeamNumber();
                if(teamNumber == leftPlayerTeamNumber)
                {
                    pairChicken = null;
                    if(GetBoolVariable("IsDeath"))
                    {
                        OnTeamDeath();
                    }
                }
            }
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                isResurectAble = CheckPairResurectAble();
            }
        }
    }
}
