using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Field Category")]
public class FieldCategoryData : ScriptableObject
{
    public string id;
    public Sprite icon;

    [SerializeField] protected string categoryName = "";

    public string GetCategoryName()
    {
        return categoryName;
    }
}