using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int HP= 100, damage = 10, maxHP;

    protected void Start ()
    {
        maxHP = HP;
    }

    private void OnEnable()
    {
        maxHP = HP;
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
