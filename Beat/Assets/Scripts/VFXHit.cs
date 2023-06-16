using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXHit : MonoBehaviour
{
    public Vector3 Scale;
    public float Speed, Crescer;
    public bool random;

    // Start is called before the first frame update
    void Start()
    {
        Scale = transform.localScale;
        if (random)
            StartCoroutine(HitRandom());
    }

    // Update is called once per frame
    void Update()
    {

        if (Scale.z / 10 <= transform.localScale.z)
            transform.localScale *= 1 - Speed * Time.deltaTime;
        else
            transform.localScale = Scale / 10;
       
    }
    public IEnumerator HitRandom()
    {
        float rnd = Random.Range(0.5f, 2.5f);
        Hitar();
        yield return new WaitForSeconds(rnd);
        StartCoroutine(HitRandom());
    }
    public void Hitar()
    {
        transform.localScale *= 1 + Crescer;
        if (transform.localScale.z > Scale.z)
            transform.localScale = Scale;
    }
}
