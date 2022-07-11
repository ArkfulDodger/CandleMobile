using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    [SerializeField]
    private Image progressBar;

    // Start is called before the first frame update
    void Start()
    {
        // start Async Operation
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        // Create Async Operation
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync("Game");
        
        while(gameLevel.progress < 1)
        {
            // set progress bar to progress level
            progressBar.fillAmount = gameLevel.progress;
            yield return new WaitForEndOfFrame();
        }

        //yield return new WaitForEndOfFrame();
    }
}
