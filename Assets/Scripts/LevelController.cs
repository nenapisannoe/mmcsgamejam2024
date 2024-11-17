using UnityEngine;

public class LevelController : MonoBehaviour {

    public ElevatorStart m_StartElevator;
    public BoxCollider m_CameraConfiner;
    
    private PlayerController player;

    public void Init(PlayerController playerController) {
        player = playerController;
        player.transform.position = m_StartElevator.StartPoint.position;
    }
    
}