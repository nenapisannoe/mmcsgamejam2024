using UnityEngine;

public class DefeatDialog : MonoBehaviour {
    
    public void OnRestartButtonClicked() {
        GameController.Instance.RestartLevel();
    }
    
    public void OnExitButtonClicked() {
        GameController.Instance.ShowMainMenu();
    }
        
}