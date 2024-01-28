using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private GameObject _fadePrefab;
    [SerializeField] private float _fadeTime;
    [SerializeField] private VideoPlayer _player;
    [SerializeField] private List<GameObject> _objectsToDisable;
    [SerializeField] private double _minusLoadTime;

    public void LoadSceneFade(string sceneName)
    {
        GetComponent<Button>().interactable = false;

        FadeInOut f = Instantiate(_fadePrefab).GetComponent<FadeInOut>();
        StartCoroutine(f.FadeInOutColor(false));

        //StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName, double waitTime)
    {
        yield return new WaitForSeconds((float)waitTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }


        yield return null;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PlayVideo(string sceneName)
    {
        GetComponent<Button>().interactable = false;

        f = Instantiate(_fadePrefab).GetComponent<FadeInOut>();
        StartCoroutine(f.FadeInOutColor(false));
        StartCoroutine(Video(sceneName));
    }

    FadeInOut f;

    IEnumerator Video(string sceneName)
    {
        yield return new WaitForSeconds(_fadeTime);
        
        _player.Play();
        _player.GetComponent<LoadScene>().StartCoroutine(_player.GetComponent<LoadScene>().LoadSceneAsync(sceneName, _player.length - _minusLoadTime));

        Destroy(f.gameObject, 0.5f);

        foreach (GameObject g in _objectsToDisable)
            g.SetActive(false);
    }
}