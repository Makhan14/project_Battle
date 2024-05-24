using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerZone : MonoBehaviour
{

    public enum Team
    {
        Red,
        Blue,
        Neutral
    }



    [SerializeField] private Team team;
    [SerializeField] private Material redTeamMaterial;
    [SerializeField] private Material blueTeamMaterial;
    [SerializeField] private Material NeutralTeamMaterial;
    [SerializeField] private Renderer targetRenderer; 
    

    private List<PickUpObject> pickUpObjectList = new List<PickUpObject>();
    private int score;
    

    private void Awake()
    {
           
        
        if (targetRenderer != null)
        {
            SetTeamMaterial();  
        }
        else
        {
            Debug.LogWarning("Renderer component not assigned!");
        }
    }

    private void SetTeamMaterial()
    {
        if (targetRenderer != null)
        {
            switch (team)
            {
                case Team.Red:
                    targetRenderer.material = redTeamMaterial;
                    break; 
                case Team.Blue:
                    targetRenderer.material = blueTeamMaterial;
                    break;
                case Team.Neutral:
                    targetRenderer.material = NeutralTeamMaterial;
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PickUpObject pickUpObject = other.GetComponent<PickUpObject>();
        if (pickUpObject != null && !pickUpObjectList.Contains(pickUpObject) && !pickUpObject.GetIsBeingHeld())
        {
            
            pickUpObjectList.Add(pickUpObject);
            Debug.Log("Added to " + team + " list.");

            
            if (team == Team.Red || team == Team.Blue)
            {
                score += pickUpObject.GetPickUpObjectScore();
                Debug.Log("Team " + team + " Score: " + score);
            }
            
            
        }

    }

    private void OnTriggerExit(Collider other)
    {
       

            
            
        
    }



}
