[<img src="https://github.com/GregFinzer/comparenetobjects/blob/master/logo.png">](http://www.kellermansoftware.com)

[<img src="https://github.com/GregFinzer/comparenetobjects/blob/master/PoweredByNDepend.png">](http://www.ndepend.com)

# Project Description
What you have been waiting for. Perform a deep compare of any two .NET objects using reflection. Shows the differences between the two objects.

# Compatibility
Compatible with .NET Framework 4.0 and higher.  .NET Core 1.3 and higher. Portable Class Library version works with .NET 4.0+, Silverlight 5+, Windows Phone 8+, Windows RT 8+, Xamarin iOS, and Xamarin Droid.  

# NuGet Package
[![NuGet](http://img.shields.io/nuget/v/CompareNETObjects.svg)](https://www.nuget.org/packages/CompareNETObjects/)

http://www.nuget.org/packages/CompareNETObjects

# Features

## Feature Overview
* Compare Children (on by default)
* Handling for Trees with Children Pointing To Parents (Circular References)
* Compares Publicly Visible Class Fields and Properties
* Compares Private Fields and Properties (off by default)
* Source code in C#
* NUnit Test Project Included
* Ability to load settings from a config file for use with powershell
* Ability to pass in the configuration
* Several configuration options for comparing private elements, ignoring specific elements, including specific elements.
* Property and Field Info reflection caching for increased performance
* Rich Differences List or simple DifferencesString
* Supports custom comparison functions
* ElapsedMilliseconds indicates how long the comparison took
* Thread Safe
* Beyond Compare Report
* WinMerge Report
* CSV Report
* User Friendly Report 

## Options
* Ability to IgnoreCollectionOrder to compare lists of different lengths
* Ability to ignore indexer comparison
* Ability to ignore types
* Ability to ignore specific members by name or by wildcard
* Interface member filtering
* Ability to treat string.empty and null as equal
* Case insensitive option for strings

## Supported Types
* Classes
* Dynamic Objects
* Primitive Types (String, Int, Boolean, etc.)
* Structs
* IList Objects
* Single and Multi-Dimensional Arrays
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

# Limitations
* Custom Collections with Non-Integer Indexers cannot be compared.
* When ignoring the collection order, the collection matching spec cannot be a field on a child class.  It has to be a property or field of the class.
* The Portable and .NET Standard builds cannot compare private fields or properties on a classs.  This is a security restriction by Microsoft.
* ElapsedMilliseconds does not work for Portable and .NET Standard builds as there is no StopWatch in those frameworks.


# Getting Started
https://github.com/GregFinzer/Compare-Net-Objects/wiki/Getting-Started

# Help File
https://github.com/GregFinzer/Compare-Net-Objects/blob/master/Compare-NET-Objects-Help/Compare-NET-Objects-Help.chm?raw=true

# Licensing
https://github.com/GregFinzer/Compare-Net-Objects/wiki/Licensing
