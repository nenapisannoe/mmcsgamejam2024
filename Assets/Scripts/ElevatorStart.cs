using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

public class ElevatorStart : MonoBehaviour {

    public Transform StartPoint;

    public PlayableDirector Director;

    public async void Play() {
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        Director.Play();
    }
    
}