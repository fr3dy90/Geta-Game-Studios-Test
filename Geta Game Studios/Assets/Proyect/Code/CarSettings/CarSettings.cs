using System.Collections;
using System.Collections.Generic;
using KartGame.KartSystems;
using UnityEngine;
using UnityEngine.Windows;

[CreateAssetMenu(fileName = "New Car Settings", menuName = "Car")]
public class CarSettings : ScriptableObject
{
    public ArcadeKart.Stats stats;
}
