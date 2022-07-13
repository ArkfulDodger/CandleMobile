using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;


// TODO: make it so array of build names is retrieved on Awake or Start, so it can be referenced to confirm scene names prior to load
// TODO: add screen (canvas) fade in and fade out

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject _loadingCanvas;
    [SerializeField] private Image _progressBar;
    private readonly float _fullyLoaded = 0.9f;
    private float _targetFillAmount;
    private bool _isSceneBeingLoaded = false;

    // ENFORCE SINGLETON
    public static LevelManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Loads a new Scene
    public void LoadScene(string sceneName)
    {
        // escape if another scene is already being loaded
        if (_isSceneBeingLoaded)
        {
            Debug.LogWarning("Did not load scene \"" + sceneName + "\" because another scene is currently loading.");
            return;
        }

        // escape if the requested scene is not in the build
        if (!IsSceneInBuild(sceneName))
        {
            Debug.LogError("Could not load scene \"" + sceneName + "\" because it is not in the build");
            return;
        }

        // load the scene
        StartCoroutine("LoadSceneAsync", sceneName);
        _isSceneBeingLoaded = true;
    }

    private bool IsSceneInBuild(string name)
    {
        if (string.IsNullOrEmpty(name)) return false;

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            var lastSlash = scenePath.LastIndexOf("/");
            var sceneName = scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1);

            if (string.Compare(name, sceneName, true) == 0)
                return true;
        }

        return false;
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // set target and fill bar back to 0
        _targetFillAmount = 0f;
        _progressBar.fillAmount = 0f;

        // Display Loading Canvas
        _loadingCanvas.SetActive(true);

        // Pause briefly to let canvas render to screen
        yield return new WaitForSeconds(0.2f);

        // Load Scene Asynchronously
        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);

        // Prevent Scene from Activating automatically
        scene.allowSceneActivation = false;

        // update the progress bar in a loop until the scene is loaded (at 90%)
        do
        {
            _targetFillAmount = scene.progress / _fullyLoaded;
            _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _targetFillAmount, 0.05f);
            yield return null;
        } while (_progressBar.fillAmount < 1);

        // Pause briefly to let full load register
        yield return new WaitForSeconds(0.2f);

        // allow scene to load
        scene.allowSceneActivation = true;
        _isSceneBeingLoaded = false;

        // wait one frame so previous scene has a chance to be replaced and doesn't flash on screen
        yield return null;

        // hide loading canvas
        _loadingCanvas.SetActive(false);

        yield return null;
    }
}
