
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    bool isTransitioning = false;
    bool collectionDisabled = false;
    AudioSource audioSource;
     void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ResponToDebugKeys();
    }

    void ResponToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collectionDisabled=!collectionDisabled;
        }
    }

    void OnCollisionEnter(Collision other) {
        if(isTransitioning || collectionDisabled){return;}

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            case "Fuel":
                Debug.Log("You picked up fuel");
                break;
            default:
                StartCrashSequence();
                break;
        }
        
    }
     void StartSuccessSequence()
    {   
        isTransitioning=true;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        
        // todo add SFX upon crash
        // todo add particle effect upon crash
        GetComponent<Movement>().enabled = false;
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }
    void StartCrashSequence()
    {
        isTransitioning=true;
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
        // todo add SFX upon crash
        // todo add particle effect upon crash
        GetComponent<Movement>().enabled = false;
        crashParticles.Play();
        Invoke("ReloadLevel", levelLoadDelay);
    }

     void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    void ReloadLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex ;
        SceneManager.LoadScene(currentLevelIndex);
    }
}
