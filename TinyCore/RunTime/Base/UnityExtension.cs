﻿using UnityEngine;

namespace Bronya
{
    public static class UnityEngine
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if(component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }
    }
}