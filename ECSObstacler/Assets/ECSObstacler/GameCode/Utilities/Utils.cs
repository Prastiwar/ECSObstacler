using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class Utils
{
    public static float2 GetRandomPosition(float4 screenBorder)
    {
        float2 randPos = 0;

        randPos.x = Random.Range(0, 2) == 0 ? screenBorder.x : screenBorder.y;
        randPos.y = Random.Range(screenBorder.z, screenBorder.w);
        return randPos;
    }

    public static float2 GetRandomHeading(float2 pos)
    {
        float2 randHeading = 1;
        randHeading.y = Random.value;
        if (pos.x < 0)
        {
            randHeading.x = 1;
        }
        else if (pos.x > 0)
        {
            randHeading.x = -1;
        }
        return randHeading;
    }

    public static float2 HeadToPlayer(float2 playerPos, float2 pos)
    {
        return math.normalize(playerPos - pos);
    }

    public static float4 GetScreenBorder()
    {
        var camera = Camera.main;
        var planes = GeometryUtility.CalculateFrustumPlanes(camera);
        var screenBorder = new float4(-planes[0].normal.x * planes[0].distance,   // x left
                                   -planes[1].normal.x * planes[1].distance,  // y right
                                   -planes[2].normal.z * planes[2].distance,  // z down
                                   -planes[3].normal.z * planes[3].distance); // w up
        return screenBorder;
    }

    public static bool IsOnBorder(float4 screenBorder, float2 pos, TransformMatrix matrix)
    {
        var scaleX = matrix.Value.m0.x;
        var scaleY = matrix.Value.m1.y;
        return pos.x <= screenBorder.x + scaleX || pos.x >= screenBorder.y - scaleX ||
               pos.y <= screenBorder.z + scaleY || pos.y >= screenBorder.w - scaleY;
    }

    public static bool IsOnBorder(float4 screenBorder, float2 pos)
    {
        return pos.x <= screenBorder.x || pos.x >= screenBorder.y ||
               pos.y <= screenBorder.z || pos.y >= screenBorder.w;
    }

    public static MeshInstanceRenderer GetLookFromPrototype(string prototypeName)
    {
        var prototype = GameObject.Find(prototypeName);
        if (prototype != null)
        {
            var result = prototype.GetComponent<MeshInstanceRendererComponent>().Value;
            Object.Destroy(prototype);
            return result;
        }
        Debug.LogWarning($"{prototypeName} was not found, creating new renderer");
        return NewMeshInstanceRenderer();
    }

    private static MeshInstanceRenderer NewMeshInstanceRenderer()
    {
        Material newMaterial = new Material(Shader.Find("Standard")) { enableInstancing = true };
        MeshInstanceRenderer renderer = new MeshInstanceRenderer {
            castShadows = UnityEngine.Rendering.ShadowCastingMode.Off,
            receiveShadows = false,
            mesh = new Mesh(),
            material = newMaterial
        };
        return renderer;
    }
}
