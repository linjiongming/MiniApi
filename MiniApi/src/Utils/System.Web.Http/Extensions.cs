using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Services;

namespace System.Web.Http
{
    public static class Extensions
    {
        public static void SetService<T>(this ServicesContainer services, T instance) where T : class
        {
            if (!(services is DefaultServices defaultServices)) return;

            Type containerType = typeof(DefaultServices);
            Type instanceType = typeof(T);

            FieldInfo mapField = containerType.GetField("_defaultServicesSingle", BindingFlags.NonPublic | BindingFlags.Instance);

            if (mapField == null) return;

            if (!(mapField.GetValue(defaultServices) is Dictionary<Type, object> _defaultServicesSingle)) return;

            _defaultServicesSingle[instanceType] = instance;

            FieldInfo hsField = containerType.GetField("_serviceTypesSingle", BindingFlags.NonPublic | BindingFlags.Instance);

            if (hsField == null) return;

            if (!(hsField.GetValue(defaultServices) is HashSet<Type> _serviceTypesSingle)) return;

            if (!_serviceTypesSingle.Contains(instanceType))
            {
                _serviceTypesSingle.Add(instanceType);
            }
        }
    }
}
