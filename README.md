# Kentico EMS integration with Recombee.
This repository contains source code for PoC integration of Kentico EMS and Recombee Artificial Intelligence Powered
Recommender as a Service.
### :warning: **DISCLAIMER** 
> This is a *sample* module. Further detailed testing and possibly bug fixing is needed for real production usage. See [Known issues](../../issues) for more information.
## Description
This project consists of two modules, Administration and MVC, each for one instance. After installation of the Administration module, you can access the Recombee application from the application menu. The Recombee application provides you with an interface for initialization of the Recombee database. A properly initialized database is necessary for the Administration module to work correctly.
The MVC module ensures the *RecommendedProducts* page builder widget on your MVC site. By default, the MVC module logs shopping cart actions -- purchases and product additions. The *RecommendedProducts* widget includes only a basic set of CSS styles usable on your other MVC sites.
## Requirements
1. *Kentico 12 Service Pack* installed.
1. The integration uses online marketing features (e.g. Contacts), so the *Kentico EMS* license is required.
1. The *Online marketing* module has to be enabled in the *Settings* application.
1. You need to have an account on Recombee.com 
   - you can create your own free account on Recombee.com or have your project registered in the Kentico organization (contact stanislavs(at)kentico.com for more details) 
## Installation
### General steps
1. Download the latest ZIP package from [Releases](../../releases).
2. Extract the ZIP package.
 
### Administration package
1. Open the solution with your administration project (WebApp.sln)
    1. Navigate to the *NuGet Package Manager Console*. 
    1. Run *Install-Package \<path-to-package-folder>\Kentico.Recombee.Admin.nupkg* where \<path-to-package-folder> is the file system path to the extracted ZIP package.
2. Build the project.
3. Go to the *Settings* application and navigate to *Integration* -> *Recombee*. Fill in the *Recombee database identifier* and *Secret token* fields.
### Package for MVC site.
1. Open the solution with your MVC project (eg. DancingGoatMvc.sln)
   1. Navigate to the *NuGet Package Manager Console*. 
   1. Run *Install-Package \<path-to-package-folder>\Kentico.Recombee.MVC.nupkg* where \<path-to-package-folder> is the file system path to the extracted ZIP package.
1. Build the project.
#### Enable articles views logging [Optional]
To log article page views, add the following code into your *ArticlesController*  action *Detail*
```csharp
var contact = ContactManagementContext.GetCurrentContact();
var recombeeClient = new RecombeeClientService();
recombeeClient.LogPageView(contact.ContactGUID, article.DocumentGUID);
```
Please note that the *Online marketing* feature has to be enabled in the *Settings* application.
#### Installing the MVC package on other projects than the DancingGoat sample site
The *RecommendedProducts* widget can be used on all MVC sites based on Kentico 12 Service Pack (older Kentico versions were not tested).
Because the widget renders the products list with links to an appropriate detail, you will need to create a *ProductController* with a *Detail* action (see the DancingGoat sample site for more details). Alternatively, you will need to modify the widget view.
The second requirement for projects other than the DancingGoat sample site is to have the [Bottstrap CSS](https://getbootstrap.com/docs/4.3/getting-started/introduction/) linked on the page with the widget.
Without the previously mentioned steps, the MVC widget may not work or can be displayed incorrectly.

![Analytics](https://kentico-ga-beacon.azurewebsites.net/api/UA-69014260-4/Kentico/ems-mvc-module-recombee?pixel)
