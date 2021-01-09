using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntObject : MonoBehaviour 
{
    Vector3 _startPos;
    private void Start()
    {
        _startPos = transform.position;
    }

    private void OnDestroy()
    {
        int _coinCoint = Random.Range(1, 4);

        for (int i = 0; i < _coinCoint; i++)
        {
            GameObject _new = GameManager._Instance._PoolManager.NewPoolObject(GameManager._Instance._GoldPrefab);
            _new.transform.position = _startPos + new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
            Vector3 _targetPos = Camera.main.ScreenToWorldPoint(GameManager._Instance._UiManager._GoldBar.transform.position) + (Camera.main.transform.forward.normalized * 50);
            _new.GetComponent<FollowToTarget>()._targetObject = _targetPos;
        }
    }
}
