using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : AdvancedFSM
{
    public static GameManager Instance { get; private set; }
    public UIManager UIManager { get; private set; }
    public SceneManager SceneManager { get; private set; }
    public AudioManager AudioManager { get; private set; }
    //public EnemyManager EnemyManager { get; private set; }

    public PlayerData playerData;


    [Space]
    [Title("Audio", TextAlignment.Left, TextColour.White, 20)]
    [Separator]

    [SerializeField]
    [Tooltip("Main Menu Background Music")]
    private AudioClip mainMenuBGMusic;

    [Space]
    [Title("UI", TextAlignment.Left, TextColour.White, 20)]
    [Separator]

    [SerializeField]
    [Tooltip("Main Menu Starting Panel")]
    public CanvasGroup mainMenuGroup;

    [SerializeField]
    [Tooltip("Main Menu Option Panel")]
    public CanvasGroup optionsGroup;

    [SerializeField]
    [Tooltip("Saved Game Panel")]
    public CanvasGroup savedGamesGroup;

    [Space]
    [Title("Scenes", TextAlignment.Left, TextColour.White, 20)]
    [Separator]

    [SerializeField]
    [Tooltip("Credit Scene Name")]
    public string creditSceneName;

    [SerializeField]
    [Tooltip("Tutorial Scene Name")]
    public string tutorialSceneName;

    public SceneObjects sceneObjects;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializePlayerData();

        // Initialize managers
        UIManager = GetComponentInChildren<UIManager>();
        SceneManager = GetComponentInChildren<SceneManager>();
        AudioManager = GetComponentInChildren<AudioManager>();
        //EnemyManager = GetComponentInChildren<EnemyManager>();
    }

    protected override void Initialize()
    {
        ConstructFSM();
    }

    protected override void FSMUpdate()
    {
        elapsedTime += Time.deltaTime;
        CurrentState.Reason(transform, transform);
        CurrentState.Act(transform, transform);
    }

    private void ConstructFSM()
    {
        // Create States
        //
        // Create Main Menu State
        GM_MainMenuState mainMenu = new GM_MainMenuState(mainMenuBGMusic, creditSceneName);
        
        // Create Start State
        GM_StartState start = new GM_StartState(sceneObjects);

        // Create Play State
        GM_PlayState play = new GM_PlayState();

        // Create Pause State
        GM_PauseState pause = new GM_PauseState();

        // Create End State
        GM_EndState end = new GM_EndState(sceneObjects);


        // Add Transitions
        //
        // Add Transitions out of Main Menu State
        mainMenu.AddTransition(TransitionType.Starting, FSMStateType.Start);

        // Transitions out of Start state
        start.AddTransition(TransitionType.Playing, FSMStateType.Play);
        start.AddTransition(TransitionType.Pausing, FSMStateType.Pause);
        start.AddTransition(TransitionType.Ending, FSMStateType.End);

        // Transitions out of Play State
        play.AddTransition(TransitionType.Starting, FSMStateType.Start);
        play.AddTransition(TransitionType.Pausing, FSMStateType.Pause);
        play.AddTransition(TransitionType.Ending, FSMStateType.End);

        // Transitions out of Pause state
        pause.AddTransition(TransitionType.Starting, FSMStateType.Start);
        pause.AddTransition(TransitionType.Playing, FSMStateType.Play);
        pause.AddTransition(TransitionType.Ending, FSMStateType.End);

        // Transitions out of End State
        end.AddTransition(TransitionType.Starting, FSMStateType.Start);
        end.AddTransition(TransitionType.Playing, FSMStateType.Play);
        end.AddTransition(TransitionType.Pausing, FSMStateType.Pause);

        // Add States to List
        //
        AddState(mainMenu);
        AddState(start);
        AddState(play);
        AddState(pause);
        AddState(end);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void InitializePlayerData()
    {
        playerData = new PlayerData(100f); // Default values
    }

    public void SavePlayerData(float health)
    {
        playerData.health = health;
    }
}

[Serializable] public class SceneObjects : IEnumerable<GameObject>
{
    public GameObject player;
    public List<GameObject> testCube;

    //I added these so that I can call on the SceneObjects in other states - Luka
    public IEnumerator<GameObject> GetEnumerator()
    {
        if (player != null)
        {
            yield return player;
        }

        if(testCube != null)
        {
            foreach(var cube in testCube)
            {
                yield return cube;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
}