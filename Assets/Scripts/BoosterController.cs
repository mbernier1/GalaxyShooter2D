using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoosterController : MonoBehaviour
{
    public Slider boosterSlider;
    public float _maxBooster = 100.0f;
    public float _booster = 0.0f;
    public bool _boosterIsEmpty = false;

    void Start()
    {
        _booster = _maxBooster;
    }

    void Update()
    {
        if (boosterSlider.value != _booster)
        {
            boosterSlider.value = _booster;
        }
    }

    public void UseBooster(float boost)
    {
        _booster -= boost;
    }

    public void FillBooster()
    {
        if(_booster < 0.0f) 
        {
            _booster = 0.0f;
        }

        if (_booster < _maxBooster)
        {
            _boosterIsEmpty = false;
            StartCoroutine(RefillBoosterBarRoutine());
        }
    }

    IEnumerator RefillBoosterBarRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        while (_boosterIsEmpty == false)
        {
            _booster += 0.5f;
            if (_booster == _maxBooster)
            {
                _boosterIsEmpty = true;
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void BoostIsFull()
    {
        _boosterIsEmpty = true;
    }
}
