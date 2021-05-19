using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMainMenuListItem : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _percent;
    [SerializeField] private TMP_Text _name;
    
    public void SetIcon(Sprite icon)
    {
        _iconImage.sprite = icon;
    }
    public void SetPercent(float percent)
    {
        _percent.text = percent.ToString() + "%";
    }
    public void SetName(string name)
    {
        _name.text = name;
    }

    public void SetLocalScale(Vector3 scale)
    {
        gameObject.transform.localScale = scale;
    }

    public void SetLocalPosition(Vector3 position)
    {
        gameObject.transform.localPosition = position;
    }
    public Vector3 GetLocalPosition()
    {
        return transform.localPosition;
    }
    public Vector3 GetLocalScale()
    {
        return transform.localScale;
    }


}
