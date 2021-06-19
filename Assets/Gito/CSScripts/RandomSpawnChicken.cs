using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static Niwatori.Utility;
using System.Collections;

namespace Niwatori
{
    public class RandomSpawnChicken : MonoBehaviourPunCallbacks
    {
        [SerializeField] private int num = 8;
        [SerializeField] private GameObject spawnArea;
        private GameObject[] chickens;

        public void Spawn()
        {
            StartCoroutine(SpawnCor());
        }

        private IEnumerator SpawnCor()
        {
            chickens = new GameObject[num];
            for (int i = 0; i < num; i++)
            {
                // GameObject chickenPrefab = (GameObject)Resources.Load("Chicken");
                // GameObject go = Instantiate(chickenPrefab);
                chickens[i] = PhotonNetwork.Instantiate("Chicken", Vector3.zero, Quaternion.identity);
                chickens[i].transform.position = new Vector3(Random.Range(spawnArea.transform.position.x - spawnArea.transform.localScale.x / 2f, spawnArea.transform.position.x + spawnArea.transform.localScale.x / 2f),
                                                    0f,
                                                    Random.Range(spawnArea.transform.position.z - spawnArea.transform.localScale.z / 2f, spawnArea.transform.position.z + spawnArea.transform.localScale.z / 2f));
                chickens[i].transform.eulerAngles = new Vector3(0f, Random.Range(0f, 360f), 0f);
                chickens[i].AddComponent<AIController>();
            }
            yield return new WaitForSeconds(5f);
            for (int i = 0; i < num; i++)
            {
                chickens[i].GetComponent<AIController>().StartAI();
            }
        }
    }
}
