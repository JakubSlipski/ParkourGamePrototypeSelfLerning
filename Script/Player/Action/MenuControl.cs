using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    [Header("Variables")]
    public bool invOpen = false;
    public bool menuOpen = false;
    public bool settingsOpen = false;

    [Header("References")]
    public PlayerAction pa;
    public PlayerMovement pm;
    public TimeControll tc;

    [Header("GameObjects")]
    public GameObject HUD;
    public GameObject escmenu;
    public GameObject settings;

    public void Start()
    {
        escmenu.SetActive(false);
        settings.SetActive(false);
        HUD.SetActive(true);
    }

    public void Update()
    {
        EscMenu();
        Inventory();
    }

    //FIXME: ADD ESC MENU GUI
    public void EscMenu()
    {
        if (Input.GetKeyDown(pa.menuKey))
        {
            if (menuOpen)
            {
                tc.UnPauseTime();
                //ui
                escmenu.SetActive(false);
                menuOpen = false;
                //cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                //hud
                HUD.SetActive(true);
            }
            else if (!menuOpen && !invOpen)
            {
                tc.PauseTime();
                //ui
                escmenu.SetActive(true);
                menuOpen = true;
                //cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                //hud
                HUD.SetActive(false);
            }
        }
    }
    //RESUME BUTTON
    public void CloseEscMenu()
    {
        if (menuOpen)
        {
            tc.UnPauseTime();
            //ui
            escmenu.SetActive(false);
            menuOpen = false;
            //cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //hud
            HUD.SetActive(true);
        }
    }
    
    //SETTINGS BUTTON
    public void SettingsShow()
    {
        if (!settingsOpen)
        {
            settings.SetActive(true);
            settingsOpen = true;
        }
        else if (settingsOpen)
        {
            settings.SetActive(false);
            settingsOpen = false;
        }
    }

    //MAIN MENU BUTTON
    public void JumpToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //QUIT BUTTON
    public void QuitButton()
    {
        Application.Quit();
    }

    //FIXME: ADD INVENTORY GUI

    public void Inventory()
    {
        if (Input.GetKeyDown(pa.invKey) && pm.grounded)
        {
            if (invOpen)
            {
                tc.UnPauseTime();
                invOpen = false;
            }
            else if (!invOpen && !menuOpen)
            {
                tc.PauseTime();
                invOpen = true;
            }
        }
    }
}
