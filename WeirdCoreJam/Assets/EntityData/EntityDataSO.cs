using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EntityDataHolder")]
public class EntityDataSO : ScriptableObject
{
    public string EntityName = "Name here...";

    public Texture AnimationSprite;

    public int SpriteCols;
    public int SpriteRows;

    public Material EntityMaterial;

    public int MAXHP;


}
