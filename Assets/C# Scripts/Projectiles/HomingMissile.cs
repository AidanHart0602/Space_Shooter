using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{

    private Transform _target = null;
    private GameObject _findTarget;
    private float _distance;
    private float _distanceToTarget = Mathf.Infinity;
    private float _missileSpeed = 300f;
    private float _rotationSpeed = 900f;

    [SerializeField]
    private Rigidbody2D _homingMissileRigidBody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FindTarget()
    {
        _findTarget = GameObject.FindGameObjectWithTag("Player");

    }

}
