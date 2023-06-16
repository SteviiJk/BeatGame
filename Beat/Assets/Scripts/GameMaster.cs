using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

[System.Serializable]
public class Notas
{
    public GameMaster.typeNota Tipo;
    public Color Cor;
    public KeyCode Key;
}

[System.Serializable]
public class Pontuaçao {
    public string name;
    public float distance;
    public Color cor;
    public float Points;
    public int count;
}

public class GameMaster : MonoBehaviour
{
    public static GameMaster GM;
    public bool isGaming;
    public MenuController mnCtrl;

    private bool acabaou, perdeu;

    public float maxHealth;
    [SerializeReference]
    private float health;
    public Slider healthBar;


    public float Score;
    public float sqnCombo,MxCombo;

    public GameObject PrefabScore, PrefabCombo;
    public Transform pointScore, pointCombo;

    public enum typeNota { Slow, Normal, Fast }
    public Notas[] baseNotas = new Notas[3];

    public RectTransform LeitorTransform;
    public Vector3 positionPoint, StartPoint;
    public List<Key> keys, nullKeys;
    public Key nextKey;
    public int idK;


    public float Distance;
    public string Ponto;
    public List<Pontuaçao> pontosType;
    public TextMeshProUGUI txtPontuacao;
    public TextMeshProUGUI[] txtPointsFinal;

    public DataSong data;
    private List<Vector2> notes;

    public AudioSource asSFX;
    public VFXHit _vfxhit;

    private void Awake()
    {
        if (GM == null)
            GM = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        else Debug.LogError("Not Found GameMaster");
       
        positionPoint = LeitorTransform.position;
        StartPoint = keys[0].transform.position;

        notes = data.notes;

        StartCoroutine(InstantciaNota());
     
        health = maxHealth - 0.01f;
        AttHealth(0);

        asSFX = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (idK >= keys.Count)
            nextKey = null;
        else if (keys.Count == 0)
            nextKey = null;
        else nextKey = keys[idK];

        if (idK < keys.Count)
            if (keys[idK]._notas == null)
                Tirar(keys[idK]);

        if (!perdeu)
        {
            if (nextKey != null)
            {
                #region Mecanica
                Distance = Vector3.Distance(positionPoint, nextKey.gameObject.transform.position);
                for (int i = 0; i < pontosType.Count; i++)
                {
                    if (Distance > pontosType[i].distance)
                        break;
                    Ponto = pontosType[i].name;
                }
                foreach (Notas item in baseNotas)
                {
                    if (Input.GetKeyDown(item.Key))
                    {
                        if (Ponto == "Miss")//longe
                        {
                            AttHealth(-1);
                            pontosType[0].count++;
                        }
                        else if (item == nextKey._notas)
                        {
                            nextKey.Acertou();
                            AttHealth(0.3f);
                            idK++;
                            for (int i = 0; i < pontosType.Count; i++)
                            {
                                if (Ponto == pontosType[i].name)
                                {
                                    Pontuar(i);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            idK++;
                            AttHealth(-1);
                            pontosType[0].count++;
                            nextKey.Falhou();
                        }
                    }
                }

                if (Ponto == "Miss")
                    if (positionPoint.y > nextKey.gameObject.transform.position.y)
                    {
                        AttHealth(-1);
                        pontosType[0].count++;
                        nextKey.Falhou();
                        idK++;
                    }

                for (int i = 0; i < idK; i++)
                {
                    if (Vector3.Distance(positionPoint, keys[i].gameObject.transform.position) > 250)
                    {
                        Tirar(keys[i]);
                        idK--;
                    }
                }
                #endregion
            }
            else if (acabaou)
                Ganhar();
        }
    }

    public void AttHealth(float _h)
    {
        health += _h;
        if (health > maxHealth)
            health = maxHealth;
        if (health <= 0)
            Perder();

        if (_h < 0)
        {
            Debug.Log("perdeu vida sound");
            asSFX.Play();
            sqnCombo = 0;
        }

        healthBar.value = (health / maxHealth);
    }

    public void Pontuar(int i)
    {
        pontosType[i].count++;
        sqnCombo++;
        if (MxCombo < sqnCombo)
            MxCombo = sqnCombo;


        Score += pontosType[i].Points * (1 + ((int)(sqnCombo / 5)));


        txtPontuacao.text = " Score:\n  0" + Score;
        
        GameObject vfxScore = Instantiate(PrefabScore, pointScore.position, Quaternion.identity);
        vfxScore.transform.parent = pointScore;
        vfxScore.GetComponent<EffectText>()._string = "+" + pontosType[i].Points * (1 + ((int)(sqnCombo / 5)));
        vfxScore.GetComponent<EffectText>().cor =  pontosType[i].cor;
    

        if (sqnCombo >= 3)
        {
            Debug.Log("VFXcombo");
            GameObject vfxCombo = Instantiate(PrefabCombo, pointCombo.position, Quaternion.identity);
            vfxCombo.transform.parent = pointCombo;
            vfxCombo.GetComponent<EffectText>()._string = "x" + sqnCombo+"\nCOMBO";
            vfxCombo.GetComponent<EffectText>().cor = Color.blue;
        }

        _vfxhit.Hitar();
}


    public void Tirar(Key _k)
    {
        nullKeys.Add(_k);
        keys.Remove(_k);
        _k.SetKey(null, StartPoint);
    }

    public void Perder()
    {
        mnCtrl.End(MenuController.Menus.Lose);
        perdeu = true;
        data.StopSong();
    }
    public void Ganhar()
    {
        mnCtrl.End(MenuController.Menus.Win);
        #region UI
        txtPointsFinal[0].text = "You Won!!!";
        for (int i = 0; i < pontosType.Count; i++)
        {
            txtPointsFinal[i + 1].color = pontosType[i].cor;
            txtPointsFinal[i + 1].text = pontosType[i].name + ":  " + pontosType[i].count;
        }
        txtPointsFinal[5].text = "Score: " + Score;
        txtPointsFinal[6].text = "Max Combo: " + MxCombo;
        #endregion
    }

    private int id;
    public float atraso = 4;
    public IEnumerator InstantciaNota()
    {
        float wait = 0;
        if (id == 0)
            wait = notes[id].x - atraso;
        else
            wait = notes[id].x - notes[id - 1].x;
        yield return new WaitForSeconds(wait);
        if (!perdeu)
        {
            if (nullKeys.Count > 0)
            {
                int rdn = (int)notes[id].y;
                nullKeys[0].SetKey(baseNotas[rdn], StartPoint);
                keys.Add(nullKeys[0]);
                nullKeys.RemoveAt(0);
            }
            id++;
            if (id < notes.Count)
                StartCoroutine(InstantciaNota());
            else
                acabaou = true;
        }
    }
    
    //private void OnDrawGizmos()
    //{
    //    Color distColr = Color.red;
    //    foreach (Pontuaçao item in pontosType)
    //    {
    //        Vector3 posiA = positionPoint;
    //        posiA.y += item.distance;
    //        Vector3 posiB = posiA;
    //        posiA.x -= 100;
    //        posiB.x += 100;
    //        Gizmos.color = item.cor;
    //        Gizmos.DrawLine(posiA, posiB);

    //        if (Distance < item.distance)
    //            distColr = item.cor;
    //    }

    //    if (positionPoint != Vector3.zero&& nextKey!=null)
    //    {
    //        Gizmos.color = distColr;
    //        Gizmos.DrawLine(positionPoint, nextKey.gameObject.transform.position);
    //        Gizmos.DrawSphere(positionPoint, 5);
    //        Gizmos.DrawSphere(nextKey.gameObject.transform.position, 5);
    //    }
    //}

//#if UNITY_EDITOR
//    [CustomEditor(typeof(GameMaster))]
//    public class MeteorEditor : Editor
//    {
//        public override void OnInspectorGUI()
//        {
//            base.OnInspectorGUI();

//            GameMaster t = (GameMaster)target;

//            if (GUILayout.Button(""))
//            {
//                t.Criar();
//            }
//        }
//    }

//#endif
}
