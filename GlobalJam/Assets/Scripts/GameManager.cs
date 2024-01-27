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

    [SerializeField] Vector2Reference playerPos;
    [SerializeField] BoolReference playerIsVisible;
    [SerializeField] GameObject TreeAnim;
    [SerializeField] GameObject ExplodingAnim;
    [SerializeField] GameObject EatenByRock;

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
        //no need/unused idea of reversed controls
    }

    IEnumerator SpearDeath()
    {
        yield return StartCoroutine(Explode());
        yield return StartCoroutine(ShowTragicEnd());
    }

    IEnumerator PlatformDeath()
    {
        //animation
        //no need/won t implement false platform anymore
        yield return StartCoroutine(ResetLevelAndChooseDeath());
    }

    IEnumerator TreeDeath()
    {
        //play animation
        playerIsVisible.Value = false;
        yield return Instantiate(TreeAnim, playerPos.Value, Quaternion.identity);
        yield return StartCoroutine(ShowTragicEnd());  
    }

    IEnumerator CrushDeath() 
    {
        //animation
        yield return StartCoroutine(Explode());
        yield return StartCoroutine(ShowTragicEnd());
    }

    IEnumerator CubeDeath()
    {
        //play animation
        yield return StartCoroutine(Explode());
        yield return StartCoroutine(ShowTragicEnd());
    }

    IEnumerator StoneDeath()
    {
        //animation
        playerIsVisible.Value = false;
        yield return Instantiate(EatenByRock, EatenByRock.transform.position, Quaternion.identity);
        yield return StartCoroutine(ShowTragicEnd());
    }

    IEnumerator Explode()
    {
        playerIsVisible.Value = false;
        yield return Instantiate(ExplodingAnim, playerPos.Value, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
    }
    IEnumerator ResetLevelAndChooseDeath()
    {

        /*currentDeathInScene = deathOrder[0];
        deathOrder.RemoveAt(0);*/
        canMove.Value = true;
        playerIsVisible.Value = true;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }


        StartCoroutine(CreateDeathInScene());   

        yield return StartCoroutine(fadeScreen.GetComponent<FadeInOut>().FadeInOutColor(true)); ;
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
