using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgent : MonoBehaviour
{
    private readonly Dictionary<Type, AbstractEnemyState> _stateCache = new Dictionary<Type, AbstractEnemyState>();
    private Vector3 _debugLine;
    private AbstractEnemyState _state;
    public int SightConeAngle = 360;
    public int SightRange = 10;
    public GameObject TargetObject;
    public float SlerpSpeed = 0.1f;
    public NavigationPath PatrolPath = null;
    public GameObject SharedAI;
    public int OnDestroyHelpRange = 0;
    private SharedEnemyAI _sharedAI;

    private Vector3 _stepVector = new Vector3(0, 0.2f, 0);
    private bool _movingUp = false;
    private int _upLimit = 50;
    private int _upCounter = 0;

    public UnityEngine.AI.NavMeshAgent NavAgent { get; set; }
    public Rigidbody Parent { get; private set; }
    public Rigidbody Target { get; private set; }
    public bool EnteredNewState { get; set; }
    public Vector3 LastSeenTargetPosition { get; set; }
    public bool SeesTarget { get; private set; }

    
    private void Start()
    {
        SightConeAngle = 360;
        SightRange = 7;
        OnDestroyHelpRange = 1;
        Parent = GetComponent<Rigidbody>();
        Target = TargetObject.GetComponent<Rigidbody>();
        NavAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _sharedAI = SharedAI.GetComponent<SharedEnemyAI>();
        _sharedAI.RegisterAgent(this, gameObject);
        List<GameObject> childObjects = GetChildrenComponents();
        _stateCache[typeof (PatrolState)] = new PatrolState(this, Parent.rotation, PatrolPath);
        _stateCache[typeof (ChaseState)] = new ChaseState(this);
        _stateCache[typeof (AttackState)] = new AttackState(this, childObjects.Find(child => child.name == "DroneTurretPoint"));
        _stateCache[typeof (ReturnState)] = new ReturnState(this, Parent.position, Parent.rotation);
        _stateCache[typeof (LookoutState)] = new LookoutState(this);

        SetState(typeof (PatrolState));
    }

    private List<GameObject> GetChildrenComponents()
    {
        List<GameObject> childrenObjects = new List<GameObject>();
        for (int i = 0; i < Parent.transform.childCount; i++)
        {
            if (Parent.transform.GetChild(i).name == "DroneTurretPoint") childrenObjects.Add(Parent.transform.GetChild(i).gameObject);
        }
        return childrenObjects;
    }

    public void SetState(Type pState)
    {
        SetState(_stateCache[pState]); //backwards compatible
    }

    public void SetState(AbstractEnemyState pState)
    {
        Debug.Log("Switching state to:" + pState.ToString());
        EnteredNewState = true;
        SetSeeTarget();
        _state = pState;
    }

    public AbstractEnemyState GetState()
    {
        return _state;
    }
    private void FixedUpdate()
    {
        if (Parent != null)
        {
            SetSeeTarget();
            _state.Update();
        }
    }

    private void SetSeeTarget()
    {
        if (LookForTarget())
        {
            LastSeenTargetPosition = Target.position;
            Debug.DrawLine(Parent.position, Target.position, Color.yellow);
            SeesTarget = true;
        }
        else
        {
            SeesTarget = false;
        }
    }


    // Update is called once per frame
    private bool LookForTarget()
    {
        var differenceVec = Target.transform.position - Parent.transform.position;

        if (differenceVec.magnitude <= SightRange) //Sees if Target is even in range
        {
            Debug.DrawLine(Parent.transform.position, Target.transform.position, Color.cyan);
            var targetAngle = Vector3.Angle(differenceVec, Parent.transform.forward);
            if (targetAngle > SightConeAngle)
            {
                return false; //Sees if Target is in sight cone
            }
            RaycastHit hit;

            if (Physics.Raycast(Parent.position, differenceVec, out hit, differenceVec.magnitude))
            {
                if (hit.collider.gameObject.tag != "Player") return false; //Checks if anything is between the Target and the Parent
            }
            return true;
        }
        return false;
    }

    

     void OnDestroy()
    {
        Debug.Log("Enemy Destroyed, Emergency Search Initiated!");
      //  _sharedAI.SearchInRangeAgent(this, OnDestroyHelpRange);
        //_sharedAI.UnRegisterAgent(this);
    }

}