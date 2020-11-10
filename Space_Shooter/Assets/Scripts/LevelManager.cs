
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public static LevelManager instance;

	public GameObject faderObj;
	public Image faderImg;
	public bool gameOver = false;
    private AudioSource audioSource; //fon
    private AudioSource audioSource_ClickClap;
    public float fadeSpeed = .02f;
    public Texture2D[] coursorMouse;

	private Color fadeTransparency = new Color(0, 0, 0, .04f);
	private string currentScene;
	private AsyncOperation async;

    public AudioClip[] audioClips;
    // 0- click, 1- laser ,3- bangship
    // 4- attachcomet 5- gameover, 6 -win

	void Awake() {
		// Only 1 Game Manager can exist at a time
		if (instance == null) {
			DontDestroyOnLoad(gameObject);
			instance = GetComponent<LevelManager>();
			SceneManager.sceneLoaded += OnLevelFinishedLoading;
            audioSource = GetComponent<AudioSource>();
            audioSource_ClickClap = transform.GetChild(1).transform.GetComponent<AudioSource>();

        } else {
			Destroy(gameObject);
		}
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			ReturnToMenu();
		}
        if (Input.GetMouseButtonDown(0))//изменение курсора при нажатии
        {
            Cursor.SetCursor(coursorMouse[1],new Vector2(0,0),CursorMode.Auto);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(coursorMouse[0], new Vector2(0, 0), CursorMode.Auto);
        }
    }

	// Load a scene with a specified string name
	public void LoadScene(string sceneName) {
		instance.StartCoroutine(Load(sceneName));
		instance.StartCoroutine(FadeOut(instance.faderObj, instance.faderImg));
	}

	// Reload the current scene
	public void ReloadScene() {
		LoadScene(SceneManager.GetActiveScene().name);
	}

	private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
		currentScene = scene.name;
		instance.StartCoroutine(FadeIn(instance.faderObj, instance.faderImg));
	}

	//Iterate the fader transparency to 100%
	IEnumerator FadeOut(GameObject faderObject, Image fader) {
		faderObject.SetActive(true);
		while (fader.color.a < 1) {
			fader.color += fadeTransparency;
			yield return new WaitForSeconds(fadeSpeed);
		}
        ActivateScene(); //Activate the scene when the fade ends
	}


    // Iterate the fader transparency to 0%
    IEnumerator FadeIn(GameObject faderObject, Image fader) {
		while (fader.color.a > 0) {
			fader.color -= fadeTransparency;
			yield return new WaitForSeconds(fadeSpeed);
		}
		faderObject.SetActive(false);
	}

	// Begin loading a scene with a specified string asynchronously
	IEnumerator Load(string sceneName) {
		async = SceneManager.LoadSceneAsync(sceneName);
		async.allowSceneActivation = false;
		yield return async;
		isReturning = false;
    }

	// Allows the scene to change once it is loaded
	public void ActivateScene() {
		async.allowSceneActivation = true;
	}

	// Get the current scene name
	public string CurrentSceneName {
		get{
			return currentScene;
		}
	}

	public void ExitGame() {
		// If we are running in a standalone build of the game
		#if UNITY_STANDALONE
			// Quit the application
			Application.Quit();
		#endif

		// If we are running in the editor
		#if UNITY_EDITOR
			// Stop playing the scene
			UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}

	private bool isReturning = false;
	public void ReturnToMenu() {
		if (isReturning) {
			return;
		}

        if (CurrentSceneName != "Menu") {
			StopAllCoroutines();
			LoadScene("Menu");
			isReturning = true;
        }
	}

    public void ClickSound_button()
    {
        audioSource_ClickClap.PlayOneShot(audioClips[0]);
        audioSource_ClickClap.volume = 0.41f;
    }
    public void ClickSound_blaster()
    {
        audioSource_ClickClap.PlayOneShot(audioClips[1]);
        audioSource_ClickClap.volume = 0.41f;
    }
    public void ClickSound_DTPCometAndShip()
    {
        audioSource_ClickClap.PlayOneShot(audioClips[2]);
        audioSource_ClickClap.volume = 0.41f;
    }
    public void ClickSound_boomComet()
    {
        audioSource_ClickClap.PlayOneShot(audioClips[3]);
        audioSource_ClickClap.volume = 0.41f;
    }
    public void ClickSound_GameOver()
    {
        audioSource_ClickClap.PlayOneShot(audioClips[4]);
        audioSource_ClickClap.volume = 1;
        StartCoroutine(AudioIn());
        StartCoroutine(waitUpVolume(audioClips[4]));
    }
    public void ClickSound_WinMusic()
    {
        audioSource_ClickClap.PlayOneShot(audioClips[5]);
        audioSource_ClickClap.volume = 1;
        StartCoroutine(AudioIn());
        StartCoroutine(waitUpVolume(audioClips[5]));
    }

    IEnumerator waitUpVolume(AudioClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        StartCoroutine(AudioOut());
    }

    IEnumerator AudioOut( )
    {
        while (audioSource.volume < 1)
        {
            audioSource.volume += 0.05f;
            yield return new WaitForSeconds(fadeSpeed);
        }

    }
    IEnumerator AudioIn()
    {
        while (audioSource.volume > 0.2f)
        {
            audioSource.volume -= 0.05f;
            yield return new WaitForSeconds(fadeSpeed);
        }

    }
}
