using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class PickUpObjectSO : ScriptableObject
{
    [Header("Details")]

    public Transform prefab;
    public Sprite sprite;
    public string objectname;

    [Header("Values")]

    public int pointValue;
    public float weight;


}