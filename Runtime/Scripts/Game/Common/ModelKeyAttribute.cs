using System;

namespace JFramework.Unity
{

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ModelKeyAttribute : Attribute
    {
    }
}