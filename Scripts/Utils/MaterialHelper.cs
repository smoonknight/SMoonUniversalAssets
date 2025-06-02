using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MaterialHelper
{
    public static Renderer[] GetRenderers(Transform parent)
    {
        return parent.GetComponentsInChildren<Renderer>();
    }

    public static List<Material> GetMaterials(Renderer[] meshRenderers)
    {
        List<Material> materials = new();
        foreach (var meshRenderer in meshRenderers)
        {
            materials.AddRange(GetMaterials(meshRenderer));
        }
        return materials;
    }
    public static List<Material> GetMaterials(Renderer meshRenderer) => meshRenderer.materials.ToList();

    public static void DisableHighlightOnMaterial(List<Material> materials)
    {
        foreach (var item in materials)
        {
            item.DisableKeyword("_EMISSION");
            item.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
        }
    }

    public static void EnableHightlightOnMaterial(Material material, Color color)
    {
        material.EnableKeyword("_EMISSION");
        material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
        material.SetColor("_EmissionColor", color);
    }

    public static void SetMaterialToTransparent(List<Material> materials)
    {
        foreach (var material in materials)
        {
            material.SetFloat("_Surface", 1.0f);
            material.SetFloat("_Blend", 1.0f);
            material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetFloat("_ZWrite", 0);
            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }
    }

    public static void EnableTransparent(List<Material> materials, int opacity)
    {
        foreach (var material in materials)
        {
            SetTransparent(material, opacity / 255f);
        }
    }

    public static void DisableTransparent(List<Material> materials)
    {
        foreach (var material in materials)
        {
            SetTransparent(material, 1);
        }
    }

    public static void SetTransparent(Material material, float alpha)
    {
        Color colorHolder = material.color;
        colorHolder.a = alpha;
        material.color = colorHolder;
    }

    public static void EnableHightlightOnMaterials(List<Material> materials, Color color)
    {
        foreach (var item in materials)
        {
            EnableHightlightOnMaterial(item, color);
        }
    }
}