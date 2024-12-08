using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController Instance;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    [Header("Controllers")]
    public CameraController CameraController;
    public PlayerController PlayerController;
    [Header("Game Scenes")]
    public Paint PaintPrefab;
    public GameObject LevelScene;
    public List<LevelController> Levels = new List<LevelController>();
    private int currentLevelIndex = -1;
    public int OVERRIDE_LEVEL = 0;
    [NonSerialized] public LevelController CurrentLevel;
    public LineRenderer ShotLine;
    [Header("UI Dialogs")]
    public StartDialog StartDialog;
    public VictoryDialog VictoryDialog;
    public DefeatDialog DefeatDialog;
    public ProjectorControllerDialog ProjectorControllerDialog;

    public Action onMainMenuShow;
    public Action onLevelShow;

    public event Action onPlayerHitFloor;

    //пока что разрешаем только 1 диалог одновременно, при открытии нового диалога старый закрывается
    private MonoBehaviour currentDialog;

    private void Awake() {
        Instance = this;

        Application.targetFrameRate = 60;
    }

    private void Start() {
        ShowMainMenu();
    }

    #region Main

    public void ShowMainMenu() {
        PlayerController.transform.position = new Vector3(0f, 0f, 0f); //camera looks at player
        LevelScene.SetActive(false);
        UnloadLevel();
        ShowDialog(StartDialog);
        onMainMenuShow?.Invoke();
    }

    public void ShowLevel() {
        HideDialog();
        var level = 0;
        #if UNITY_EDITOR
        level = OVERRIDE_LEVEL;
        #endif
        LoadLevel(level);
        LevelScene.SetActive(true);
        onLevelShow?.Invoke();
    }

    public void RestartLevel() {
        HideDialog();
        if (currentLevelIndex >= 0) {
            LoadLevel(currentLevelIndex);
        }
        else {
            ShowMainMenu();
        }
    }
    
    public void NextLevel() {
        HideDialog();
        if (currentLevelIndex >= 0 && Levels.Count >= currentLevelIndex) {
            LoadLevel(currentLevelIndex + 1);
        }
        else {
            ShowMainMenu();
        }
    }

    #endregion

    #region UI - Dialogs

    public void ShowProjectorControllerDialog(LightProjectorControllerData data) {
        ProjectorControllerDialog.Init(data);
        ShowDialog(ProjectorControllerDialog);
        PlayerController.ChangeControlsAvailable(false);
    }

    public void HideProjectorControllerDialog() {
        HideDialog();
        PlayerController.ChangeControlsAvailable(true);
    }

    private void ShowDialog(MonoBehaviour dialog) {
        HideDialog();
        currentDialog = dialog;
        currentDialog.gameObject.SetActive(true);
    }

    private void HideDialog() {
        if (currentDialog != null) {
            currentDialog.gameObject.SetActive(false);
            currentDialog = null;
        }
    }

    #endregion

    #region Scenes

    private void LoadLevel(int index) {
        UnloadLevel();
        currentLevelIndex = index;
        CurrentLevel = Instantiate(Levels[index], LevelScene.transform);
        CurrentLevel.Init(PlayerController);
        PlayerController.gameObject.SetActive(true);
        CameraController.Assign(CurrentLevel.m_CameraConfiner);
        CurrentLevel.m_StartElevator.Play();
        PlayerController.PlayStart();
    }

    private void UnloadLevel() {
        currentLevelIndex = -1;
        if (CurrentLevel != null) {
            Destroy(CurrentLevel.gameObject);
            CurrentLevel = null;
        }
        PlayerController.ChangeControlsAvailable(false);
        PlayerController.ChangeControllerEnabled(false);
        PlayerController.gameObject.SetActive(false);
        CameraController.Reset();
    }

    #endregion

    #region Level Logic

    public void LevelComplete() {
        PlayerController.ChangeControlsAvailable(false);
        ShowDialog(VictoryDialog);
    }

    public void LevelFailed() {
        PlayerController.ChangeControlsAvailable(false);
        ShowDialog(DefeatDialog);
    }

    public async void ShotCharacter(Transform shotOrigin, Color color) {
        ShotLine.SetPositions(new [] {
            shotOrigin.position,
            PlayerController.GetComponentInChildren<BoxCollider>().transform.position
        });
        ShotLine.sharedMaterial.EnableKeyword("_EMISSION");
        ShotLine.sharedMaterial.SetColor(BaseColor, color);
        ShotLine.sharedMaterial.SetColor(EmissionColor, color * 15f);
        ShotLine.gameObject.SetActive(true);
        PlayerController.Kill();
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        ShotLine.gameObject.SetActive(false);
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        LevelFailed();
    }

    public async void KillEnemy(EnemyController enemyController) {
        //TODO: player attack anim
        var paint = Instantiate(PaintPrefab, CurrentLevel.transform);
        paint.transform.position = enemyController.transform.position;
        paint.SetColor(enemyController.Color);
        await enemyController.Death();
        Destroy(enemyController.gameObject);
    }

    #endregion

}
