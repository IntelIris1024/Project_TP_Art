
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackState : AbstractEnemyState
{
    private float _slerpSpeed = 0.1f;
    //private GameObject _gun;
    //private GameObject _bulletPrefab;
    private Vector3 _actualAim;
    private System.Random random;
    private int _aimDistortion = 10; //The lower, the less distortion in degrees
    private float _aimSteadiness = 0.01f; // the higher, the more inaccurate
    private int _acceptableShotRange = 200; //Max distortion (in deg) in which enemy will still shoot
    private bool _acceptableShot;
    private bool _countFireDelta;
    private float _fireDelta;
    private Transform _turret;
    private Vector3 _turretVector;

    public AttackState(EnemyAgent agent, GameObject turretPoint) : base(agent)
    {
        
        _turret = turretPoint.transform;
        _turretVector = _turret.position;
    }

    public override void Update()
    {
        if (_agent.EnteredNewState)
        {
            _agent.EnteredNewState = false;
        }
        if (_agent.SeesTarget)
        {
            var differenceVector = _agent.Target.position - _agent.Parent.position;
            _agent.Parent.transform.rotation = Quaternion.Slerp(_agent.Parent.transform.rotation, Quaternion.LookRotation(differenceVector, Vector3.up), _slerpSpeed);
            _agent.Parent.transform.rotation = Quaternion.Euler(new Vector3(0f, _agent.Parent.transform.rotation.eulerAngles.y, 0f));
            LineOfAimHandler();
            Debug.Log("I should still be seeing the player!");
        }
        else
        {
            _agent.SetState(typeof(ChaseState));
        }
    }

    void LineOfAimHandler()
    {
        _turretVector = _turret.position;
        Vector3 differenceVector = _agent.Target.transform.position - _turretVector; //Vector to get length and height from
        Vector3 customVector = _agent.Parent.transform.forward; //Vector to use as a guide while aiming, has randomness
        customVector.Normalize();
        customVector *= differenceVector.magnitude;
        customVector.y = differenceVector.y;
        _actualAim = Vector3.Slerp(_actualAim, customVector, _aimSteadiness); //Vector that the actual shot is cast from, slerps between different customVec pos's
        _actualAim.Normalize();
        _actualAim *= differenceVector.magnitude;

        var targetAngle = Vector3.Angle(_actualAim, differenceVector);
        if (targetAngle > _acceptableShotRange)
        {
            //Debug.DrawLine(_agent.Parent.transform.position, _agent.Parent.transform.position + _actualAim, Color.red);
            Debug.DrawLine(_turretVector, _turretVector + _actualAim, Color.red);
            //Sees if Target is in acceptable range, if not, no ray is cast
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(_turretVector, _actualAim, out hit))
            {
                Debug.Log("Aiming at player!");
                _agent.StartCoroutine(RestartLevel());
            }
        }

    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }
}

