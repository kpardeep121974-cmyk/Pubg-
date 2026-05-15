
using UnityEngine;

public class FootstepSoundManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip[] concreteSteps;
    public AudioClip[] grassSteps;
    public AudioClip[] woodSteps;
    public AudioClip[] waterSteps;

    [Header("Settings")]
    public float baseStepInterval = 0.5f; // चलते समय आवाज़ों के बीच का गैप (सेकंड में)
    public float sprintStepInterval = 0.3f; // दौड़ते समय आवाज़ें और तेज़ होंगी

    private AudioSource audioSource;
    private CharacterController characterController;
    private float stepTimer;

    private void Awake()
    {
        // इसे central Service Locator में रजिस्टर करें
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<FootstepSoundManager>(this);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();

        // अगर AudioSource नहीं है तो ऑटोमैटिकली ऐड करें
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // 3D Spatial Audio सेटिंग्स (ताकि दिशा और दूरी का पता चले)
        audioSource.spatialBlend = 1.0f; // 1.0 मतलब पूरी तरह 3D साउंड
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 25f; // 25 मीटर के बाद आवाज़ आना बंद हो जाएगी
    }

    void Update()
    {
        // अगर प्लेयर ज़मीन पर है और हिल रहा है
        if (characterController.isGrounded && characterController.velocity.magnitude > 0.1f)
        {
            // चेक करें कि प्लेयर नॉर्मल चल रहा है या स्प्रिंट (दौड़) कर रहा है
            bool isSprinting = characterController.velocity.magnitude > 6f; 
            float currentInterval = isSprinting ? sprintStepInterval : baseStepInterval;

            stepTimer += Time.deltaTime;

            if (stepTimer >= currentInterval)
            {
                PlayFootstepBasedOnSurface();
                stepTimer = 0f;
            }
        }
    }

    void PlayFootstepBasedOnSurface()
    {
        RaycastHit hit;
        // पैर के नीचे रेकास्ट छोड़ें (3 मीटर नीचे तक)
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 3f))
        {
            string surfaceTag = hit.collider.tag;
            AudioClip clipToPlay = null;

            // ज़मीन के Tag के हिसाब से ऑडियो क्लिप चुनें
            switch (surfaceTag)
            {
                case "Concrete":
                    clipToPlay = GetRandomClip(concreteSteps);
                    break;
                case "Grass":
                    clipToPlay = GetRandomClip(grassSteps);
                    break;
                case "Wood":
                    clipToPlay = GetRandomClip(woodSteps);
                    break;
                case "Water":
                    clipToPlay = GetRandomClip(waterSteps);
                    break;
                default:
                    clipToPlay = GetRandomClip(concreteSteps); // डिफ़ॉल्ट साउंड
                    break;
            }

            if (clipToPlay != null)
            {
                audioSource.PlayOneShot(clipToPlay);
            }
        }
    }

    AudioClip GetRandomClip(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return null;
        int randomIndex = Random.Range(0, clips.Length);
        return clips[randomIndex];
    }
}
