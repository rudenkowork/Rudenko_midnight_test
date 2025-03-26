using System;
using System.Collections.Generic;

namespace Plugins.RContainer
{
    public static class TypeExtensions
    {
        static readonly Dictionary<Type, Type[]> _interfaces = new Dictionary<Type, Type[]>();

        public static Type[] Interfaces(this Type type)
        {
            Type[] result;
            if (!_interfaces.TryGetValue(type, out result))
            {
#if UNITY_WSA && ENABLE_DOTNET && !UNITY_EDITOR
                result = type.GetTypeInfo().ImplementedInterfaces.ToArray();
#else
                result = type.GetInterfaces();
#endif
                _interfaces.Add(type, result);
            }

            return result;
        }
    }
}