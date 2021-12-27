using System;
using UnityEngine;

public class SpinningPanel : MonoBehaviour
{
    #region Public Fields

    public float rotateSpeed = 120;
    #endregion

    private Transform _transform;
    
    #region Unity Methods

    private void Start()
    {
        _transform = gameObject.transform;
    }

    private void Update()
    {
        _transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }
    #endregion

    #region Private Methods
    #endregion
}
