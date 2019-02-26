[<img src="https://github.com/GregFinzer/comparenetobjects/blob/master/logo.png">](http://www.kellermansoftware.com)

[<img src="https://github.com/GregFinzer/comparenetobjects/blob/master/PoweredByNDepend.png">](http://www.ndepend.com)

# Project Description
What you have been waiting for. Perform a deep compare of any two .NET objects using reflection. Shows the differences between the two objects.

# Compatibility
* Compatible with .NET Framework 4.0 and higher.  
* .NET Standard 1.3 Build Compatible with .NET Core 1.0, Mono 4.6, Xamarin.iOS 10.0, Xamarin.Mac 3.0, Xamarin.Android 7.0, Universal Windows Platform 10.0
* .NET Standard 2.0 Build Compatible with .NET Core 2.0, Mono 5.4, Xamarin.iOS 10.14, Xamarin.Mac 3.8, Xamarin.Android 8.0, Universal Windows Platform 10.0.16299

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
* NUnit Test Project Included with over **200+** unit tests
* Ability to load settings from a config file for use with powershell
* Ability to pass in the configuration
* Ability to save and load the configuration as json
* Test Extensions .ShouldCompare and .ShouldNotCompare
* Several configuration options for comparing private elements, ignoring specific elements, including specific elements.
* Property and Field Info reflection caching for increased performance
* Rich Differences List or simple DifferencesString
* Difference Callback
* Supports custom comparison functions
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
* Classes
* Dynamic (Expando objects are supported but not DynamicObject)
* Anonymous Types
* Primitive Types (String, Int, Boolean, etc.)
* Structs
* IList Objects
* Collections
* Single and Multi-Dimensional Arrays
* Immutable Arrays
* IDictionary Objects
* Enums
* Timespans
* Guids
* Classes that Implement IList with Integer Indexers
* DataSet Data
* DataTable Data
* DataRow Data
* DataColumn Differences
* LinearGradient
* HashSet
* URI
* IPEndPoint
* Types of Type (RuntimeType)
* StringBuilder

# Limitations
* Custom Collections with Non-Integer Indexers cannot be compared.
* Private properties and fields cannot be compared for .NET Core 1.3.  They are allowed to be compared in .NET Core 2.0 and higher.
* DynamicObject type is not supported but will be implemented in 2020 when the library is versioned to .NET Framework 4.5
* When ignoring the collection order, the collection matching spec cannot be a field on a child or parent class.  It has to be a property or field of the class.  The property has to be a simple type.
* ElapsedMilliseconds does not work for .NET Standard builds as there is no StopWatch in those frameworks.
* If you override Equals and GetHashCode in your classes, Equals and GetHashCode must be implemented correctly. GetHashCode must be unique for each object.  If the hash code is the same for two objects, they will be considered equal.


# Getting Started
https://github.com/GregFinzer/Compare-Net-Objects/wiki/Getting-Started

# Help File
https://github.com/GregFinzer/Compare-Net-Objects/blob/master/Compare-NET-Objects-Help/Compare-NET-Objects-Help.chm?raw=true

# Licensing
https://github.com/GregFinzer/Compare-Net-Objects/wiki/Licensing
