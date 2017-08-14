using System;
using System.Reflection;

namespace ServiceStack.Text.Tests.NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            new NUnitLite.AutoRun(typeof(Program).GetTypeInfo().Assembly).Execute(args);
        }
    }
}