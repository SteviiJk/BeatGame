using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public enum Menus { Menu, SampleScene, Win, Lose }

    public Menus TelaAtual;

    public GameObject[] HUDs;
    public Menus[] HUDsmn;

    private float cd;
    void Update()
    {
        cd -= Time.deltaTime;
       if (cd < 0)
            switch (TelaAtual)
            {
                case Menus.Menu:
                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                        IrGame();
                    }
                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        IrSaida();
                    }
                    break;
                case Menus.SampleScene:
                    break;
                case Menus.Win:
                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                        IrGame();
                    }
                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        IrMenu();
                    }
                    break;
                case Menus.Lose:
                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                        IrGame();
                    }
                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        IrMenu();
                    }
                    break;
            }
    }

    public void End(Menus menu)
    {
        if (TelaAtual != menu)
        {
            cd = 3;
            TelaAtual = menu;
            for (int i = 0; i < HUDs.Length; i++)
            {
                if (HUDs[i] != null)
                    HUDs[i].SetActive(TelaAtual == HUDsmn[i]);
            }
        }
    }

    public void IrSaida()
    {
        Application.Quit();
    }
    public void IrGame()
    {
        SceneManager.LoadScene(Menus.SampleScene.ToString());
    }
    public void IrMenu()
    {
        SceneManager.LoadScene(Menus.Menu.ToString());
    }

    public void LoadScene(Menus menu)
    {
        SceneManager.LoadScene(menu.ToString());
    }

}