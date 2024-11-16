using System;
using UnityEngine;

public class EnemyAnimationHandler : MonoBehaviour {

    public event Action OnShotTrigger; 

    public void ShotTrigger() {
        OnShotTrigger?.Invoke();
    }
    
}