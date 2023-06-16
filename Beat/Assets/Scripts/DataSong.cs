using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSong : MonoBehaviour
{

    public List<Vector2> notes;
    private int id;
    public float delay = 5, atraso = 4;
    public AudioSource sorce;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlaySong());
    }
    
    private IEnumerator PlaySong()
    {
        yield return new WaitForSeconds(delay);
        sorce.Play();
    }
    public void StopSong()
    {
        sorce.Stop();
    }
}
