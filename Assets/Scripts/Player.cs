using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour 
{
    Animator _anim;
    SpriteRenderer _sprite;
    private void Start()
    {
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        GameManager._Instance._Player = this;
    }

    private void Update()
    {
        tasking();
        walkToPos();
        animationsControl();
    }

    //Animations-----------------
    Vector3 _deltaPos;
    [SerializeField]
    Vector3 _dir;
    [SerializeField]
    float _angle;
    void animationsControl()
    {
        if(_deltaPos != transform.position)
        {
            _dir = (_deltaPos - transform.position).normalized;
            _angle = Vector3.SignedAngle(transform.forward, _dir, transform.up);
            _deltaPos = transform.position;
        }

        if (_walking)
        {
            _anim.SetInteger("Action", 1);

            if(_angle < 0)
            {
                _sprite.flipX = true;

                if(_angle < -35 && _angle > -45)
                {
                    _anim.SetInteger("Direction", 0);
                }

                if (_angle < -45 && _angle > -120)
                {
                    _anim.SetInteger("Direction", 1);
                }

                if (_angle < -120 && _angle > -145)
                {
                    _anim.SetInteger("Direction", 2);
                }
            }
            else
            {
                _sprite.flipX = false;

                if (_angle > 35 && _angle < 45)
                {
                    _anim.SetInteger("Direction", 0);
                }

                if (_angle > 45 && _angle < 120)
                {
                    _anim.SetInteger("Direction", 1);
                }

                if (_angle > 120 && _angle < 145)
                {
                    _anim.SetInteger("Direction", 2);
                }
            }
        }
        else
        {
            _anim.SetInteger("Action", 0);
        }
    }

    //Tasking--------------------
    [SerializeField]
    bool _tasking;
    [SerializeField]
    GameObject _taskinObject;
    [SerializeField]
    GameObject _taskBar;
    void tasking()
    {
        if(_tasking)
        {
            if(_walking)
            {
                _taskBar.GetComponent<Image>().fillAmount = 0;

                Color _color = Color.white;
                _color.a = 0.5f;
                _taskinObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = _color;
            }
            else
            {
                if(!_taskBar.activeInHierarchy)
                {
                    _taskBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3, 0));
                    _taskBar.SetActive(true);
                }
                else
                {
                    _taskBar.GetComponent<Image>().fillAmount += Time.deltaTime;

                    if(_taskBar.GetComponent<Image>().fillAmount >= 1)
                    {
                        Object.Destroy(_taskinObject);
                        _taskBar.SetActive(false);
                        _tasking = false;
                    }
                }
            }
        }
    }


    //WalkToPosition-------------
    NavMeshPath _path;
    [SerializeField]
    bool _walking;
    [SerializeField]
    int _walkIndex;
    [SerializeField]
    float _walkTang;
    [SerializeField]
    int _pathLenth;
    [SerializeField]
    GameObject _targetParticle;
    bool _pointOverUi;
    void walkToPos()
    {

        if (_path != null)
        {
            _pathLenth = _path.corners.Length;
        }
        else
        {
            _path = new NavMeshPath();
        }

        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch _t = Input.GetTouch(i);

                if (_t.phase == TouchPhase.Began)
                {
                    if (EventSystem.current.IsPointerOverGameObject(_t.fingerId))
                    {
                        _pointOverUi = true;
                    }

                }
            }
      
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 _mpos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(_mpos);
            RaycastHit hit = new RaycastHit();

            if(EventSystem.current.IsPointerOverGameObject())
            {
                _pointOverUi = true;
            }



            if (Physics.Raycast(ray, out hit, 100f))
            {
                //If Map
                if (hit.transform.tag == "Map" && _tasking == false && _pointOverUi == false)
                {
                    NavMeshPath _deltapath = new NavMeshPath();
                    NavMesh.CalculatePath(transform.position, hit.point, NavMesh.AllAreas, _deltapath);
                    if(_deltapath.status == NavMeshPathStatus.PathComplete)
                    {
                        _path = _deltapath;

                        if (_path.status == NavMeshPathStatus.PathComplete)
                        {
                            _walkIndex = 0;
                            _walkTang = 0;
                            _walking = true;
                            GameObject _newParticle = GameManager._Instance._PoolManager.NewPoolObject(_targetParticle);
                            _newParticle.transform.position = hit.point + new Vector3(0, 0.1f, 0);
                        }
                    }

                    //_targetPos = hit.point;
                    //Debug.DrawLine(ray.origin, hit.point);
                }

                //If Object
                if(hit.transform.tag == "IntObject" && _tasking == false && _pointOverUi == false)
                {
                    NavMeshPath _deltapath = new NavMeshPath();
                    NavMesh.CalculatePath(transform.position, hit.transform.position, NavMesh.AllAreas, _deltapath);
                    if (_deltapath.status == NavMeshPathStatus.PathComplete)
                    {
                        _path = _deltapath;

                        if (_path.status == NavMeshPathStatus.PathComplete)
                        {
                            _walkIndex = 0;
                            _walkTang = 0;
                            _walking = true;
                            _tasking = true;
                            _taskinObject = hit.transform.gameObject;
                            GameObject _newParticle = GameManager._Instance._PoolManager.NewPoolObject(_targetParticle);
                            _newParticle.transform.position = hit.point + new Vector3(0, 0.1f, 0);
                        }
                    }

                    //_targetPos = hit.point;
                    //Debug.DrawLine(ray.origin, hit.point);
                }
            }

            _pointOverUi = false;
        }

        //debugPath
        if(_path != null)
        {
            for (int i = 0; i < _path.corners.Length; i++)
            {
                Debug.DrawLine(_path.corners[i], _path.corners[Mathf.Clamp(i + 1, 0, _path.corners.Length - 1)]);
            }
        }

        if (_walking && _path != null && _path.corners.Length > 0)
        {
            float _distance = Vector3.Distance(_path.corners[_walkIndex], _path.corners[Mathf.Clamp(_walkIndex + 1,0,_path.corners.Length)]) * 0.1f;
            if (_walkTang < _distance)
            {
                _walkTang += Time.deltaTime;
                Vector3 _walkPos = Vector3.Lerp(_path.corners[_walkIndex], _path.corners[_walkIndex + 1], Mathf.InverseLerp(0, _distance, _walkTang));
                transform.position = _walkPos + new Vector3(0, 1.6f, 0);

            }
            else
            {
                if(_walkIndex < _path.corners.Length - 2)
                {
                    _walkIndex += 1;
                    _walkTang = 0;
                }
                else
                {
                    _walking = false;
                }

            }
        }

    }
}
