using System.Collections;
using System.Collections.Generic;
using KartGame.KartSystems;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public CarSettings settings;
    public GameObject player_Pref;


    public GameObject CreatePlayer()
    {
        GameObject go = Instantiate(player_Pref);
        if (settings != null)
        {
            go.GetComponent<ArcadeKart>().baseStats = settings.stats;
        }
        return go;
    }
}
