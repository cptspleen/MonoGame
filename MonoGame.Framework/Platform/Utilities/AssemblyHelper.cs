﻿// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoGame.Utilities
{
    internal static class AssemblyHelper
    {
        public static string GetDefaultWindowTitle()
        {
            // Set the window title.
            string windowTitle = string.Empty;

            // When running unit tests this can return null.
            var assembly = Assembly.GetEntryAssembly();
            if (assembly != null)
            {
                // Use the Title attribute of the Assembly if possible.
                try
                {
                    var assemblyTitleAtt = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute)));
                    if (assemblyTitleAtt != null)
                        windowTitle = assemblyTitleAtt.Title;
                }
                catch
                {
                    // Nope, wasn't possible :/
                }

                // Otherwise, fallback to the Name of the assembly.
                if (string.IsNullOrEmpty(windowTitle))
                    windowTitle = assembly.GetName().Name;
            }

            return windowTitle;
        }

        public static IEnumerable<Type> GetAppDomainTypes(Func<Type, bool> filterType)
        {
            var types = new List<Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (filterType(type))
                        {
                            types.Add(type);
                        }
                    }
                }
                catch (ReflectionTypeLoadException)
                {
                    // Do nothing, effectively skip trying to load types from this assembly.
                }
            }

            return types;
        }
    }
}
