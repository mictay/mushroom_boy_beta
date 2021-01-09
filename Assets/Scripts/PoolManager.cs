using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class poolType
{
    public List<GameObject> _list;

    public poolType()
    {
        _list = new List<GameObject>();
    }
}

public class PoolManager : MonoBehaviour 
{
	private void Start()
	{
        GameManager._Instance._PoolManager = this;
        _Poollist = new List<poolType>();
	}


    public List<poolType> _Poollist;
    public GameObject NewPoolObject(GameObject _InputPrefabObject)
    {
        bool _have = false;
        int _indexNumber = 0;
        for (int i = 0; i < _Poollist.Count; i++)
        {
            if (_InputPrefabObject.name == _Poollist[i]._list[0].name)
            {
                _indexNumber = i;
                _have = true;
            }
        }

        //IfHaeObjectType
        if (_have)
        {
            //IfcanNotUseInPool
            if (_Poollist[_indexNumber]._list[0].activeInHierarchy)
            {
                GameObject _returnObject = Instantiate(_InputPrefabObject);
                _returnObject.name = _InputPrefabObject.name;
                _Poollist[_indexNumber]._list.Add(_returnObject);
                SetChildToPool(_returnObject);

                return _returnObject;
            }
            else
            {
                _Poollist[_indexNumber]._list.Add(_Poollist[_indexNumber]._list[0]);
                GameObject _returnObject = _Poollist[_indexNumber]._list[0];
                _Poollist[_indexNumber]._list.RemoveAt(0);
                SetChildToPool(_returnObject);
                _returnObject.SetActive(true);

                return _returnObject;

            }

        }
        else
        {
            _Poollist.Add(new poolType());
            GameObject _returnObject = Instantiate(_InputPrefabObject);
            _returnObject.name = _InputPrefabObject.name;
            _Poollist[_Poollist.Count - 1]._list.Add(_returnObject);
            SetChildToPool(_returnObject);

            return _returnObject;
        }

    }

    void SetChildToPool(GameObject _input)
    {
        bool _have = false;
        int _index = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            if(_input.name == transform.GetChild(i).name)
            {
                _have = true;
                _index = i;
            }
        }

        if(_have)
        {
            _input.transform.parent = transform.GetChild(_index);
        }
        else
        {
            GameObject _newparent = new GameObject();
            _newparent.name = _input.name;
            _newparent.transform.parent = this.transform;
            _input.transform.parent = _newparent.transform;
        }
    }
}
