using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowToTarget : MonoBehaviour
{
    public Vector3 _targetObject;
    public int _value;
    [SerializeField]
    float _tang;
    float _speed;
    private void Update()
    {
        if(_tang < 0.4f)
        {
            _tang += Time.deltaTime * _speed;
            transform.position = Vector3.Lerp(transform.position, _targetObject, _tang);
        }
        else
        {
            GameManager._Instance._Gold += _value;
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        _value = Random.Range(10, 20);
        _speed = Random.Range(0.5f, 1f);
        _tang = 0;
    }
}
