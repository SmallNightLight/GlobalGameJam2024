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
        TimeLimit,
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

    IEnumerator ReversedDeath()
    {
        yield return StartCoroutine(ResetLevelAndChooseDeath());
    }

    IEnumerator TimeLimitDeath()
    {
        yield return StartCoroutine(ResetLevelAndChooseDeath());
    }

    IEnumerator PlatformDeath()
    {
        yield return StartCoroutine(ResetLevelAndChooseDeath());
    }

    IEnumerator TreeDeath()
    {
        //play animation
        yield return StartCoroutine(fadeScreen.GetComponent<FadeInOut>().FadeInOutColor(false));
        yield return StartCoroutine(ResetLevelAndChooseDeath());
    }

    IEnumerator CrushDeath() 
    {
        yield return StartCoroutine(fadeScreen.GetComponent<FadeInOut>().FadeInOutColor(false));
        yield return StartCoroutine(ResetLevelAndChooseDeath());
    }

    IEnumerator CubeDeath()
    {
        //play animation
        yield return StartCoroutine(fadeScreen.GetComponent<FadeInOut>().FadeInOutColor(false));
        yield return StartCoroutine(ResetLevelAndChooseDeath());
    }

    IEnumerator StoneDeath()
    {
        yield return StartCoroutine(ResetLevelAndChooseDeath());
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
