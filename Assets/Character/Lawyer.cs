using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lawyer : MonoBehaviour
{
    public enum Team
    {
        Prosecutors,
        Defendants,
    }

    public Team correspondingTeam;
    public int health;
    public int maxHealth;
    public bool hasItem = false;

    void Start()
    {

    }

    void Update()
    {

    }

    public void die()
    {
        gameObject.SetActive(false);
    }
}
