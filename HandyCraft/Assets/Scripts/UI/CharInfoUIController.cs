using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharInfoUIController : MonoBehaviour
{
    // Start is called before the first frame update
    private string _charName;
    private CharInfo _info;

    [SerializeField]
    private Image Fill;
    private TMP_Text name;

    private int _maxHp;
    private float _targetPersentage;

    void Awake()
    {
        name = transform.Find("Name").GetComponent<TMP_Text>();
        name.text = transform.parent.name;
        _info = GetComponentInParent<CharInfo>();
        _info.OnHpChange += OnHpChange;
        _maxHp = _info.GetMaxHp();
        _targetPersentage = 1;
    }

    void Update()
    {
        Fill.fillAmount = Mathf.Lerp(Fill.fillAmount, _targetPersentage, 0.1f);
    }

    private void OnHpChange(int after)
    {
        _targetPersentage = (float)after / _maxHp;
    }
}
