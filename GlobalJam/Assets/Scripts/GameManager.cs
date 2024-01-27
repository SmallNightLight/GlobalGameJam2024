using ScriptableArchitecture.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public enum deathEvents
    {
        Reversed,
        Spear,
        Platform,
        Tree,
        Crush,
        Cube,
        Stone
    }

    deathEvents currentDeathInScene;

    List<deathEvents> deathOrder = new List<deathEvents>();

    public List<GameObject> deathPrefabs = new List<GameObject>();

    public GameObject fadeScreenPrefab;

    GameObject fadeScreen;

    List<GameObject> deathPrefabInScene;

    public GameObject laughingEndingPrefab;

    [SerializeField] BoolReference canMove;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //currentDeathInScene = (deathEvents)UnityEngine.Random.Range(0, Enum.GetValues(typeof(deathEvents)).Length);
        //StartCoroutine(CreateDeathOrder());

        deathPrefabInScene = new List<GameObject>();
        //REMOVE LATER !
        StartCoroutine(CreateDeathInScene());
        DontDestroyOnLoad(gameObject);
    }

    IEnumerator CreateDeathOrder()
    {
        while(deathOrder.Count != Enum.GetValues(typeof(deathEvents)).Length)
        {
            deathEvents tempDeathVar = (deathEvents)UnityEngine.Random.Range(0, Enum.GetValues(typeof(deathEvents)).Length);

            if (!deathOrder.Contains(tempDeathVar))
            {
                deathOrder.Add(tempDeathVar);
            }
            yield return null;
        }

        currentDeathInScene = deathOrder[0];
        deathOrder.RemoveAt(0);
        yield return null;
    }

    IEnumerator CreateDeathInScene()
    {
        foreach(var death in deathPrefabs)
        {
            deathPrefabInScene.Add(Instantiate(death, new Vector3(), Quaternion.identity));
        }
        fadeScreen = Instantiate(fadeScreenPrefab, new Vector3(), Quaternion.identity);
        yield return null;
    }

    public void Death(deathEvents deathType)
    {
        StartCoroutine(deathType + "Death");
    }

    IEnumerator ShowTragicEnd()
    {
        canMove.Value = false;
        yield return Instantiate(laughingEndingPrefab, new Vector3(), Quaternion.identity);
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(fadeScreen.GetComponent<FadeInOut>().FadeInOutColor(false));
        yield return StartCoroutine(ResetLevelAndChooseDeath());
    }

    IEnumerator ReversedDeath()
    {
        yield return StartCoroutine(ResetLevelAndChooseDeath());
    }

    IEnumerator SpearDeath()
    {
        yield return StartCoroutine(ResetLevelAndChooseDeath());
    }

    IEnumerator PlatformDeath()
    {
        //animation

        yield return StartCoroutine(ResetLevelAndChooseDeath());
    }

    IEnumerator TreeDeath()
    {
        //play animation

        yield return StartCoroutine(ShowTragicEnd());  
    }

    IEnumerator CrushDeath() 
    {
        //animation

        yield return StartCoroutine(ShowTragicEnd());
    }

    IEnumerator CubeDeath()
    {
        //play animation

        yield return StartCoroutine(ShowTragicEnd());
    }

    IEnumerator StoneDeath()
    {
        //animation

        yield return StartCoroutine(ShowTragicEnd());
    }

    IEnumerator ResetLevelAndChooseDeath()
    {

        /*currentDeathInScene = deathOrder[0];
        deathOrder.RemoveAt(0);*/
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        StartCoroutine(CreateDeathInScene());

        yield return null;
    }

    public void FirstLvlInitListener()
    {
        StartCoroutine(FirstLvlInit());
    }
    IEnumerator FirstLvlInit()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        StartCoroutine(CreateDeathInScene());

        yield return null;
    }
}
