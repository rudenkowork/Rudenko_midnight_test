using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Plugins.RContainer
{
    public class Container
    {
        private readonly Dictionary<Type, Type> _bindings = new();
        private readonly Dictionary<Type, object> _singletons = new();

        private Container()
        {
        }

        public static Container GlobalInstance => _globalInstance ??= new Container();
        private static Container _globalInstance;

        public void Bind<TInterface, TImplementation>() where TImplementation : TInterface
        {
            _bindings[typeof(TInterface)] = typeof(TImplementation);
        }

        public void BindInstance<TInterface>(TInterface instance)
        {
            _singletons[typeof(TInterface)] = instance;
        }

        public void Inject(object instance)
        {
            InjectMethods(instance);
        }

        private object Get(Type type)
        {
            if (_singletons.TryGetValue(type, out var instance))
            {
                return instance;
            }

            if (_bindings.TryGetValue(type, out var implType))
            {
                instance = CreateInstance(implType);
                _singletons[type] = instance;
                return instance;
            }

            if (!type.IsAbstract && !type.IsInterface)
            {
                instance = CreateInstance(type);
                return instance;
            }

            throw new InvalidOperationException($"Type '{type.FullName}' is not bound.");
        }

        private object CreateInstance(Type type)
        {
            var constructor = type.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault();

            if (constructor == null)
            {
                throw new InvalidOperationException($"No public constructors found for type '{type.FullName}'.");
            }

            var parameters = constructor.GetParameters();
            var args = parameters.Select(p => Get(p.ParameterType)).ToArray();
            var instance = constructor.Invoke(args);

            InjectMethods(instance);

            return instance;
        }

        private void InjectMethods(object instance)
        {
            var methods = instance.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttribute<InjectAttribute>() != null);
            
            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                var args = parameters.Select(p => Get(p.ParameterType)).ToArray();
                method.Invoke(instance, args);
            }
        }
    }
}