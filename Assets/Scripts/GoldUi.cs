using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldUi : MonoBehaviour 
{
    [SerializeField]
    Text _text;
    int _delataGold;
    float _tang;
    private void Update()
    {
        if(GameManager._Instance)
        {
            if(GameManager._Instance._Gold != _delataGold)
            {
                _tang += Time.deltaTime;
                _delataGold = (int)Mathf.Lerp(_delataGold, GameManager._Instance._Gold, _tang);
                _text.text = _delataGold.ToString();
            }
            else
            {
                _tang = 0;
            }
        }
    }

    private void Start()
    {
        _text.text = _delataGold.ToString();
    }
}
