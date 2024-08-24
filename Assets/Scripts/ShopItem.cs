using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
internal struct ShopItem
{
    public GameObject prefab;
    public Image image;
    public int cost;
    public string description;
}