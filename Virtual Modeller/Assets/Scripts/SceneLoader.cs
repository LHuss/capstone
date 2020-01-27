﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {

    public GameObject loadingScreen;
    public Slider slider;
    public Text loadingText;

    public void LoadScene (int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    // coroutine to load scene asynchronously
    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            // 0-0.9 loading, 0.9-1 activation
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            loadingText.text = progress * 100f + "%";
            // wait until next frame before continuing
            yield return null;
        }
    }

    // coroutine to load scene asynchronously
    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            // 0-0.9 loading, 0.9-1 activation
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            loadingText.text = progress * 100f + "%";
            // wait until next frame before continuing
            yield return null;
        }
    }

}
