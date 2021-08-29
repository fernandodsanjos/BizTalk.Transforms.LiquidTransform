
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
namespace TestLiquid
{
    public static class Extensions
    {
        public static IEnumerable<Type> GetInterfacesAndSelf(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException();
            if (type.IsInterface)
                return new[] { type }.Concat(type.GetInterfaces());
            else
                return type.GetInterfaces();
        }

        public static IEnumerable<KeyValuePair<Type, Type>> GetDictionaryKeyValueTypes(this Type type)
        {
            foreach (Type intType in type.GetInterfacesAndSelf())
            {
                if (intType.IsGenericType
                    && intType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                {
                    var args = intType.GetGenericArguments();
                    if (args.Length == 2)
                        yield return new KeyValuePair<Type, Type>(args[0], args[1]);
                }
            }
        }
    }
}
