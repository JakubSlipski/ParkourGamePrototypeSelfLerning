using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBinds : MonoBehaviour
{
    [Header("Movemnt")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.C;
    public KeyCode slideKey = KeyCode.LeftControl;
    //Wallrunning
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;
    //Dash/Dodge
    public KeyCode dashKey = KeyCode.E;
    //Grapple
    public KeyCode grappleKey = KeyCode.Mouse2;
    //Swinging
    public KeyCode swingKey = KeyCode.Mouse1;

    [Header("Action")]
    public KeyCode useKey = KeyCode.F;
    //Items
    public KeyCode medicKey = KeyCode.H;
    public KeyCode flashLightKey = KeyCode.T;
    //Inventory
    public KeyCode invKey = KeyCode.I;
    public KeyCode swapMedic = KeyCode.Tab;
    //Menu
    public KeyCode menuKey = KeyCode.Escape;

    [Header("DEVMODE")]
    public KeyCode resetGrounded = KeyCode.F1;
    public KeyCode infStamina = KeyCode.F2;
    public KeyCode infHealth = KeyCode.F3;
    public KeyCode defKeys = KeyCode.F4;


    [Header("References")]
    public PlayerMovement pm;
    public Sliding sl;
    public WallRunning wr;
    public LedgeGrabbing lg;
    public Dashing ds;
    public Grappling grap;
    public Swinging sw;
    public HealthControl hc;
    public PlayerAction pa;
    public PloeSwing ps;

    public DevMODE Dev;

    public void Start()
    {
        SetKeyCode();
    }

    public void Update()
    {
        if (Input.GetKeyDown(defKeys))
        {
            BackToDefoult();
        }
        else
            SetKeyCode();
    }

    private void SetKeyCode()
    {
        //////////////////////////////////////////
        ///
        ///         PLAYER MOVEMENT
        /// 
        //////////////////////////////////////////
        pm.jumpKey = jumpKey;
        pm.sprintKey = sprintKey;
        pm.crouchKey = crouchKey;
        sl.slideKey = slideKey;
        //Wallrunning
        wr.jumpKey = jumpKey;
        wr.upwardsRunKey = upwardsRunKey;
        wr.downwardsRunKey = downwardsRunKey;
        //Ledge
        lg.jumpKay = jumpKey;
        //Dashing
        ds.dashKey = dashKey;
        //Grappling
        grap.grappleKey = grappleKey;
        //Swinging
        sw.swingKey = swingKey;
        ps.swingJumpKey = jumpKey;

        //////////////////////////////////////////
        ///
        /// PlayerAction/InventoryActiob/MenuAction
        /// 
        //////////////////////////////////////////

        //Action
        pa.useKey = useKey;
        //Medic
        hc.healingKey = medicKey;
        //Items
        pa.flashLightKey = flashLightKey;
        //Inventory
        pa.invKey = invKey;
        pa.swapMedic = swapMedic;
        //Menu
        pa.menuKey = menuKey;

        //////////////////////////////////////////
        ///
        ///             DevMODE
        /// 
        //////////////////////////////////////////

        Dev.resetGrounded = resetGrounded;
        Dev.stamina = infStamina;
        Dev.health = infHealth;
    }

    private void BackToDefoult()
    {
        jumpKey = KeyCode.Space;
        sprintKey = KeyCode.LeftShift;
        crouchKey = KeyCode.C;
        slideKey = KeyCode.LeftControl;
        //Wallrunning
        upwardsRunKey = KeyCode.LeftShift;
        downwardsRunKey = KeyCode.LeftControl;
        //Dash/Dodge
        dashKey = KeyCode.E;
        //Grapple
        grappleKey = KeyCode.Mouse2;
        //Swinging
        swingKey = KeyCode.Mouse1;
    
        useKey = KeyCode.F;
        //Items
        medicKey = KeyCode.H;
        flashLightKey = KeyCode.T;
        //Inventory
        invKey = KeyCode.I;
        swapMedic = KeyCode.Tab;
        //Menu
        menuKey = KeyCode.Escape;

        resetGrounded = KeyCode.F1;
        infStamina = KeyCode.F2;
        infHealth = KeyCode.F3;
    }
}
