using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GM_MainMenuState : FSMState
{
    private enum eSelectedState
    {
        MAIN_MENU,
        NEW_GAME,
        CONTINUE_GAME,
        LOAD_GAME,
        OPTIONS,
        CREDITS,
        EXIT
    }

    private eSelectedState selectedState;

    private AudioClip bgMusicClip;

    private PlayerInput playerInput;
    private InputAction skipCredits;

    private Animator creditsAnimator;

    private string creditsSceneName;

    private Button newGameBtn = null;
    private Button continueBtn = null;
    private Button loadGameBtn = null;
    private Button optionBtn = null;
    private Button creditsBtn = null;
    private Button exitBtn = null;
    private Button optionsMenuBackBtn = null;
    private Button loadMenuBackBtn = null;

    private float skipDelay = 5f;
    private float timer = 0f;

    private bool creditsLoaded;
    private bool creditsInitialized;
    private bool skipEnabled;

    // Constructor
    public GM_MainMenuState(AudioClip clip, string sceneName)
    {
        stateType = FSMStateType.MainMenu;
        selectedState = eSelectedState.MAIN_MENU;

        bgMusicClip = clip;
        creditsSceneName = sceneName;

        EnterStateInit();
    }


    public override void EnterStateInit()
    {
        creditsLoaded = false;
        skipEnabled = false;
        creditsInitialized = false;

        InitializeButtonReferences();

        Debug.Log("Main Menu State Entered.");
    }

    public override void Reason(Transform player, Transform gm)
    {
        switch (selectedState)
        {
            case eSelectedState.MAIN_MENU:
                // Do nothing...
                break;
            case eSelectedState.NEW_GAME:
                // Start new Game File
                //
                gm.GetComponent<GameManager>().PerformTransition(TransitionType.Starting);
                break;
            case eSelectedState.CONTINUE_GAME:
                // Continue Last Played Game File
                //
                gm.GetComponent<GameManager>().PerformTransition(TransitionType.Starting);
                break;
            case eSelectedState.LOAD_GAME:
                // Load Game From File
                //
                ///gm.GetComponent<GameManager>().PerformTransition(TransitionType.Starting);
                break;
            case eSelectedState.OPTIONS:
                // Do nothing...
                break;
            case eSelectedState.CREDITS:
                // Do nothing...
                break;
            case eSelectedState.EXIT:
                // Do nothing...
                break;
            default:
                break;
        }
    }

    public override void Act(Transform player, Transform npc)
    {
        // Play Background Music
        GameManager.Instance.AudioManager.PlayMusic(bgMusicClip, true);

        // Options Selected

        // Credits Selected
        if (creditsLoaded)
        {
            InitializeCredits();
            HandleSkipTimer();

            if(creditsAnimator != null)
            {
                if (creditsAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !creditsAnimator.IsInTransition(0))
                {
                    UnloadCredits();
                }
            }
        }
    }

    /// <summary>
    /// Initializes all buttons on the main menu and their listeners
    /// </summary>
    private void InitializeButtonReferences()
    {
        // Start New Game Button
        if (newGameBtn == null) newGameBtn = GameObject.Find("new game button").GetComponent<Button>();

        newGameBtn.onClick.RemoveAllListeners();
        newGameBtn.onClick.AddListener(StartNewGame);

        // Continue Last Game Button
        if (continueBtn == null) continueBtn = GameObject.Find("continue button").GetComponent<Button>();

        continueBtn.onClick.RemoveAllListeners();
        continueBtn.onClick.AddListener(ContinueGame);

        // Load Game Button
        if (loadGameBtn == null) loadGameBtn = GameObject.Find("load game button").GetComponent<Button>();

        loadGameBtn.onClick.RemoveAllListeners();
        loadGameBtn.onClick.AddListener(LoadGame);

        // Options Button
        if (optionBtn == null) optionBtn = GameObject.Find("options button").GetComponent<Button>();

        optionBtn.onClick.RemoveAllListeners();
        optionBtn.onClick.AddListener(GameManager.Instance.UIManager.OptionsPressed);

        // Credit Button
        if (creditsBtn == null) creditsBtn = GameObject.Find("credits button").GetComponent<Button>();

        creditsBtn.onClick.RemoveAllListeners();
        creditsBtn.onClick.AddListener(LoadCredits);

        // Exit Button
        if (exitBtn == null) exitBtn = GameObject.Find("edit game button").GetComponent<Button>();

        exitBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.AddListener(GameManager.Instance.UIManager.ExitGamePressed);

        // Options Menu Back Button
        if (optionsMenuBackBtn == null) optionsMenuBackBtn = GameObject.Find("go back button").GetComponent<Button>();

        optionsMenuBackBtn.onClick.RemoveAllListeners();
        optionsMenuBackBtn.onClick.AddListener(GameManager.Instance.UIManager.MainMenuBackPressed);

        // Load Menu Back Button
        if (loadMenuBackBtn == null) loadMenuBackBtn = GameObject.Find("load menu back button").GetComponent<Button>();

        loadMenuBackBtn.onClick.RemoveAllListeners();
        loadMenuBackBtn.onClick.AddListener(GameManager.Instance.UIManager.MainMenuBackPressed);
    }

    /// <summary>
    /// Loads new game using UIManager
    /// </summary>
    private void StartNewGame()
    {
        selectedState = eSelectedState.NEW_GAME;

        if(skipCredits != null) skipCredits.performed -= UnloadCredits;

        RemoveAllListeners();

        GameManager.Instance.UIManager.StartNewGame();
    }

    /// <summary>
    /// Loads last saved game using the UIManager
    /// </summary>
    private void ContinueGame()
    {
        selectedState = eSelectedState.CONTINUE_GAME;

        //skipCredits.performed -= UnloadCredits; --commented out for testing

        //RemoveAllListeners(); --commented out for testing

        GameManager.Instance.UIManager.ContinueLastGame();
    }

    /// <summary>
    /// Loads list of saved games using the UIManager
    /// </summary>
    private void LoadGame()
    {
        selectedState = eSelectedState.LOAD_GAME;
        //RemoveAllListeners();

        GameManager.Instance.UIManager.LoadGame();
    }

    /// <summary>
    /// Initializes specific components only in the credits scene
    /// </summary>
    private void InitializeCredits()
    {
        if (creditsInitialized) return;

        // Check if the "Everybody" object has been loaded
        if (GameObject.Find("Everybody") == null) return;

        GameObject everybody = GameObject.Find("Everybody");

        // Assign the player input component to handle skipping
        playerInput = everybody.GetComponent<PlayerInput>();

        skipCredits = playerInput.actions["Pause"];
        skipCredits.performed += UnloadCredits;

        creditsAnimator = everybody.GetComponent<Animator>();

        creditsInitialized = true;
    }

    /// <summary>
    /// Loads the credits scene on top of main menu through the UIManager
    /// </summary>
    private void LoadCredits()
    {
        creditsLoaded = true;
        GameManager.Instance.UIManager.CreditsPressed();
    }

    /// <summary>
    /// Unloads the credit scene using the SceneManager
    /// </summary>
    /// <param name="ctx"></param>
    private void UnloadCredits(InputAction.CallbackContext ctx)
    {
        if (!skipEnabled) return;

        creditsInitialized = false;
        creditsLoaded = false;
        skipEnabled = false;

        creditsAnimator = null;

        timer = 0f;

        //GameManager.Instance.SceneManager.UnloadScene(creditsSceneName);
        GameManager.Instance.SceneManager.UnloadSceneWithTransition(creditsSceneName);
    }

    /// <summary>
    /// Unloads the credit scene using the SceneManager (Without InputAction)
    /// </summary>
    private void UnloadCredits()
    {
        if (!skipEnabled) return;

        creditsInitialized = false;
        creditsLoaded = false;
        skipEnabled = false;

        creditsAnimator = null;

        timer = 0f;

        //GameManager.Instance.SceneManager.UnloadScene(creditsSceneName);
        GameManager.Instance.SceneManager.UnloadSceneWithTransition(creditsSceneName);
    }

    /// <summary>
    /// Handles the timer to enable exiting the credits
    /// </summary>
    private void HandleSkipTimer()
    {
        if (!skipEnabled)
        {
            timer += Time.deltaTime;
            if (timer >= skipDelay)
            {
                skipEnabled = true;
            }
        }
    }

    /// <summary>
    /// Removes all button listener events
    /// </summary>
    private void RemoveAllListeners()
    {
        newGameBtn.onClick.RemoveAllListeners();
        continueBtn.onClick.RemoveAllListeners();
        loadGameBtn.onClick.RemoveAllListeners();
        optionBtn.onClick.RemoveAllListeners();
        creditsBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.RemoveAllListeners();
        optionsMenuBackBtn.onClick.RemoveAllListeners();
        loadMenuBackBtn.onClick.RemoveAllListeners();
    }
}