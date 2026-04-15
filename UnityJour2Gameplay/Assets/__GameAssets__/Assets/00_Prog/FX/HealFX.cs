
using System;
using UnityEngine;
using UnityEngine.VFX;

public class HealFX : MonoBehaviour
{
        private Transform _target;
        private VisualEffect _healFx;

        private void Awake()
        {
                _healFx = GetComponent<VisualEffect>();
                _target = GameManager.Instance.GetPlayer().transform;
                
                Destroy(gameObject, 5.0f);
        }

        private void SetTarget(Transform target)
        {
                _target = target;
        }

        private void Update()
        {
                _healFx.SetVector3("Direction Particle",  Vector3.Normalize(_target.position + new Vector3(0f,0.5f,0f) - transform.position) / 2);
        }
}