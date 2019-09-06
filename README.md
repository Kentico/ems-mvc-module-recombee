# Kentico EMS integration with Recombee.
This repository contains source code for PoC integration of Kentico EMS and Recombee Artificial Intelligence Powered
Recommender as a Service

## :warning: **DISCLAIMER** 
This is a *sample* module, it needs detailed testing and maybe some bug fixing for real production usage. See also [Known issues](#known-issues).

# Description
TODO
# Requirments
1. This module uses online marketing contacts, so EMS license is reqiured.
1. Online marketing module has to be enabled in setting application.
1. You need to have an account on Recombee.com 
   - you can create your own free account on Recombee.com or have a project registered in the Kentico organization (contact stanislavs(at)kentico.com for more details) 

## Installation
### General steps
1. Download the latest ZIP package from [Release]
2. Extract ZIP packag
 
### Administration package
1. Open solution with your administration project (WebApp.sln)
	1. Navigate to "NuGet Package Manager Console". 
	1. Run *Install-Package \<path-to-package-folder>\Kentico.Recombee.Admin.nupkg* where \<path-to-package-folder> is file system path to extracted ZIP package.
2. Build the project

### Package for MVC site.
1. Open solution with your MVC project (eg. DancingGoatMvc.sln)
   1. Navigate to "NuGet Package Manager Console". 
   1. Run *Install-Package \<path-to-package-folder>\Kentico.Recombee.MVC.nupkg* where \<path-to-package-folder> is file system path to extracted ZIP package.
1. Build the project

#### Enable articles views logging
If you want to log also articles page views, you need to add folowing code into your *ArticlesController*  action *Detail*

```csharp
var contact = ContactManagementContext.GetCurrentContact();

var recombeeClient = new RecombeeClientService();
recombeeClient.LogPageView(contact.ContactGUID, article.DocumentGUID);
```

Please note *Online marketing* feature has to be enabled in settings application.


# Notes
If you install the MVC package on non-Dancing goat, you will need to add bootstrap and ProductController


### Known issues