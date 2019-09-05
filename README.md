# ems-recombee-integration
Kentico EMS integration with Recombee.

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
2. Build the project



# Notes
If you install the MVC package on non-Dancing goat, you will need to add bootstrap and ProductController