using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject _loadingCanvas;
    [SerializeField] private Image _progressBar;
    private readonly float _fullyLoaded = 0.9f;
    private float _target;

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
            Destroy(this);
        }
    }

    public async void LoadScene(string sceneName)
    {
        // set target and fill bar back to 0
        _target = 0f;
        _progressBar.fillAmount = 0f;

        // Load Scene Asynchronously
        var scene = SceneManager.LoadSceneAsync(sceneName);

        // Prevent Scene from Activating automatically
        scene.allowSceneActivation = false;

        // Display Loading Canvas
        _loadingCanvas.SetActive(true);

        // update the progress bar in a loop until the scene is loaded (at 90%)
        do
        {
            //await Task.Delay(100);
            _target = scene.progress / _fullyLoaded;
            //_progressBar.fillAmount = scene.progress;
        } while (scene.progress < _fullyLoaded);

        await Task.Delay(1000);

        // allow scene to load
        scene.allowSceneActivation = true;

        // hide loading canvas
        _loadingCanvas.SetActive(false);
    }

    private void Update()
    {
        _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _target, 3 * Time.deltaTime);
    }
}
