using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject mask = null;

    public void LoadGame(string sceneName) => StartCoroutine(LoadScene(sceneName));
    private IEnumerator LoadScene(string sceneName)
    {
        mask.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
