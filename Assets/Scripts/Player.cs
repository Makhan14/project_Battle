using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{

    private const string IS_WALKING = "IsWalking?"; 


    

    public static Player Instance { get; private set; }     


    
    
    [Header("Refrences")]

    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask pickUpObjectLayerMask;
    [SerializeField] private LayerMask wallLayerMask;
    [SerializeField] private Transform objectGrabPointTransform;
    

    [Header("Movement")]

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;

    [Header("Force")]

    [SerializeField] private float throwFroce = 10f; 


    private float playerRadius = .7f; 
    private float playerHeight = 2f; 
    private Vector3 moveDir; 
    private bool varIsWalking;
    private Vector3 lastInteractDir;
   
    private PickUpObject pickUpObject;
    private bool isHoldingObject; 
    



    private void Awake()
    {  
        Instance = this;    

    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        Vector2 inputVector = GameInput.instance.GetMovementVectorNormalized();

        moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (pickUpObject)
        {
            // carrying an object
            Vector3 throwDistance = objectGrabPointTransform.forward; 
            
            pickUpObject.Throw(throwDistance, throwFroce);

            pickUpObject.SetIsBeingHeld(false);
            pickUpObject = null;
            
            
            
        }
        else
        {
            // not carrying an object 
            float interactDistance = 2f;
            if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit rayCastHit, interactDistance, pickUpObjectLayerMask))
            {
                if (rayCastHit.transform.TryGetComponent(out pickUpObject))
                {
                    pickUpObject.PickUp(objectGrabPointTransform);

                    pickUpObject.SetIsBeingHeld(true);
                }
            }                        
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();

         
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = GameInput.instance.GetMovementVectorNormalized();

        moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir !=  Vector3.zero)
        {
            lastInteractDir = moveDir;  
        }
        
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit rayCastHit, interactDistance, pickUpObjectLayerMask))
        {
            if (rayCastHit.transform.TryGetComponent(out PickUpObject pickUpObject))
            {
                  
            }
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.instance.GetMovementVectorNormalized();

        moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        playerRadius = 0.7f;
        playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance, wallLayerMask);

        if (!canMove)
        {
            // cannot move towards modeDir

            // attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = ((moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance, wallLayerMask));

            if (canMove)
            {
                // can only move on the x
                moveDir = moveDirX;
            }
            else
            {
                // cannot move only on the X
                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance, wallLayerMask);

                if (canMove)
                {
                    // we can move only on the Z
                    moveDir = moveDirZ;
                }
                else
                {
                    // cannot move in any direction 
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        varIsWalking = moveDir != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotationSpeed * Time.deltaTime);
    }




    // return bool for IsWalking animation bool 
    public bool IsWalking()
    {
        return varIsWalking;   
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 point1 = transform.position + Vector3.up * playerRadius;
        Vector3 point2 = transform.position + Vector3.up * (playerHeight - playerRadius); 
        float moveDistance = moveSpeed * Time.deltaTime;

        // draw the initial capsule 
        DrawWireCapsule(point1, point2, playerRadius);

        // raycast for interation
        float rayLength = 2f;
        Debug.DrawRay(transform.position, transform.forward * rayLength, Color.yellow);

    }

    private void DrawWireCapsule(Vector3 point1, Vector3 point2, float radius)
    {
        Gizmos.DrawWireSphere(point1, radius);
        Gizmos.DrawWireSphere(point2, radius);
        Gizmos.DrawLine(point1 + Vector3.right * radius, point2 + Vector3.right * radius);
        Gizmos.DrawLine(point1 - Vector3.right * radius, point2 - Vector3.right * radius);
        Gizmos.DrawLine(point1 + Vector3.forward * radius, point2 + Vector3.forward * radius);
        Gizmos.DrawLine(point1 - Vector3.forward * radius, point2 - Vector3.forward * radius);
    }

    public bool IsPlayerHoldingPickUpObject()
    {
        return isHoldingObject;
    }

}
