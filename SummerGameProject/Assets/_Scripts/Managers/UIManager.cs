using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Main Menu Functions
    public void StartNewGame()
    {
        //GameManager.Instance.SceneManager.LoadScene(GameManager.Instance.tutorialSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        GameManager.Instance.SceneManager.LoadSceneWithTransition(GameManager.Instance.tutorialSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void ContinueLastGame()
    {
        throw new System.NotImplementedException("Continue Game not Implemented.");
    }

    public void LoadGame()
    {
        CanvasGroup startGroup = GameManager.Instance.mainMenuGroup;
        CanvasGroup savedGamesGroup = GameManager.Instance.savedGamesGroup;

        startGroup.alpha = 0;
        startGroup.interactable = false;
        startGroup.blocksRaycasts = false;

        savedGamesGroup.alpha = 1;
        savedGamesGroup.interactable = true;
        savedGamesGroup.blocksRaycasts = true;

        throw new System.NotImplementedException("Load Game Saves not Implemented.");
    }

    public void OptionsPressed()
    {
        CanvasGroup startGroup = GameManager.Instance.mainMenuGroup;
        CanvasGroup optionGroup = GameManager.Instance.optionsGroup;

        startGroup.alpha = 0;
        startGroup.interactable = false;
        startGroup.blocksRaycasts = false;

        optionGroup.alpha = 1;
        optionGroup.interactable = true;
        optionGroup.blocksRaycasts = true;

        Debug.Log("Options Pressed.");
    }

    public void MainMenuBackPressed()
    {
        CanvasGroup startGroup = GameManager.Instance.mainMenuGroup;
        CanvasGroup optionGroup = GameManager.Instance.optionsGroup;
        CanvasGroup savedGamesGroup = GameManager.Instance.savedGamesGroup;

        if (optionGroup != null && optionGroup.alpha > 0)
        {
            optionGroup.alpha = 0;
            optionGroup.interactable = false;
            optionGroup.blocksRaycasts = false;
        }
        else if(savedGamesGroup != null && savedGamesGroup.alpha > 0)
        {
            savedGamesGroup.alpha = 0;
            savedGamesGroup.interactable = false;
            savedGamesGroup.blocksRaycasts = false;
        }
        startGroup.alpha = 1;
        startGroup.interactable = true;
        startGroup.blocksRaycasts = true;
    }

    public void CreditsPressed()
    {
        GameManager.Instance.SceneManager.LoadScene(GameManager.Instance.creditSceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    public void ExitGamePressed()
    {
        Debug.Log("Exiting game.");
        GameManager.Instance.ExitGame();
    }
    #endregion
}
