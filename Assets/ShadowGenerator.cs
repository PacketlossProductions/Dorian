using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
[RequireComponent(typeof(CompositeCollider2D))]
[RequireComponent(typeof(Tilemap))]
public class ShadowGenerator : MonoBehaviour
{
    List<GameObject> shadowCasters = new List<GameObject>();
    CompositeCollider2D cc;
    Tilemap tilemap;

    private static BindingFlags accessFlagsPrivate = BindingFlags.NonPublic | BindingFlags.Instance;
    private static FieldInfo meshField = typeof(ShadowCaster2D).GetField("m_Mesh", accessFlagsPrivate);
    private static FieldInfo shapePathField = typeof(ShadowCaster2D).GetField("m_ShapePath", accessFlagsPrivate);
    private static MethodInfo onEnableMethod = typeof(ShadowCaster2D).GetMethod("OnEnable", accessFlagsPrivate);

    void Awake()
    {
        cc = GetComponent<CompositeCollider2D>();
        tilemap = GetComponent<Tilemap>();
        Tilemap.tilemapTileChanged += Tilemap_tilemapTileChanged;
    }

    private void Tilemap_tilemapTileChanged(Tilemap changedMap, Tilemap.SyncTile[] syncTiles)
    {
        Debug.LogError("tileMapchanged: " + changedMap.name);
        if(changedMap == tilemap)
        {
            Debug.LogError("It was me");
            Regenerate();
        } else
        {
            Debug.LogError("It wasn't me");
        }
    }

    private void Start()
    {
        Regenerate();
    }

    void Regenerate()
    {
        for(int i=shadowCasters.Count;i>0;i--)
        {
            GameObject current = shadowCasters[i];
            shadowCasters.Remove(current);
            Destroy(shadowCasters[i]);
        }

        for (int pathIndex = 0; pathIndex < cc.pathCount; pathIndex++)
        {
            Debug.LogWarning("Starting to build catcher " + pathIndex);
            // Create a shadowcaster for every "island" in the composite collider
            GameObject obj = new GameObject("ShadowCatcher " + pathIndex);
            obj.transform.parent = transform;
            ShadowCaster2D caster = obj.AddComponent<ShadowCaster2D>();
            caster.selfShadows = true;

            // Get the shape of the composite collider and copy it to the shadow catcher
            Vector3[] positions = new Vector3[cc.GetPathPointCount(pathIndex)];
            Vector2[] vertices = new Vector2[cc.GetPathPointCount(pathIndex)];
            cc.GetPath(pathIndex, vertices);

            for(int i=0;i<vertices.Length;i++)
            {
                positions[i] = new Vector3(vertices[i].x, vertices[i].y, 0.0f);
            }
            shapePathField.SetValue(caster, positions);
            //meshField.SetValue(caster, null);
            onEnableMethod.Invoke(caster, new object[0]);

            shadowCasters.Add(obj);
        }
    }
}
