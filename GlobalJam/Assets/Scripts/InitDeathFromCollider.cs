using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitDeathFromCollider : MonoBehaviour
{
    public GameManager.deathEvents typeOfDeath;
    [SerializeField] private BoolReference activeTreeEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activeTreeEvent.Value) return;

        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        activeTreeEvent.Value = true;
        GameManager.Instance.Death(typeOfDeath);      
    }
    
}
