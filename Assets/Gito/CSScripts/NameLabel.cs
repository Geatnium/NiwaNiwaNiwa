using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NameLabel : MonoBehaviourPunCallbacks
{
    [SerializeField] private Color normalColor, playerColor, pairColor, enemyColor;
    public void SetNameLabel(string name)
    {
        TextMeshPro nameLabel = GetComponent<TextMeshPro>();
        nameLabel.text = name;
    }

    public void SetColor(ENameColor nameColor)
    {
        TextMeshPro nameLabel = GetComponent<TextMeshPro>();
        switch (nameColor)
        {
            case ENameColor.Normal:
                nameLabel.color = normalColor;
                break;
            case ENameColor.Player:
                nameLabel.color = playerColor;
                break;
            case ENameColor.Pair:
                nameLabel.color = pairColor;
                break;
            case ENameColor.Enemy:
                nameLabel.color = enemyColor;
                break;
        }
    }

    private void Update()
    {
        transform.forward = transform.position - Camera.main.transform.position;
        // transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0f, 0f);
    }
}

public enum ENameColor
{
    Normal, Player, Pair, Enemy
}
