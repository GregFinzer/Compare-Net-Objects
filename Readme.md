[<img src="https://github.com/GregFinzer/comparenetobjects/blob/master/logo.png">](http://www.kellermansoftware.com)

[<img src="https://github.com/GregFinzer/comparenetobjects/blob/master/PoweredByNDepend.png">](http://www.ndepend.com)

# Kellerman Software Closing as of February 29th, 2024
As of February 29th, 2024, Kellerman Software will be closing up shop. We sincerely appreciate every purchase and kind word. The sales have allowed us to fund many cool products.  This repository will no longer be maintained.

The reason for the closure is simply this; the current manager for the Bed Brigade Grove City chapter is moving and God has put it on my heart to fill that position. Bed Brigade is a charity that builds and delivers beds for people that don't have them. 80% of the beds go to children that do not have a bed; the rest is mostly single mothers.

More information about Bed Brigade.
https://www.bedbrigadecolumbus.org/

Questions you may have:
1. Will Kellerman Software be sold to a third party? We tried to sell Kellerman Software several years ago and no-one was interested except for one low ball offer that was less than the profit for a single month. If you are interested in making an offer let us know.
2. Will the products on Kellerman Software become open source? No. Open source software requires more support than commercial software as there are significantly more users.
3. How do I get to heaven?

Romans 3:23 For all have sinned and fall short of the glory of God.
Romans 5:8 But God demonstrates His own love toward us, in that while we were yet sinners, Christ died for us.
Romans 6:23 For the wages of sin is death, but the free gift of God is eternal life in Christ Jesus our Lord.
Romans 10:9-10 If you confess with your mouth Jesus as Lord, and believe in your heart that God raised Him from the dead, you will be saved; for with the heart a person believes, resulting in righteousness, and with the mouth he confesses, resulting in salvation.

Sincerely,

Greg Finzer

CEO, Kellerman Software

# Project Description
What you have been waiting for. Perform a deep compare of any two .NET objects using reflection. Shows the differences between the two objects.

# Compatibility
* Compatible with .NET Framework 4.0 and higher.  
* .NET Standard 1.3 Build Compatible with .NET Core 1.0, Mono 4.6, Xamarin.iOS 10.0, Xamarin.Mac 3.0, Xamarin.Android 7.0, Universal Windows Platform 10.0
* .NET Standard 2.0 Build Compatible with .NET Core 2.0, Mono 5.4, Xamarin.iOS 10.14, Xamarin.Mac 3.8, Xamarin.Android 8.0, Universal Windows Platform 10.0.16299, Unity 2018.1
* .NET Standard 2.1 Build Compatible with .NET 5, .NET 6, .NET 7, .NET 8, Mono 6.4, Xamarin.iOS 12.16, Xamarin.Mac 5.16, Xamarin.Android 10.0

# NuGet Package

<a href="https://ci.appveyor.com/project/GregFinzer/compare-net-objects">
  <img src="https://ci.appveyor.com/api/projects/status/pi60wxnpsre5gu3f?svg=true" alt="AppVeyor Status" height="50">
</a>


<a href="https://www.nuget.org/packages/CompareNETObjects">
  <img src="http://img.shields.io/nuget/v/CompareNETObjects.svg" alt="NuGet Version" height="50">
</a>

<a href="https://www.nuget.org/packages/CompareNETObjects">
  <img src="https://img.shields.io/nuget/dt/CompareNETObjects.svg" alt="NuGet Downloads" height="50">
</a>

http://www.nuget.org/packages/CompareNETObjects

## Installation

Install with NuGet Package Manager Console
```
Install-Package CompareNETObjects
```

Install with .NET CLI
```
dotnet add package CompareNETObjects
```

# Features

## Feature Overview
* Compare Children (on by default)
* Handling for Trees with Children Pointing To Parents (Circular References)
* Compares Publicly Visible Class Fields and Properties
* Compares Private Fields and Properties (off by default)
* Source code in C#
* NUnit Test Project Included with over **275+** unit tests
* Ability to load settings from a config file for use with powershell
* Ability to pass in the configuration
* Ability to save and load the configuration as json
* Test Extensions .ShouldCompare and .ShouldNotCompare
* Several configuration options for comparing private elements, ignoring specific elements, including specific elements.
* Property and Field Info reflection caching for increased performance
* Rich Differences List or simple DifferencesString
* Difference Callback
* Supports custom comparison for types and properties
* ElapsedMilliseconds indicates how long the comparison took
* Thread Safe
* Beyond Compare Report
* WinMerge Report
* CSV Report
* User Friendly Report 
* HTML Report

## Options
* Ability to IgnoreCollectionOrder to compare lists of different lengths
* Ability to ignore indexer comparison
* Ability to ignore types
* Ability to ignore specific members by name or by wildcard
* Interface member filtering
* Ability to treat string.empty and null as equal
* Ability to ignore string leading and trailing whitespace
* Case insensitive option for strings
* Ignore millisecond differences between DateTime values or DateTimeOffset values
* Precision for double or decimal values

## Supported Types
* Anonymous Types
* Arrays (Single, Multi-Dimensional, and Immutable)
* Classes
* Collections
* DataColumn
* DataRow
* DataSet
* DataTable
* DateOnly (.NET Core 6.0 or highter)
* DateTime
* DateTimeOffset
* Dictionary
* Dynamic (Expando objects and Dynamic objects are supported)
* Enum
* Fields
* Font (Windows Only)
* Guid
* HashSet
* IDictionary
* IList
* IntPtr
* IPEndPoint (Supported for everything except .NET Standard 1.0)
* LinearGradient
* List
* Primitive Types (String, Int, Boolean, etc.)
* Properties
* SByte
* StringBuilder
* Struct
* Timespans
* HashSet
* TimeOnly (.NET Core 6.0 or highter)
* Timespan
* Types of Type (RuntimeType)
* URI

# Limitations
* Custom Collections with Non-Integer Indexers cannot be compared.
* Private properties and fields cannot be compared for .NET Core 1.3.  They are allowed to be compared in .NET Core 2.0 and higher.
* When ignoring the collection order, the collection matching spec must be a property on the class.  It cannot be a field or a property  on a child or parent class.  The property has to be a simple type.
* COM Objects are not compared.  To compare COM objects wrap their properties in a .NET Object or create a <a href="https://github.com/GregFinzer/Compare-Net-Objects/wiki/Custom-Comparers">custom comparer</a>.  Also See:  https://stackoverflow.com/questions/9735394/reflection-on-com-interop-objects
* Version 4.62 and earlier used the hash code to identify objects to keep track of parents and children. In later versions the object reference is used.  The reason for this change is that developers were overriding the GetHashCode.  If you are overriding equals in your project with Compare .NET Objects inside, you will need to set *Config.UseHashCodeIdentifier = true* or it will cause a stack overflow.  See this issue:  https://github.com/GregFinzer/Compare-Net-Objects/issues/282.  See the proper way to override equals:  https://github.com/GregFinzer/Compare-Net-Objects/wiki/Overriding-Equals


# Getting Started
https://github.com/GregFinzer/Compare-Net-Objects/wiki/Getting-Started

# Help File
https://github.com/GregFinzer/Compare-Net-Objects/blob/master/Compare-NET-Objects-Help/Compare-NET-Objects-Help.chm?raw=true

# Licensing
Compare .NET Objects is an open source project with an Ms-PL license with no commercial support.  It is free to use and distribute for commercial and non-commercial purposes.  Below is a link to the licensing.  
https://github.com/GregFinzer/Compare-Net-Objects/wiki/Licensing

If you would like a perpetual MIT license with commercial support for a period of one year for all developers at your organization, purchase this option:
https://kellermansoftware.com/products/compare-net-objects
