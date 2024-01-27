using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private GameObject _fadePrefab;
    [SerializeField] private float _fadeTime;

    public void LoadSceneFade(string sceneName)
    {
        GetComponent<Button>().interactable = false;

        FadeInOut f = Instantiate(_fadePrefab).GetComponent<FadeInOut>();
        StartCoroutine(f.FadeInOutColor(false));

        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return new WaitForSeconds(_fadeTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }


        yield return null;
    }
}