using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Key : MonoBehaviour
{
    public float speed;
    private float _speed;
    public Vector3 posi;
    public Notas _notas;
    private Image _imagem;
    private TextMeshProUGUI _texto;
    private CanvasGroup _group;
    private Vector3  InScale;
    private bool ligado = false, Sumir;
    // Start is called before the first frame update
    void Start()
    {
        _imagem = GetComponent<Image>();
        _texto = GetComponentInChildren<TextMeshProUGUI>();
        _group = GetComponent<CanvasGroup>();
        posi = transform.position;
        InScale = transform.localScale;

        _notas = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (ligado)
        {
            posi.y -= speed * Time.deltaTime;
            transform.position = posi;
            if (Sumir)
            {
                transform.localScale *= 1 + 00.1f * Time.deltaTime;
                _group.alpha -= Time.deltaTime;
            }
        }
    }

    public void SetKey(Notas n, Vector3 _posi)
    {
        ligado = (n != null);

        _notas = n;
        if (n != null)
        {
            _imagem.color = n.Cor;
            _texto.text = n.Key.ToString();
        }
        posi = _posi;
        Comecou();
    }

    public void Comecou()
    {
        _speed = speed;
        transform.localScale = InScale;
        _group.alpha = 1;
        Sumir = false;
    }
    public void Falhou()
    {
        transform.localScale *= 0.8f;
        _group.alpha = 0.25f;
    }
    public void Acertou()
    {
        Sumir = true;
    }
}
