using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class EnumIncrementDecrementAttribute : PropertyAttribute
{
    public Type EnumType { get; private set; }

    public EnumIncrementDecrementAttribute(Type enumType)
    {
        EnumType = enumType;
    }
}
