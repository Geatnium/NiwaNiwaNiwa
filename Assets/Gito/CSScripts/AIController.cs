using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static Niwatori.Utility;

namespace Niwatori
{
    public class AIController : MonoBehaviourPunCallbacks
    {
        private IChicken _chicken;
        private IChicken chicken
        {
            get { return _chicken == null ? _chicken = GetComponent<IChicken>() : _chicken; }
        }

        private bool isEnableAI = false;

        private enum State
        {
            Normal, Follow
        }
        private State state;

        private float t = 0f, t2 = 0f, ct = 0f;
        private float normalInterval = 3f, followInterval = 0.5f, searchRadius = 3.0f, attackDistance = 1.0f, checkTargetInterval = 0.2f;
        private float randomInterval;
        private IChicken target;

        private IChicken[] chickens;

        public void SetNumberAndTeamNumber(int number, int teamNumber)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                chicken.SetNumber(number);
                chicken.SetTeamNumber(teamNumber);
            }
        }

        public void SetNameLabel(string name)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                chicken.SetNameLabel(name);
            }
        }

        public void StartAI()
        {
            isEnableAI = true;
            chicken.SetIsEnable(true);
            chicken.OnDownInput(EInput.Forward);
            RandomFunc(() =>
            {
                chicken.OnDownInput(EInput.Right);
            }, () =>
            {
                chicken.OnDownInput(EInput.Left);
            });
            GameObject[] chickenObjs = GameObject.FindGameObjectsWithTag("Chicken");
            chickens = new IChicken[chickenObjs.Length];
            for (int i = 0; i < chickenObjs.Length; i++)
            {
                chickens[i] = chickenObjs[i].GetComponent<IChicken>();
            }
        }

        private void ToDirectionInput(Vector3 dir)
        {
            dir.y = 0f;
            dir = dir.normalized;
            if (dir.z > 0.3f)
            {
                chicken.OnDownInput(EInput.Forward);
                chicken.OnUpInput(EInput.Back);
            }
            else if (dir.z < -0.3f)
            {
                chicken.OnDownInput(EInput.Back);
                chicken.OnUpInput(EInput.Forward);
            }
            else
            {
                chicken.OnUpInput(EInput.Forward);
                chicken.OnUpInput(EInput.Back);
            }
            if (dir.x > 0.3f)
            {
                chicken.OnDownInput(EInput.Right);
                chicken.OnUpInput(EInput.Left);
            }
            else if (dir.x < -0.3f)
            {
                chicken.OnDownInput(EInput.Left);
                chicken.OnUpInput(EInput.Right);
            }
            else
            {
                chicken.OnUpInput(EInput.Right);
                chicken.OnUpInput(EInput.Left);
            }
        }

        private void Update()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (isEnableAI)
                {
                    ct += Time.deltaTime;
                    if (ct > checkTargetInterval)
                    {
                        ct = 0f;
                        float minDis = searchRadius;
                        for (int i = 0; i < chickens.Length; i++)
                        {
                            if (!chickens[i].GetBoolVariable("IsDeath"))
                            {
                                if (chickens[i].GetTeamNumber() != chicken.GetTeamNumber())
                                {
                                    float dis = Vector3.Distance(transform.position, chickens[i].GetMineGameObject().transform.position);
                                    if (dis < minDis)
                                    {
                                        minDis = dis;
                                        target = chickens[i];
                                    }
                                }
                            }
                        }
                        if (minDis < searchRadius)
                        {
                            state = State.Follow;
                        }
                        else
                        {
                            state = State.Normal;
                        }
                    }
                    if (state == State.Normal)
                    {
                        t += Time.deltaTime;
                        if (t > normalInterval + randomInterval)
                        {
                            t = 0f;
                            randomInterval = Random.Range(-0.5f, 0.5f);
                            PercentDoFunc(0.25f, () =>
                            {
                                chicken.OnDownInput(EInput.Dash);
                            }, () =>
                            {
                                chicken.OnUpInput(EInput.Dash);
                            });
                            if (Physics.Raycast(transform.position + Vector3.up * 0.3f, transform.forward, 1.0f))
                            {
                                ToDirectionInput(Quaternion.Euler(0, Random.Range(-90f, 90f), 0f) * -transform.forward);
                            }
                            else
                            {
                                ToDirectionInput(Quaternion.Euler(0, Random.Range(-90f, 90f), 0f) * transform.forward);
                            }
                        }
                    }
                    else if (state == State.Follow)
                    {
                        t += Time.deltaTime;
                        if (t > followInterval)
                        {
                            t = 0f;
                            ToDirectionInput(target.GetMineGameObject().transform.position - transform.position);
                            if (Vector3.Distance(target.GetMineGameObject().transform.position, transform.position) < attackDistance && !target.GetBoolVariable("IsDeath"))
                            {
                                chicken.OnDownInput(EInput.Attack);
                            }
                        }
                    }
                }
            }
        }
    }
}
