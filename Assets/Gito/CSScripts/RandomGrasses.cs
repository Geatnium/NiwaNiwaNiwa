using UnityEngine;

public class RandomGrasses : MonoBehaviour
{
    [SerializeField] private int num = 100;
    [SerializeField] private GameObject grass;

    [ContextMenu("Generate")]
    public void Generate()
    {
        for (int i = 0; i < num; i++)
        {
            GameObject g = Instantiate(grass, new Vector3(Random.Range(-25f, 25f), 0f, Random.Range(-25f, 25f)), Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
            g.transform.parent = transform;
        }
    }
}
