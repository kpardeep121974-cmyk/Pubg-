using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [Header("BGMI Style Animation")]
    public Image gameLogo;          // Aapka game logo
    public CanvasGroup splashScreen; // Logo wali screen
    public float fadeDuration = 2.0f;

    [Header("Free Fire Style Login UI")]
    public GameObject loginPanel;    // Login buttons ka panel
    public GameObject bgVideoPlayer; // Background me chalne wali video

    void Start()
    {
        // Shuruat me login panel chupa rahega aur logo animation chalega
        loginPanel.SetActive(false);
        bgVideoPlayer.SetActive(false);
        StartCoroutine(PlayBGMIPickingAnimation());
    }

    IEnumerator PlayBGMIPickingAnimation()
    {
        // 1. Logo Fade In (BGMI Style)
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            if (gameLogo != null)
                gameLogo.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, timer / fadeDuration));
            yield return null;
        }

        // 2. Logo thodi der screen par rukega
        yield return new WaitForSeconds(1.5f);

        // 3. Screen Fade Out
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            if (splashScreen != null)
                splashScreen.alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            yield return null;
        }

        // 4. Animation khatam, ab Free Fire jaisa Login UI open hoga
        splashScreen.gameObject.SetActive(false);
        ShowFreeFireLoginUI();
    }

    void ShowFreeFireLoginUI()
    {
        bgVideoPlayer.SetActive(true); // Background video chalu ho jayegi
        loginPanel.SetActive(true);    // Facebook/Google buttons dikhenge
    }

    // Buttons ke liye functions
    public void OnFacebookLoginPressed()
    {
        Debug.Log("Connecting to Facebook...");
        // Yahan login logic aayega
    }

    public void OnGoogleLoginPressed()
    {
        Debug.Log("Connecting to Google...");
    }
}
