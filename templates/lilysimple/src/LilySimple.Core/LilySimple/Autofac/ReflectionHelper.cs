using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace LilySimple.Autofac
{
    public class ReflectionHelper
    {
        /// <summary>
        /// 获取当前项目的程序集，排除所有的系统程序集(Microsoft.***、System.***等)、Nuget下载包
        /// </summary>
        /// <returns></returns>
        public static IList<Assembly> GetAllAssemblies()
        {
            List<Assembly> list = new List<Assembly>();
            var deps = DependencyContext.Default;
            //只注入当前项目的程序集
            var libs = deps.CompileLibraries.Where(lib => lib.Type == "project");
            foreach (var lib in libs)
            {
                try
                {
                    var asm = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                    list.Add(asm);
                }
                catch (Exception)
                {
                }
            }
            return list;
        }
    }
}
