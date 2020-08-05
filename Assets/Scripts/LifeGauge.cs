using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class LifeGauge : MonoBehaviour
{
    public int MaxHearts = 8;

    private List<Image> _hearts;

    // Start is called before the first frame update
    void Start()
    {
        _hearts = new List<Image>(MaxHearts);
        Transform heartstart = transform.Find("heartstart");
        for (int i = 0; i < MaxHearts; ++i)
        {
            Transform tr = transform.Find("heart" + i);
            Image image = null;

            if (tr)
            {
                image = tr.GetComponent<Image>();
            }

            if (image != null)
            {
                _hearts.Add(image);
            }
        }
    }

    void SetHealth(float health)
    {
        for (int i = 0; i < _hearts.Count; ++i)
        {
            Image heart = _hearts[i];
            heart.fillAmount = Mathf.Max(0, Mathf.Min(health, 1));
            health = health - 1;
        }
    }
}
