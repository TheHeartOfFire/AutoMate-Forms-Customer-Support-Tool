// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using AMFormsCST.Desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AMFormsCST.Desktop.DependencyModel;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTransientFromNamespace(
        this IServiceCollection services,
        string namespaceName,
        params Assembly[] assemblies
    )
    {
        foreach (Assembly assembly in assemblies)
        {
            IEnumerable<Type> types = assembly
                .GetTypes()
                .Where(x =>
                    x.IsClass
                    && x.Namespace != null
                    && x.Namespace.StartsWith(namespaceName, StringComparison.InvariantCultureIgnoreCase)
                );

            foreach (Type? type in types)
            {
                if (services.All(x => x.ServiceType != type))
                {
                    if (
                        type.Name == nameof(ViewModel)
                        && type.Namespace == typeof(ViewModel).Namespace
                    )
                    {
                        continue;
                    }

                    _ = services.AddTransient(type);
                }
            }
        }

        return services;
    }
}