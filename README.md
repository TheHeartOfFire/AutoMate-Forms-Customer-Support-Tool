# Solera Case Management Tool

The Solera Case Management Tool is a modern WPF desktop application designed to streamline various tasks related to case management. It provides a suite of utilities for handling `.formgen` files, generating standardized form names, managing code snippets, and taking notes.

## Features

- **Dashboard**: A central hub for quick access to information and tools.
- **Formgen Utilities**: An editor for `.formgen` files, allowing you to view and manipulate their structure, including properties, settings, and UUIDs.
- **Form Name Generator**: Ensures consistency by generating form names that adhere to best practices.
- **Code Snippets**: A repository for storing and quickly accessing frequently used code snippets.
- **Templates**: Manage and utilize templates to accelerate development workflows.
- **Notebook**: An integrated note-taking feature to keep track of important information related to your cases.
- **Customizable Settings**: Tailor the application's behavior and appearance, including theme (light/dark) and window preferences.
- **Automatic Updates**: The application stays up-to-date automatically using Velopack.

## Technologies Used

This project is built with a modern stack, ensuring a responsive and reliable user experience.

- **.NET 9** and **C# 13**
- **WPF** for the user interface
- **WPF UI (WPF-UI)** for modern controls and styling
- **MVVM (Model-View-ViewModel)** architecture using **CommunityToolkit.Mvvm**
- **Dependency Injection** via `Microsoft.Extensions.Hosting`
- **Serilog** for structured logging
- **Velopack** for easy installation and application updates

## Getting Started

### Installation

The application is distributed via GitHub Releases.

1.  Navigate to the latest release on the project's GitHub page.
2.  Download the `Setup.exe` file.
3.  Run the installer. Velopack will handle the installation and future updates seamlessly.

### Building from Source

To build the project from the source code, you will need:

- Visual Studio 2022 (with .NET desktop development workload)
- .NET 9 SDK

1.  Clone the repository:
    ```sh
    git clone <repository-url>
    ```
2.  Open the solution file (`.sln`) in Visual Studio.
3.  Restore the NuGet packages.
4.  Build and run the `AMFormsCST.Desktop` project.

## Publishing

The project includes build targets for packaging with Velopack and creating a GitHub release.

- **VeloPack**: After publishing the project in `Release` mode, the `VeloPack` target will execute, creating an installer and update packages.
- **GitHub Release**: The `GitHub Release` target uses the `gh` CLI to create a new release, generate notes, and upload the packaged application files.