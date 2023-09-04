/*
 * Author: Wyatt Murray
 * Version: 1
 * Date: 9/4/23
 * 
 * Description:
 * Manages asynchronous loading and unloading of scenes with fade transitions.
 * 
 * Setup:
 * To set up in your project, create a scene with this script on an empty GameObject. 
 * Create a canvas under it and add a panel that covers the full screen and is colored black.
 * Remove all other scenes' EventSystems except this one.
 * Load the necessary data into the Inspector as needed and link the fade image and canvas.
 * Provide a fade duration of at least 0.25 seconds.
 * 
 * Additional Info:
 * - Ensure that referenced objects are available to call the two public methods.
 * - Remember that data must be preloaded unless you want to use GameObject.Find.
 * - This script is designed for loading single scenes with a player scene attached.
 * - There may be future improvements to support loading multiple scenes simultaneously.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoader : MonoBehaviour
{
    [System.Serializable]
    public struct DefinedSceneData
    {
        /// <summary>
        /// The name of the Scene as a string.
        /// </summary>
        public string sceneName;
        /// <summary>
        /// A toggle for labeling which scenes are persistant.
        /// </summary>
        public bool isPersistant;
    }

    /// <summary>
    /// A list of scene names to be used during loading and unloading operations.
    /// </summary>
    public List<DefinedSceneData> sceneNames = new List<DefinedSceneData>();

    /// <summary>
    /// The image used for fading transitions.
    /// </summary>
    [SerializeField] private Image fadeImage;

    /// <summary>
    /// The Canvas GameObject associated with this loader.
    /// </summary>
    [SerializeField] private GameObject canvas;

    /// <summary>
    /// The duration of the fade-in and fade-out transitions in seconds.
    /// </summary>
    [SerializeField] private float fadeDuration = 1.0f;


    [SerializeField] private bool isFading = false;


    private void Awake()
    {
        // Make sure the fadeImage is clear upon first load
        SetFadeAlpha(0.0f);
        StartCoroutine(LoadMainMenuAsync());
    }

    private IEnumerator LoadMainMenuAsync()
    {
        // Load the main menu scene additively
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNames[1].sceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        Cursor.visible = true;

        // Get the loaded main menu scene
        Scene mainMenuScene = SceneManager.GetSceneByName(sceneNames[1].sceneName);

        if (mainMenuScene.isLoaded)
        {
            // Find the Canvas within the loaded main menu scene
            MainMenuUI canvasInMainMenu = null;
            GameObject[] rootObjects = mainMenuScene.GetRootGameObjects();

            foreach (GameObject rootObject in rootObjects)
            {
                canvasInMainMenu = rootObject.GetComponentInChildren<MainMenuUI>();
                if (canvasInMainMenu != null)
                {
                    canvasInMainMenu.loader = this;

                    break;
                }
            }
            if (canvasInMainMenu != null)
            {
                // Do something with the Canvas
                Debug.Log("Found Canvas in the main menu scene.");
            }
            else
            {
                Debug.LogWarning("Canvas not found in the main menu scene.");
            }
        }
        else
        {
            Debug.LogError("Failed to load the main menu scene.");
        }
    }

    /// <summary>
    /// Loads a scene with a fade transition and an option to include the player scene.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load.</param>
    /// <param name="includePlayer">Whether to include the player scene.</param>
    public void LoadSceneWithFade(string sceneName, bool includePlayer)
    {
        if (!isFading)
        {
            StartCoroutine(FadeOutAndLoad(sceneName, includePlayer));
            Cursor.visible = false;
        }
    }

    /// <summary>
    /// Fades out and loads a new scene asynchronously.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load.</param>
    /// <param name="includePlayer">Whether to include the player scene.</param>
    private IEnumerator FadeOutAndLoad(string sceneName, bool includePlayer)
    {
        canvas.SetActive(true);
        isFading = true;
        // Fade out to black
        yield return StartCoroutine(Fade(0.0f, 1.0f));

        // Unload all scenes except for the Persistent Scene (if needed)
        UnloadAllScenesExceptPersistent();

        // Load the new scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if(includePlayer)
        {
            AsyncOperation playerLoad = SceneManager.LoadSceneAsync(sceneNames[0].sceneName, LoadSceneMode.Additive);

            while (!playerLoad.isDone)
            {
                yield return null;
            }

        }
        
        // Set the new scene as the active scene
        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(loadedScene);

        // Fade back in
        yield return StartCoroutine(Fade(1.0f, 0.0f));
        isFading = false;
        canvas.SetActive(false);
    }

    /// <summary>
    /// Fades the screen from one alpha value to another over a specified duration.
    /// </summary>
    /// <param name="startAlpha">The starting alpha value.</param>
    /// <param name="endAlpha">The ending alpha value.</param>
    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            SetFadeAlpha(alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final alpha value is set
        SetFadeAlpha(endAlpha);
    }

    /// <summary>
    /// Sets the alpha value of the fade image.
    /// </summary>
    /// <param name="alpha">The alpha value to set.</param>
    private void SetFadeAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
        }
    }

    /// <summary>
    /// Unloads all scenes that are not explicitly stated as persistant.
    /// </summary>
    private void UnloadAllScenesExceptPersistent()
    {
        Debug.Log("Unloading all scenes except those in sceneNames list.");

        int sceneCount = SceneManager.sceneCount;

        for (int i = 0; i < sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            // Check if the scene is marked as persistent in the sceneNames list
            bool isPersistent = false;
            foreach (var definedScene in sceneNames)
            {
                if (definedScene.sceneName == scene.name && definedScene.isPersistant)
                {
                    isPersistent = true;
                    break;
                }
            }

            if (!isPersistent)
            {
                AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(scene);

                unloadOperation.completed += (operation) =>
                {
                    if (unloadOperation.isDone)
                    {
                        Debug.Log("Unloaded scene: " + scene.name);
                    }
                    else
                    {
                        Debug.LogError("Failed to unload scene: " + scene.name);
                    }
                };
            }
        }
    }
}


