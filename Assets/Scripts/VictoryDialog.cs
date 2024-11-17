using UnityEngine;

public class VictoryDialog : MonoBehaviour {
    
    public void OnNextButtonClicked() {
        GameController.Instance.NextLevel();
    }
    
    public void OnExitButtonClicked() {
        GameController.Instance.ShowMainMenu();
    }
        
}