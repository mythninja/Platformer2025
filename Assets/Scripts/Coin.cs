using System;
using UnityEngine;

public class Coin : MonoBehaviour, Iitem
{

    public static event Action<int> OnCoinCollect;
    //sets variable to 5 on how much the coin is worth 
    public int worth = 1;
    //runs the collect script when player touches coin 
    public void Collect()
    {
        if (!gameObject.activeSelf) return; // guard against double collect

        OnCoinCollect?.Invoke(worth);
        gameObject.SetActive(false); // disable immediately
        Destroy(gameObject);
    }

}