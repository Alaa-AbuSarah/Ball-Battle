using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject mask = null;

    public void LoadScene(string sceneName) => StartCoroutine(LoadSceneAsync(sceneName));
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        mask.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
