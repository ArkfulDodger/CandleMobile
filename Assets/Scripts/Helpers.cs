using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static Transform FindChildWithTag(this Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child;
            }
        }
        return null;
    }
    
    public static List<Transform> FindChildrenWithTag(this Transform parent, string tag)
    {
        List<Transform> transforms = new();

        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                transforms.Add(child);
            }
        }

        if (transforms.Count > 0)
        {
            return transforms;
        }
        else
        {
            return null;
        }
    }

    public static T GetComponentInChildWithTag<T>(this Transform parent, string tag) where T:Component
    {
        foreach(Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                T component = child.GetComponent<T>();
                if (component)
                {
                    return component;
                }
            }
        }
        return null;
    }

    public static List<T> GetComponentsInChildrenWithTag<T>(this Transform parent, string tag) where T:Component
    {
        List<T> components = new();

        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                T component = child.GetComponent<T>();
                if (component)
                {
                    components.Add(component);
                }
            }
        }

        if (components.Count > 0)
        {
            return components;
        }

        return null;
    }
}
