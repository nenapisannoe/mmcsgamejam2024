using UnityEngine;

public class StartDialog : MonoBehaviour {

    public void OnStartButtonClicked() {
        GameController.Instance.ShowLevel();
    }

}