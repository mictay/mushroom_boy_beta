using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUi : MonoBehaviour 
{
    [SerializeField]
    int _Itemprice;

    [SerializeField]
    Text _PriceUi;
    [SerializeField]
    GameObject _AddButton;
    [SerializeField]
    GameObject _NeedMoreGold;
    [SerializeField]
    GameObject _RealPrefab;
    [SerializeField]
    int _MaxAddCount;
    [SerializeField]
    int _CurrentCount;
    private void Start()
    {
        _PriceUi.text = _Itemprice.ToString();
    }

    private void OnEnable()
    {
        if(_CurrentCount < _MaxAddCount)
        {
            if (GameManager._Instance && GameManager._Instance._Gold >= _Itemprice)
            {
                _NeedMoreGold.SetActive(false);
                _AddButton.SetActive(true);
            }
            else
            {
                _NeedMoreGold.SetActive(true);
                _AddButton.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }

    }


    public void Add()
    {
        GameManager._Instance._Player.transform.position = new Vector3(-40, 1.5f, -40);
        GameObject _new = Instantiate(_RealPrefab);
        _new.transform.position = new Vector3(-25, 0, -40);
        GameManager._Instance._Gold -= _Itemprice;
        GameManager._Instance._UiManager.ClossShop();
        _CurrentCount += 1;
    }
}
