# Kentico EMS integration with Recombee.
This repository contains source code for PoC integration of Kentico EMS and Recombee Artificial Intelligence Powered
Recommender as a Service

## :warning: **DISCLAIMER** 
This is a *sample* module, it needs detailed testing and maybe some bug fixing for real production usage. See also [Known issues](#known-issues).

# Description
This project consists of two modules each for one instance (Admin, MVC). After installation of the administration module, you will have available Recombee application in the application menu. This application provides you administration interface to initialize Recombe database. Without the initialized database the module won't work correctly.

The MVC module ensures PageBuilder widget *RecommendedProducts* in your MVC site. By default, the module logs cart actions - purchases and products additions. The widget contains a basic set of CSS styles to be usable on all MVC sites (non-DancingGoat).

# Requirments
1. Kentico EMS 12SP.
1. This module uses online marketing contacts, so EMS license is required.
1. Online marketing module has to be enabled in setting application.
1. You need to have an account on Recombee.com 
   - you can create your free account on Recombee.com or have a project registered in the Kentico organization (contact stanislavs(at)kentico.com for more details) 

## Installation
### General steps
1. Download the latest ZIP package from [Release](../releases)
2. Extract ZIP packag
 
### Administration package
1. Open solution with your administration project (WebApp.sln)
    1. Navigate to "NuGet Package Manager Console". 
    1. Run *Install-Package \<path-to-package-folder>\Kentico.Recombee.Admin.nupkg* where \<path-to-package-folder> is file system path to extracted ZIP package.
2. Build the project
3. Go to settings application and navigate to Integration -> Recombee. Fill Recombe database identifier and secret token.

### Package for MVC site.
1. Open solution with your MVC project (eg. DancingGoatMvc.sln)
   1. Navigate to "NuGet Package Manager Console". 
   1. Run *Install-Package \<path-to-package-folder>\Kentico.Recombee.MVC.nupkg* where \<path-to-package-folder> is file system path to extracted ZIP package.
1. Build the project

#### Enable articles views logging [Optional]
To log articles page views, add the following code into your *ArticlesController*  action *Detail*

```csharp
var contact = ContactManagementContext.GetCurrentContact();

var recombeeClient = new RecombeeClientService();
recombeeClient.LogPageView(contact.ContactGUID, article.DocumentGUID);
```

Please note *Online marketing* feature has to be enabled in the settings application.

#### Installing MVC package on non-DancingGoat based project
The widget can be used on all MVC sites based on K12SP (older version were not tested).
Since the widget renders products list with links to an appropriate detail you will need to add/have ProductController with action Detail (see DancingGoat sample for more details). Alternatively,
you will need to modify the widget view.

The second requirement for non-DG sites is to have [Bottstrap CSS](https://getbootstrap.com/docs/4.3/getting-started/introduction/).

Without previous steps, the MVC widget will couldn't work or can be displayed incorrectly.

# Notes
If you install the MVC package on non-Dancing goat, you will need to add bootstrap and ProductController


### Known issues
- When a new product is added to the E-commerce store, it is not reflected in Recombee
    - Similar problems are known when deleting/changing a product.
- Contact merge is not handled
- Unavailable products (due to selling out) are not handled as well.
