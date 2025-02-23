using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private Animation fadeAnimation;
    [SerializeField] private AnimationClip fadeInClip;
    [SerializeField] private AnimationClip fadeOutClip;
    public string currentSceneName;

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (fadeAnimation != null && fadeInClip != null)
        {
            fadeAnimation.clip = fadeInClip;
            fadeAnimation.Play();

            currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }
    }

    public void LoadScene(string sceneName, LoadSceneMode mode)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, mode);
    }

    public void UnloadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
    }

    public void LoadSceneWithTransition(string sceneName, LoadSceneMode mode)
    {
        StartCoroutine(TransitionAndLoad(sceneName, mode));
    }

    public void UnloadSceneWithTransition(string sceneName)
    {
        StartCoroutine(TransitionAndUnload(sceneName));
    }

    private IEnumerator TransitionAndLoad(string sceneName, LoadSceneMode mode)
    {
        fadeAnimation.Stop();

        if (fadeAnimation != null && fadeOutClip != null)
        {
            fadeAnimation.clip = fadeOutClip;
            fadeAnimation.Play();
            yield return new WaitForSeconds(fadeOutClip.length);
        }

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, mode);
    }

    private IEnumerator TransitionAndUnload(string sceneName)
    {
        fadeAnimation.Stop();

        if (fadeAnimation != null && fadeInClip != null)
        {
            fadeAnimation.clip = fadeInClip;
            fadeAnimation.Play();

            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);

            yield return new WaitForSeconds(fadeInClip.length);
        }
    }
}