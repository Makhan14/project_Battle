using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class GameInput : MonoBehaviour
{

   public static GameInput instance {  get; private set; }


    public event EventHandler OnInteractAction;
    
    private PlayerInputActions playerInputActions;




    private void Awake()
    {
        instance = this;    

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        // start listening to Player Input Actions
        playerInputActions.Player.Interact.performed += Interact_performed;
        
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        // Getting inputs from keyboard and translate to Vector2
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;
        
        return inputVector; 
    }


    
}

