using System;
using UnityEngine;

namespace Game.EffectScripts {

    public enum RotateAxis {
        X,
        Y,
        Z
    }

    public class RotateEffect : MonoBehaviour {
        [SerializeField] private GameObject m_TargetObject;
        [SerializeField] private bool m_StartRandom;
        [SerializeField] private RotateAxis m_RotateAxis = RotateAxis.Y;
        [SerializeField][Range(-90f, 90f)] private float m_RotateSpeed = 30f; //градусов в секунду

        Transform t;
        private Vector3 rotateVector;

        private void Start() {
            rotateVector = GetVector();
            if (m_TargetObject != null) {
                t = m_TargetObject.transform;
                if (m_StartRandom) {
                    var rand = Utils.Random.Next(360);
                    t.localRotation = Quaternion.Euler(rotateVector * rand);
                }
            }
        }

        private Vector3 GetVector() {
            return m_RotateAxis switch {
                RotateAxis.X => Vector3.right,
                RotateAxis.Y => Vector3.up,
                RotateAxis.Z => Vector3.forward,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void Update() {
            if (m_TargetObject != null && m_TargetObject.activeSelf) {
                t.Rotate(rotateVector * (m_RotateSpeed * Time.deltaTime));
            }
        }

    }

}