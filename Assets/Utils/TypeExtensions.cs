﻿using System;

public static class TypeExtensions
{
    public static Type GetBaseTypeOverObject(this Type type)
    {
        while (type.BaseType != typeof(object) && type.BaseType != typeof(ValueType))
        {
            type = type.BaseType;
        }
        return type;
    }
}
