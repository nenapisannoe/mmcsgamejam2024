using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController Instance;

    [Header("Controllers")]
    public CameraController CameraController;
    public PlayerController PlayerController;
    [Header("Game Scenes")]
    public GameObject MainMenuScene;
    public GameObject LevelScene;
    public List<LevelController> Levels = new List<LevelController>();
    private int currentLevelIndex = -1;
    [NonSerialized] public LevelController CurrentLevel;
    [Header("UI Dialogs")]
    public StartDialog StartDialog;
    public VictoryDialog VictoryDialog;
    public DefeatDialog DefeatDialog;
    public ProjectorControllerDialog ProjectorControllerDialog;
    
    //пока что разрешаем только 1 диалог одновременно, при открытии нового диалога старый закрывается
    private MonoBehaviour currentDialog;
    
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        ShowMainMenu();
    }

    #region Main

    public void ShowMainMenu() {
        LevelScene.SetActive(false);
        UnloadLevel();
        MainMenuScene.SetActive(true);
        ShowDialog(StartDialog);
    }

    public void ShowLevel() {
        MainMenuScene.SetActive(false);
        HideDialog();
        LoadLevel(0);
        LevelScene.SetActive(true);
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
        PlayerController.SetColor(Color.black);
        PlayerController.ChangeControlsAvailable(true);
        PlayerController.ChangeControllerEnabled(true);
        CameraController.AssignTarget(PlayerController.transform);
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

    #endregion
    
}