using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

public class ElevatorFinish : MonoBehaviour {

    public PlayableDirector Director;

    public async void Play() {
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        Director.Play();
    }
    
}