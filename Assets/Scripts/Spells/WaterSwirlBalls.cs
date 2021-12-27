using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spells
{
    public class WaterSwirlBalls : MonoBehaviour
    {
        #region Public Fields

        public float dyingTime = 1.0f;
        public float speed = 1;
        public PSMeshRendererUpdater psUpdater;
        #endregion

        private Transform _transform;
        
        #region Unity Methods

        private void Awake()
        {
            // psUpdater.UpdateMeshEffect(gameObject);
        }

        private void Start()
        {
            dyingTime += (Random.value - 0.5f) / 5;
            _transform = GetComponent<Transform>();
            Destroy(gameObject, 1);
        }

        private void Update()
        {
            _transform.Translate(Vector3.forward * (Time.deltaTime * speed), Space.Self);
        }
        #endregion

        #region Private Methods
        
        
        
        #endregion
    }
}
