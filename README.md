# Primo
A fully customiseable bot developed by [RealSGII2](https://github.com/RealSGII2), [Blackcatmaxy](https://github.com/Blackcatmaxy),
and [Clerically](https://github.com/clerically), developed using .NET 6 and Blazor WASM in C#.

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
Want to contribute to Primo? Go ahead, all contributions are welcome.

### Cloning
Clone the repo like so:
```
$ git clone https://github.com/RealSGII2/CustomCommandBot.git
```

### Prerequisites
- Visual Studio or Rider at its latest preview, with the **ASP.net and web development** package.
- .NET 6 Preview 6 SDK installed

### Testing
To get started with testing, you'll need an Application connected to a bot account. Once you've created one, create a `secrets.yaml` file in `Sever/Config`. Give it the content shown here:
```yaml
bot_token: your-token-here
```

Replace the values nessecary to run the application.

Once you're finished, run `CustomCommandBot.Server` and you're good to go. The databases will be created automatically if they do not exist.

### Editing the website
#### Styling
> Notice: We are changing from Web Compiler to a different extension soon
The website's styling is written in Sass to make writing styles much easier and efficient. If you're going to be editing styling, you'll
need to install the [Web Compiler](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.WebCompiler) on the Visual Studio
marketplace.

To compile Sass files, you may right click on **index.scss**, and under Web Compiler, click `Recompile file`. This may also be done using
`CTRL + Shift + Q`. 

#### Hot reload
To make testing go faster, we take advantage of ASP.NET's hot reload feature. To use it, simply run `dotnet watch` in the `Server` project.