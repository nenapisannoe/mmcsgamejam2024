using UnityEngine;

public class LevelController : MonoBehaviour {
    
    public Transform m_PlayerSpawnPoint;
    public Collider m_LevelEndCollider;
    
    private PlayerController player;

    public void Init(PlayerController playerController) {
        player = playerController;
        player.transform.position = m_PlayerSpawnPoint.position;
    }
    
}