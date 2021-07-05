# CustomCommandBot
A fully customiseable bot developed by [RealSGII2](https://github.com/RealSGII2) and [Blackcatmaxy](https://github.com/Blackcatmaxy),
developed using .NET 6 and Blazor WASM in C#.

## Usage
- The bot may not be invited at this moment.
- The documentation hasn't been created yet either.

## Updates
Updates will be posted in the [Announcements category in the GitHub repository](https://github.com/RealSGII2/CustomCommandBot/discussions/categories/announcements).

## Bugs, Requests, Questions?
- Feature requests **must** be in the [Feature Requests discussion category on GitHub](https://github.com/RealSGII2/CustomCommandBot/discussions/categories/feature-requests).
- Questions for now may be posted in the [Questions discussion category on GitHub](https://github.com/RealSGII2/CustomCommandBot/discussions/categories/questions).
- Bugs may be submitted to the [Issue Tracker on Github](https://github.com/RealSGII2/CustomCommandBot/issues).

## Contributing
Want to contribute to CustomCommandBot? Go ahead.

### Editing the website
#### Styling
The website's styling is written in Sass to make writing styles much easier and efficient. If you're going to be editing styling, you'll
need to install the [Web Compiler](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.WebCompiler) on the Visual Studio
marketplace.

To compile Sass files, you may right click on **index.scss**, and under Web Compiler, click `Recompile file`. This may also be done using
`CTRL + Shift + Q`. 

#### Hot reload
To make testing go faster, we take advantage of ASP.NET's hot reload feature. To use it, simply run `dotnet watch` in the `Server` project.