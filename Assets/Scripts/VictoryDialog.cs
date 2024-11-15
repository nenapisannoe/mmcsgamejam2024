using UnityEngine;

public class VictoryDialog : MonoBehaviour {
    
    public void OnNextButtonClicked() {
        GameController.Instance.RestartLevel();
    }
    
    public void OnExitButtonClicked() {
        GameController.Instance.ShowMainMenu();
    }
        
}