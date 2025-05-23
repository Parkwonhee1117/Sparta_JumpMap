using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Condition : MonoBehaviour
{
    [SerializeField] private float curValue;
    [SerializeField] private float startValue;
    [SerializeField] private float maxValue;
    [SerializeField] private float passiveValue;
    [SerializeField] private Image uibar;

    public float PassiveValue {get {return passiveValue;}}
    public float CurValue {get {return curValue;}}

    void Start()
    {
        curValue = startValue;
    }

    void Update()
    {
        uibar.fillAmount = GetPercentage();
    }

    public float GetPercentage()
    {
        return curValue / maxValue;
    }

    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void Subtract(float value)
    {
        curValue = Mathf.Max(curValue - value, 0);
    }
}
