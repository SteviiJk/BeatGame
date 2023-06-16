using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class EffectText : MonoBehaviour
{
    public TextMeshProUGUI texto;
    public string _string = "=000";
    public Vector3 Speed;
    private Vector3 posi;
    public Color cor;

    // Start is called before the first frame update
    void Start()
    {
        texto = GetComponent<TextMeshProUGUI>();
        texto.text = _string;
        posi = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        cor.a -= Time.deltaTime;
        texto.color = cor;
        texto.fontSize += Time.deltaTime * 40f;
        posi += Time.deltaTime * Speed;
        transform.position = posi;
        if (cor.a < 0)
            Destroy(gameObject);
    }
}
