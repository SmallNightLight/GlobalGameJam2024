using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitDeathFromCollider : MonoBehaviour
{
    public GameManager.deathEvents typeOfDeath;
    [SerializeField] private BoolReference activeTreeEvent;

    [SerializeField] private SoundEffectReference _soundEffectRaiser;
    [SerializeField] private SoundEffectReference _effect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;
        if (!activeTreeEvent.Value) return;

        if (typeOfDeath == GameManager.deathEvents.Tree)
        {
            _soundEffectRaiser.Raise(_effect.Value);
        }

        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        activeTreeEvent.Value = true;
        GameManager.Instance.Death(typeOfDeath);      
    }
    
}
