using UnityEngine;

public class PickUpObject : MonoBehaviour
{

    private enum ScoreState
    {
        Neutral, 
        Blue, 
        Red
    }



    [Header("References")]
    
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] private PickUpObjectSO pickUpObjectSO;
    [SerializeField] private Renderer targetRenderer;

    private Rigidbody rb;
    private Transform objectGrabPointTransform;
    private bool isBeingHeld;
    private ScoreState scoreState;
    private Renderer objectRenderer;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        scoreState = ScoreState.Neutral;    
    }

    private void Update()
    {
        if (objectGrabPointTransform != null)
        {
            transform.position = objectGrabPointTransform.position;
            transform.rotation = objectGrabPointTransform.rotation;
        }

    }

    public void SetScoreState(PlayerZone.Team team)
    {
        switch (team)
        {
            case PlayerZone.Team.Blue:
                scoreState = ScoreState.Blue;
                TintMaterial(Color.blue);
                break;
            case PlayerZone.Team.Red:
                scoreState = ScoreState.Red;
                TintMaterial(Color.red);
                break;
            case PlayerZone.Team.Neutral:
                scoreState = ScoreState.Neutral;
                TintMaterial(Color.white);
                break;
        }
    }

    public void Interact()
    {
        Debug.Log("Interact!"); 
    }

    public void PickUp(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        rb.useGravity = false;
        rb.isKinematic = true;
        //Debug.Log("PickUpObject PickedUp.");
    }

    public void Throw(Vector3 throwDirection, float throwForce)
    {
        this.objectGrabPointTransform = null;
        rb.useGravity = true;
        rb.isKinematic = false;

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        //Debug.Log("PickUpObject Thrown.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((groundLayerMask.value & (1 << collision.gameObject.layer)) > 0)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            //Debug.Log("Hit the ground, freezing constraints.");
        }
    }

    private void TintMaterial(Color color)
    {
        if (objectRenderer != null)
        {
            targetRenderer.material.color = color; 
        }
        else
        {
            Debug.LogWarning("Renderer component not found on PickUpObject!");
        }
    }

    public PickUpObjectSO PickUpObjectSO()
    {
        return pickUpObjectSO;  
    }

    public int GetPickUpObjectScore()
    {
        return pickUpObjectSO.pointValue;
    }

    public bool GetIsBeingHeld()
    {
        return isBeingHeld; 
    }

    public void SetIsBeingHeld(bool boolValue)
    {
        isBeingHeld = boolValue;    
    }
}

