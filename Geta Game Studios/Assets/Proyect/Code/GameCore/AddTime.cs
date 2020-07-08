using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTime : MonoBehaviour
{
    public float timeToAdd;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            TimeAtackManager.Instance.AddTime(timeToAdd);
            gameObject.SetActive(false);
        }
    }
}
