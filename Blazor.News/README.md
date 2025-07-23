# Blazor.News

Blazor.News is a modular library for building news-related features in Blazor applications, targeting .NET 8. It provides components and utilities for displaying, managing, and editing news articles, with support for validation, database integration, and image processing.

## Features

- News article display and detail components
- Admin forms for creating and editing news articles
- FluentValidation integration for robust form validation
- Entity Framework Core support for SQL Server
- Image processing and SVG asset management
- Designed for Blazor Server and Blazor WebAssembly

## Installation

Add the NuGet package to your project:


Or use the __NuGet Package Manager__ in Visual Studio:
1. Open the __NuGet Package Manager__.
2. Search for `Blazor.News`.
3. Click __Install__.

## Usage

1. Reference the library in your Blazor project.
2. Import the relevant namespaces in your `.razor` files:
2. 3. Use the provided components, such as `ArtikelDetail`, `ArtikelNews`, and `NieuwsArtikelForm`, in your pages.

## Dependencies

- [FluentValidation.AspNetCore](https://www.nuget.org/packages/FluentValidation.AspNetCore)
- [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer)
- [SixLabors.ImageSharp](https://www.nuget.org/packages/SixLabors.ImageSharp)
- [SixLabors.ImageSharp.Drawing](https://www.nuget.org/packages/SixLabors.ImageSharp.Drawing)
- [Blazor.Shared.Editors](../Blazor.Shared.Editors/README.md)

## Assets

SVG files for localization are included in `wwwroot/SVG` and copied to the output directory.

## Target Framework

- .NET 8

## License

Specify your license here (e.g., MIT, Apache-2.0).

## Contributing

Contributions are welcome! Please submit issues or pull requests via GitHub.

---

For more information, see the documentation or contact the maintainer.