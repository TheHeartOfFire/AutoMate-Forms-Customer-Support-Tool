﻿using AMFormsCST.Core;
using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Interfaces.UserSettings;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Services;
using AMFormsCST.Core.Types;
using AMFormsCST.Core.Types.BestPractices.Models;
using AMFormsCST.Core.Types.BestPractices.Practices;
using AMFormsCST.Core.Types.UserSettings;
using AMFormsCST.Core.Utils;
using AMFormsCST.Desktop.DependencyModel;
using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Models.UserSettings;
using AMFormsCST.Desktop.Resources;
using AMFormsCST.Desktop.Services;
using AMFormsCST.Desktop.ViewModels;
using AMFormsCST.Desktop.ViewModels.Pages;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using AMFormsCST.Desktop.Views.Pages;
using AMFormsCST.Desktop.Views.Pages.Tools;
using Lepo.i18n.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Formatting.Compact;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Windows;
using System.Windows.Threading;
using Velopack;
using Wpf.Ui;
using Wpf.Ui.DependencyInjection;

namespace AMFormsCST.Desktop;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private static readonly IHost _host = BuildHost();

    private static IHost BuildHost()
    {
        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers = { AddPolymorphicTypes }
            }
        };

        IO.ConfigureJson(jsonOptions);

        return Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(c =>
            {
                _ = c.SetBasePath(AppContext.BaseDirectory);
            })
            .ConfigureServices((_1, services) =>
            {
                _ = services.AddNavigationViewPageProvider();
                services.AddSingleton<ILogService>(sp =>
                {
                    var logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.Console()
                        .WriteTo.File(
                        path: "logs\\app.log", 
                        formatter: new CompactJsonFormatter(), 
                        rollingInterval: RollingInterval.Day)
                        .Enrich.FromLogContext()
                        .CreateLogger();

                    return new SerilogService(logger);
                });
                _ = services.AddTransient<IDebounceService, DebounceService>();

                _ = services.AddHostedService<ApplicationHostService>();

                _ = services.AddSingleton<IWindow, MainWindow>();
                _ = services.AddSingleton<MainWindowViewModel>();
                _ = services.AddSingleton<INavigationService, NavigationService>();
                _ = services.AddSingleton<ISnackbarService, SnackbarService>();
                _ = services.AddSingleton<IContentDialogService, ContentDialogService>();
                _ = services.AddSingleton<WindowsProviderService>();

                _ = services.AddSingleton<DashboardPage>();
                _ = services.AddSingleton<DashboardViewModel>();
                _ = services.AddSingleton<ToolsPage>();
                _ = services.AddSingleton<ToolsViewModel>();
                _ = services.AddSingleton<SettingsPage>();
                _ = services.AddSingleton<SettingsViewModel>();

                _ = services.AddTransientFromNamespace("AMFormsCST.Desktop.Views", GalleryAssembly.Assembly);
                _ = services.AddTransientFromNamespace(
                    "AMFormsCST.Desktop.ViewModels",
                    GalleryAssembly.Assembly
                );

                _ = services.AddTransient<IDialogService, DialogService>();
                _ = services.AddSingleton<IFileSystem, FileSystem>();
                _ = services.AddTransient<IFormgenUtils, FormgenUtils>();
                _ = services.AddTransient<FormgenUtilsProperties>();

                _ = services.AddSingleton<AutoMateFormModel>();
                _ = services.AddSingleton<IFormNameBestPractice, AutoMateFormNameBestPractices>();
                _ = services.AddSingleton<ITemplateRepository, TemplateRepository>();
                _ = services.AddSingleton<IBestPracticeEnforcer, BestPracticeEnforcer>();
                _ = services.AddSingleton<INotebook, Notebook>();
                services.AddSingleton<IOrgVariables>(sp =>
                {
                    var enforcer = sp.GetRequiredService<IBestPracticeEnforcer>();
                    var notebook = sp.GetRequiredService<INotebook>();
                    var orgVars = new AutomateFormsOrgVariables(enforcer, notebook);

                    return orgVars;
                });
                _ = services.AddSingleton<ISupportTool, SupportTool>();
                _ = services.AddSingleton<IUpdateManagerService, UpdateManagerService>();
                _ = services.AddSingleton<IUiSettings, UiSettings>();
                _ = services.AddSingleton<IUserSettings, UserSettings>();
                _ = services.AddSingleton<ISettings, Settings>();

                _ = services.AddStringLocalizer(b =>
                {
                    b.FromResource<Translations>(new("pl-PL"));
                });
            })
            .Build();
    }
    private static void AddPolymorphicTypes(JsonTypeInfo jsonTypeInfo)
    {
        if (jsonTypeInfo.Type == typeof(ISettings))
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$type",
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes = { new(typeof(Settings), "settings") }
            };
        }

        if (jsonTypeInfo.Type == typeof(IUserSettings))
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$type",
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes = { new(typeof(UserSettings), "usersettings") }
            };
        }

        if (jsonTypeInfo.Type == typeof(IUiSettings))
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$type",
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes = { new(typeof(UiSettings), "uisettings") }
            };
        }

        if (jsonTypeInfo.Type == typeof(ISetting))
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$type",
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    new(typeof(UserSettings), "usersettings"),
                    new(typeof(UiSettings), "uisettings"),
                    new(typeof(AutomateFormsOrgVariables), "orgvars"),

                    new(typeof(ThemeSetting), "themesetting"),
                    new(typeof(AlwaysOnTopSetting), "alwaysontopsetting"),
                    new(typeof(NewTemplateSetting), "newtemplatesetting"),
                    new(typeof(CapitalizeFormCodeSetting), "capitalizeformcodesetting")
                }
            };
        }
    }

    /// <summary>
    /// Gets registered service.
    /// </summary>
    /// <typeparam name="T">Type of the service to get.</typeparam>
    /// <returns>Instance of the service or <see langword="null"/>.</returns>
    public static T GetRequiredService<T>()
        where T : class
    {
        return _host.Services.GetRequiredService<T>();
    }

    /// <summary>
    /// Occurs when the application is loading.
    /// </summary>
    private void OnStartup(object sender, StartupEventArgs e)
    {
        var velopackLogger = new VelopackSerilogLogger(GetRequiredService<ILogService>());
        VelopackApp.Build().SetLogger(velopackLogger).Run();
        _host.Start();

    }

    /// <summary>
    /// Occurs when the application is closing.
    /// </summary>
    private void OnExit(object sender, ExitEventArgs e)
    {
        var supportTool = GetRequiredService<ISupportTool>();
        supportTool.SaveAllSettings();

        _host.StopAsync().Wait();
        _host.Dispose();
    }

    /// <summary>
    /// Occurs when an exception is thrown by an application but not handled.
    /// </summary>
    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) { }
}

