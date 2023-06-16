using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarColor : MonoBehaviour
{
    [Range(0, 1)]
    public float value;
    private Image _image;
    public Slider _s;
    public Color Baixa, Media, Alta, Perfeita;
    private float cd, target = 1;
    private Color cor;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        value = _s.value;
        if (value <= 0.5f)
        {
            cor = Color.Lerp(Media, Baixa, (1 - (2 * value)));
        }
        else if (value == 1)
        {
            cor = Color.Lerp(Alta, Perfeita, cd);
            cd = Mathf.MoveTowards(cd, target, Time.deltaTime);
            if (cd >= 1)
                target = 0;
            if (cd <= 0)
                target = 1;
        }
        else
        {
            cor = Color.Lerp(Media, Alta, (2 * (value - 0.5f)));
            cd = 0;
            target = 1;
        }
        cor.a = 1;
        _image.color = cor;
    }
   
}
