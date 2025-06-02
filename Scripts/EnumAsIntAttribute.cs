using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class EnumAsIntAttribute : PropertyAttribute
{
    public Type EnumType { get; private set; }

    public EnumAsIntAttribute(Type enumType)
    {
        EnumType = enumType;
    }
}
