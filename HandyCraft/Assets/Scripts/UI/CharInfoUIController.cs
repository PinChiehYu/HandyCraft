using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharInfoUIController : MonoBehaviour
{
    // Start is called before the first frame update
    private string _charName;
    private CharacterInfo info;

    [SerializeField]
    private Image Fill;
    private TMP_Text name;

    private int maxHp;
    private float targetPersentage;

    void Awake()
    {
        name = transform.Find("Name").GetComponent<TMP_Text>();
        name.text = transform.root.name;
        info = GetComponentInParent<CharacterInfo>();
        info.OnHpChange += OnHpChange;
        maxHp = info.GetMaxHp();
        targetPersentage = 1;
    }

    void Update()
    {
        Fill.fillAmount = Mathf.Lerp(Fill.fillAmount, targetPersentage, 0.1f);
    }

    private void OnHpChange(int after)
    {
        targetPersentage = (float)after / maxHp;
    }
}
