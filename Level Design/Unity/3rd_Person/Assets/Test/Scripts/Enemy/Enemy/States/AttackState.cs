
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackState : AbstractEnemyState
{
    private float _slerpSpeed = 0.1f;
    private System.Random random;
    private int _aimDistortion = 1; //The lower, the less distortion in degrees
    private float _aimSteadiness = 0.1f; // the higher, the more inaccurate
    private int _acceptableShotRange = 10; //Max distortion (in deg) in which enemy will still shoot
    private bool _acceptableShot;
    private bool _countFireDelta;
    private float _fireDelta;
    private Transform _turret;
    private Vector3 _turretVector;

    public AttackState(EnemyAgent agent, GameObject turretPoint) : base(agent)
    {
        random = new System.Random();
        //_gun = gun;
        //_bulletPrefab = bulletPrefab;
        _turret = turretPoint.transform;
        _turretVector = _turret.position;
    }

    public override void Update()
    {
        if (_agent.EnteredNewState)
        {
            _agent.EnteredNewState = false;
        }

        LineOfAimHandler();
    }
  

    void LineOfAimHandler()
    {
        RaycastHit hit;
        if (Physics.Raycast(_agent.transform.position, _agent.Target.transform.position-_agent.Parent.transform.position, out hit))
        {
            if (!_countFireDelta) _countFireDelta = true;
            if (_fireDelta == 0)
            {
                //_agent.CreateParticlesRotated(_muzzleFlash, _turretVector, Quaternion.LookRotation(_actualAim, Vector3.up));
                if (hit.collider.gameObject.tag == "Player")
                {
                    Debug.Log("Player was hit!");
                    _agent.StartCoroutine(RestartLevel());
                    // _agent.CreateParticles(_bloodParticles, hit.point);
                }
                else
                {
                    Debug.Log("MISSED COZ I SUCK");
                   // _agent.CreateParticles(_impactParticles, hit.point);
                }
            }
            if (_countFireDelta)
            {
                _fireDelta += Time.deltaTime;
            }
            if (_fireDelta >= 1f)
            {
                _fireDelta = 0;
                _countFireDelta = false;
            }
        }
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }
}

