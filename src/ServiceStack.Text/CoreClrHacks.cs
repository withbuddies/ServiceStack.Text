using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

#if CORE_CLR
using Microsoft.Extensions.DependencyModel;
internal static class TypeExtensions
{
    public static MethodInfo GetMethod(this Type type, string name, Type[] parameterTypes) => type.GetTypeInfo().GetMethod(name, parameterTypes);
}
internal static class MemoryStreamExtensions
{
    public static byte[] GetBuffer(this MemoryStream value)
    {
        ArraySegment<byte> buff;
        if (!value.TryGetBuffer(out buff))
        {
            throw new Exception("Buffer not exposable");
        }
        return buff.Array;
    }
}
internal class AppDomain
{
    public static AppDomain CurrentDomain { get; private set; }

    static AppDomain()
    {
        CurrentDomain = new AppDomain();
    }

    public Assembly[] GetAssemblies()
    {
        var assemblies = new List<Assembly>();
        var dependencies = DependencyContext.Default.RuntimeLibraries;
        foreach (var library in dependencies)
        {
            if (IsCandidateCompilationLibrary(library))
            {
                var assembly = Assembly.Load(new AssemblyName(library.Name));
                assemblies.Add(assembly);
            }
        }
        return assemblies.ToArray();
    }

    private static bool IsCandidateCompilationLibrary(RuntimeLibrary compilationLibrary)
    {
        return compilationLibrary.Name == ("Specify")
            || compilationLibrary.Dependencies.Any(d => d.Name.StartsWith("Specify"));
    }
}
#else
namespace System
{
    internal static class MethodInfoExtensions
    {
        public static Delegate CreateDelegate(this MethodInfo method, Type delegateType)
            => Delegate.CreateDelegate(delegateType, method);
    }
}
namespace System.Reflection
{
    internal static class TypeExtensions
    {
        public static Type GetTypeInfo(this Type type) => type;
    }
}
#endif
