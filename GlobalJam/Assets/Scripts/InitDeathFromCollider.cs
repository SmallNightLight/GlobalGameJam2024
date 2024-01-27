using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitDeathFromCollider : MonoBehaviour
{
    public GameManager.deathEvents typeOfDeath;
    [SerializeField] private BoolReference activeTreeEvent;


    private void Start()
    {
        if (activeTreeEvent.Value)
            Destroy(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        activeTreeEvent.Value = true;
        GameManager.Instance.Death(typeOfDeath);      
    }
    
}
