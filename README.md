<a name="readme-top"></a>
<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->


<!-- PROJECT LOGO -->

<p align="center">
<img width="300" height="125" src="https://i.imgur.com/w5hcUtR.png">
</p>

<div align="center">

<h3 align="center">Simple Plotting</h3>

  <p align="center">
    A wrapper for parsing, plotting, serializing CSV data and processing bitmaps.
    <br />
    <br />
    <a href="https://github.com//ryan-io/CommandPipeline/issues">Report Bug</a>
    ·
    <a href="https://github.com//ryan-io/CommandPipeline/issues">Request Feature</a>
  </p>
</div>

---
<!-- TABLE OF CONTENTS -->

<details align="center">
  <summary>Table of Contents</summary>
  <ol>
  <li>
      <a href="#overview">Overview</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites-and-dependencies">Prerequisites and Dependencies</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage & Examples</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments-and-credit">Acknowledgments</a></li>
  </ol>
</details>

---

<!-- ABOUT THE PROJECT -->

# Overview

Simple Plotting is a library that acts as a wrapper for [ScottPlot](https://github.com/ScottPlot/ScottPlot) (C# MatPlotLib wrapper) and [CsvHelper](https://github.com/JoshClose/CsvHelper0). This library has been used in the Automotive industry to parse CSV files and plot the data containing data relating to temperature, relative humidity, voltage and current output. This library can be used in conjecture with all types of data.
In addition to generating plots, the API is also used to create interactive plots in various .NET environments. These interactive plots can be annotated, have draggable shapes added to them, have their axes labels, dimensions, colors, ticks, etc. all changed. It is versatile in this regard.
The primary goals for this project are:
1) Parse CSV files and load into memory for manipulation
2) Define how CSV files are parsed
3) Generate plots from the parsed CSV data using MatPlotLib
4) Make the plots interactive (WPF, WinForms, Avalonia)
5) Save generated plots to disk

##### Features
<ol>
<li>
Define a 'strategy' for how CSV files are parsed. A default strategy is provided, but the consumer should define a strategy that aligns with the formatting of the files they are working with.
</li>
<li>
Define the type of plot. ScottPlot supports many.. this library currently supports:
	- Scatter
	- Signal
	- Constant signal
	- Line
	- Image (annotations)
	- 'Crosshair'
	- Markers
</li>
<li>
An abstracted plotting API and an abstracted photo annotation API
</li>
<li>
Plotting API:
	- Set title, axes labels, font (size, color, family, style), ticks, dimensions, colors, output format.
	- Add annotations and shapes to point out critical points in a plot
</li>
<li>
Photo Annotation API:
	- Add images, text and annotations to photos
</li>
</ol>

<p align="right">(<a href="#readme-top">back to top</a>)</p>

# Built With
- JetBrains Rider
- Tested with WPF & Blazor WASM

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- GETTING STARTED -->
# Getting Started
Clone/fork this repository and add a reference to the simple-plotting project.
OR, add this as a Nuget package to your project.

##### Csv Parsing

- Start by defining a 'strategy' for how to parse Csv files. This does require knowledge of CsvHelper and how the API works.
	- The [examples](https://joshclose.github.io/CsvHelper/examples/) provided by Josh (CsvHelper author) is an excellent source for quickly learning what you need
- A default strategy is provided ONLY as an example:

```csharp
public class DefaultCsvParseStrategy : CsvParseStrategy {  
    StringBuilder Sb { get; } = new();  
  
    double? SampleRate { get; set; }  
  
    public override async Task StrategyAsync(  
       List<PlotChannel> output,  
       CsvReader csvr,  
       CancellationToken? cancellationToken = default) {  
       const int skipFourRows = 4;  
       const int skipSixRows  = 6;  
  
       try {  
          SkipRowsNumberOfRows(csvr, skipSixRows);  
  
          await csvr.ReadAsync();  
  
          SampleRate = CsvParserHelper.ExtractSampleRate(csvr[1]); // we could ignore this entirely  
  
          SkipRowsNumberOfRows(csvr, skipFourRows);  
  
          csvr.ReadHeader();  
  
          if (csvr.HeaderRecord == null)  
             throw new Exception(Message.EXCEPTION_NO_HEADER);  
  
          var channelsToParse = csvr.HeaderRecord.Length - 3;  
  
          for (var i = 0; i < channelsToParse; i++) {  
             if (cancellationToken.HasValue && cancellationToken.Value.IsCancellationRequested)  
                break;  
  
             output.Add(new PlotChannel(csvr.HeaderRecord[3 + i], PlotChannelType.Temperature, SampleRate));  
          }  
  
          while (await csvr.ReadAsync()) {  
             if (cancellationToken.WasCancelled())  
                break;  
  
             ParseCurrentReaderIndex(output, csvr, cancellationToken, channelsToParse);  
          }  
       }  
       catch (Exception e) {  
          throw new Exception(Message.EXCEPTION_STRATEGY_FAILED, e);  
       }  
    }
```

- To implement your own strategy, created a class that derives from CsvParseStrategy or implements ICsvParseStrategy
	- I'd love to help you define a strategy for your needs
- Once implemented, simply invoke

```csharp
var initialStrategy = new DefaultCsvParseStrategy(-65d, 165d);  
var source          = CsvParserFactory.StartNew(initialStrategy, path);
var output = await source.ExtractAsync(file);
```

##### Plotting

- Once a csv file has been parsed, you can now feed the parsed data into the PlotBuilderFluent API.
	- This API is fluent, meaning you can chain method calls

```csharp
PlotBuilderFluent.StartNewPlot(output, numOfPlots)
```

- Review the methods included with the API. These methods help you customize the plot

```csharp
.WithType<SignalPlot>()  
.WithTitle("Test title", 24)  
.WithSize(PlotSize.S1280X800)  
.WithPrimaryXAxisLabel(Constants.X_AXIS_LABEL_DATE_TIME, 20)  
.WithPrimaryYAxisLabel(Constants.Y_AXIS_LABEL_TEMP, 20)  
.WithSecondaryYAxisLabel(Constants.Y_AXIS_LABEL_RH, 20)  
.ShowLegend(PlotAlignment.UpperRight)  
.SetDataPadding(valueY: .2, valueX: 0.0)

product.GotoPlottables().AddDraggableLine(0, 50, 40000, out _);  
product.GotoPostProcess().TrySetLabel("Test label", 0);  
product.GotoPlottables().WithAnnotationAt("Test annotation", 1, 100, 100, out _);
```

##### Image Processing

- The image processing API is analogous to the PlotBuilderFluent API and functions identically.
- Start by creating a new instance of BitmapParser. Once this is instantiated, simply query the PlotBuilderFluent API to create a new canvas.

```csharp
var parser  = new BitmapParser(ref paths);  
var builder = PlotBuilderFluent.StartNewCanvas(ref parser.GetAllBitmaps());
```

- You are ready to manipulate the image(s)

```csharp
parser.ModifyRgbUnsafe(0, (ref int red, ref int green, ref int blue) => {  
                              red   -= 25;  
                              green += 10;  
                              blue  += 20;  
                          });
```
<p align="right">(<a href="#readme-top">back to top</a>)</p>


# Prerequisites and Dependencies

- .NET 6
* C# 10
* Microsoft.Extensions.Logging.7.0.0
* [ScottPlot 4.1.67](https://github.com/ScottPlot/ScottPlot)
* [CsvHelper 30.0.1](https://github.com/JoshClose/CsvHelper)
* [csFastFloat 4.1.0](https://github.com/CarlVerret/csFastFloat)

##### Please feel free to contact me with any issues or concerns in regards to the dependencies defined above. We can work around the majority of them if needed.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

# Installation

- Clone or fork this repository. Once done, add a reference to this library in your project
- Download the latest dll and create a reference to it in your project
- Install via NPM

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- USAGE EXAMPLES -->
# Usage

The following is a demonstration of the API within a console application:

```csharp
using ScottPlot.Plottable;  
using simple_plotting;  
using simple_plotting.runtime;
// Program.cs entry point

await GeneratePlots();  
await ModifyImages(); 

return 0;
```

##### Plotting

```csharp
async Task<int> GeneratePlots() {  
    try {  
       // define a file name w/ extension
       var file = "my-test-file.csv";  
	   // define a directory; this is where your data resides
       var path = @"C:\test-data";  
  
       const int numOfPlots = 50;  
  
       // can use CsvParser.StartNew(path) to skip SetSource(path)  
       // this example uses the builtin DefaultCsvParseStrategy       var initialStrategy = new DefaultCsvParseStrategy(-65d, 165d);  
       var source          = CsvParserFactory.StartNew(initialStrategy, path);  
  
       // if you prefer to set the pat manually, you can do the following  
       // var initialStrategy = new DefaultCsvParseStrategy(-65d, 165d);       // var source          = CsvParser.StartNew(initialStrategy);       // source.SetSource(path);  
       // invoke source.ExtractAsync(file) to generate output       var output = await source.ExtractAsync(file);  
  
       // check if output is null  
       if (output == null) {  
          Console.WriteLine("Could not extract data from CSV file...");  
          return -1;  
       }  
  
       // begin building the plot via fluent builder  
       var product = PlotBuilderFluent.StartNewPlot(output, numOfPlots)  
                                      .WithType<SignalPlot>()  
                                      .WithTitle("Test title", 24)  
                                      .WithSize(PlotSize.S1280X800)  
                                      .WithPrimaryXAxisLabel(Constants.X_AXIS_LABEL_DATE_TIME, 20)  
                                      .WithPrimaryYAxisLabel(Constants.Y_AXIS_LABEL_TEMP, 20)  
                                      .WithSecondaryYAxisLabel(Constants.Y_AXIS_LABEL_RH, 20)  
                                      .ShowLegend(PlotAlignment.UpperRight)  
                                      .SetDataPadding(valueY: .2, valueX: 0.0)  
                                      .DefineSource(source)  
                                      .RotatePrimaryXAxisTicks(PlotAxisRotation.FortyFive)  
                                      .RotatePrimaryYAxisTicks(PlotAxisRotation.Zero)  
                                      .SetLayout(right: 123, top: 123)  
                                      .FinalizeConfiguration()  
                                      .Produce();  
  
       product.GotoPlottables().AddDraggableLine(0, 50, 40000, out _);  
       product.GotoPostProcess().TrySetLabel("Test label", 0);  
       product.GotoPlottables().WithAnnotationAt("Test annotation", 1, 100, 100, out _);  
       var status = product.TrySaveAtSource(@"C:\output");  
  
       var set    = new HashSet<SignalPlotXYConst<double, double>>();  
       product.GetPlottablesAsCache(0, ref set);  
         
       Console.WriteLine(status.State ? "Successfully saved plot!" : "Could not save plot...");  
       return 0;  
    }  
    catch (Exception e) {  
       Console.WriteLine(e);  
       throw;  
    }  
}
```

##### Photo Annotation

- The API for photo annotation currently requires unsafe context. 
- There is an API to process images in a managed environment that I can include.
- Thanks to [Turgay](https://csharpexamples.com/fast-image-processing-c/) at 'csharpexamples.com' for inspiration on fast Bitmap processing.

```csharp
async Task ModifyImages() {  
	// get location of the image you want to annotate/process
    var path  = @"C:\my-folder\";
	var file = path + "\my-pic.my-ext";
	
	// define a path array; include as many image paths as you want
    var paths = new[] { file };  

	// instantiate a new instance of the BitmapParser
	// the default constructor takes a reference to an allocated array of strings
    var parser  = new BitmapParser(ref paths);  

	// query the fluent builder to start a new canvas plot
	// A canvas is a type of plot that does not look like a traditional plot
	// It is simply a window for containing your images for manipulation
    var builder = PlotBuilderFluent.StartNewCanvas(ref parser.GetAllBitmaps());  

	// invoke parser.ModifyRgbUnsafe
	// this method takes a delegate that must match:
	// public delegate void BitmapRgbDelegate(ref int red, ref int green, ref int blue);
	// red, green and blue parameters are enumerated for each pixel in the bitmap
	// each parameter is passed by reference; simply modify thes as you see fit
	// modifications to thes values result in a new RGB value for that enumerated pixel
    parser.ModifyRgbUnsafe(0, (ref int red, ref int green, ref int blue) => {  
                                 red   -= 25;  
                                 green += 10;  
                                 blue  += 20;  
                              });  
	// Save the processed bitmaps
    await parser.SaveBitmapsAsync(path);  
	
	// this example manually invokes Dipose
	// a 'using' statement is also appropriate
    parser.Dispose();  
}
```

<!-- ROADMAP -->
# Roadmap

1) Provide a default implementation for processing Bitmaps in a managed (safe) context

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONTRIBUTING -->
# Contributing

Contributions are absolutely welcome. This is an open source project. 

1. Fork the repository
2. Create a feature branch
```Shell
git checkout -b feature/your-feature-branch
```
3. Commit changes on your feature branch
```Shell
git commit -m 'Summary feature'
```
4. Push your changes to your branch
```Shell
git push origin feature/your-feature-branch
```
5. Open a pull request to merge/incorporate your feature

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- LICENSE -->
# License

Distributed under the MIT License.

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- CONTACT -->
# Contact

<p align="center">
<b><u>RyanIO</u></b> 
<br/><br/> 
<a href = "mailto:ryan.io@programmer.net?subject=[RIO]%20Procedural%20Generator%20Project" >[Email]</a>
<br/>
[LinkedIn]
<br/>
<a href="https://github.com/ryan-io">[GitHub]</a></p>

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ACKNOWLEDGMENTS -->
# Acknowledgments and Credit

* Scott over at [ScottPlot](https://scottplot.net/) for his library.
* [Turgay](https://csharpexamples.com/author/turgay/) for fast image processing

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/github_username/repo_name.svg?style=for-the-badge
[contributors-url]: https://github.com/github_username/repo_name/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/github_username/repo_name.svg?style=for-the-badge
[forks-url]: https://github.com/github_username/repo_name/network/members
[stars-shield]: https://img.shields.io/github/stars/github_username/repo_name.svg?style=for-the-badge
[stars-url]: https://github.com/github_username/repo_name/stargazers
[issues-shield]: https://img.shields.io/github/issues/github_username/repo_name.svg?style=for-the-badge
[issues-url]: https://github.com/github_username/repo_name/issues
[license-shield]: https://img.shields.io/github/license/github_username/repo_name.svg?style=for-the-badge
[license-url]: https://github.com/github_username/repo_name/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/linkedin_username
[product-screenshot]: images/screenshot.png
[Next.js]: https://img.shields.io/badge/next.js-000000?style=for-the-badge&logo=nextdotjs&logoColor=white
[Next-url]: https://nextjs.org/
[React.js]: https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB
[React-url]: https://reactjs.org/
[Vue.js]: https://img.shields.io/badge/Vue.js-35495E?style=for-the-badge&logo=vuedotjs&logoColor=4FC08D
[Vue-url]: https://vuejs.org/
[Angular.io]: https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white
[Angular-url]: https://angular.io/
[Svelte.dev]: https://img.shields.io/badge/Svelte-4A4A55?style=for-the-badge&logo=svelte&logoColor=FF3E00
[Svelte-url]: https://svelte.dev/
[Laravel.com]: https://img.shields.io/badge/Laravel-FF2D20?style=for-the-badge&logo=laravel&logoColor=white
[Laravel-url]: https://laravel.com
[Bootstrap.com]: https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white
[Bootstrap-url]: https://getbootstrap.com
[JQuery.com]: https://img.shields.io/badge/jQuery-0769AD?style=for-the-badge&logo=jquery&logoColor=white
[JQuery-url]: https://jquery.com
