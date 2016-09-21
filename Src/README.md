# Contoso Insurance - Azure App Services Code Sample #

## What is Contoso Insurance? ##
Contoso Insurance is a sample that demonstrates the advantages of using the Azure App Service for building Modern Applications.  It demonstrates using the following Azure App Services features.

- App Service authentication/authorization
- Continuous integration and deployment
- Mobile app server SDK
- Mobile offline sync client SDK
- Mobile file sync SDK
- Logic Apps
	- Invoking Azure Functions via web hooks
	- Sending Email
- Azure Functions
	- Web hook triggers
	- Queue triggers
- Azure Cognitive Services - Computer Vision API

The sample also demonstrates how to use other technologies including:

- ASP.NET MVC Web Site with Knockout
- ASP.NET MVC Web API
- Xamarin Forms Mobile Application
- Azure Application Insights
- Azure PAAS SQL Databases
- Entity Framework
- Azure Storage Accounts
	- Queues
	- Blobs
- Extension methods
- Custom Attributes

## Documentation ##

**Scenario Flow Documentation**
See the [Contoso Insurance Visio Document](/Contoso Insurance.vsdx) for an end to end picture of the flow of the entire scenario and the components used all along the way.

**Technical Documentation**

See the [Azure Components document](/Azure Components.docx) for a complete and detailed description of all of the components that implement this sample.  This document includes:

- Components list
- Logic Apps Diagrams
- Mobile Claims SQL Database Schema and description
- CRM Claims SQL Database Schema and description
- Application Insights Status Logging Matrix
- User Matrix
- Email Matrix
- How to add new customer users 

## How To: Configure your Development Environment ##

Download and install the following tools to build and/or develop this application locally.

- [Visual Studio 2015 Community](https://go.microsoft.com/fwlink/?LinkId=691978&clcid=0x409)
- [Xamarin Platform for Visual Studio](https://xamarin.com/platform)

## How To: Deploy the Demo ##

**Create an Computer Vision Account**

1. Open https://www.microsoft.com/cognitive-services/en-us/sign-up in your web browser.
1. Click Let's Go
1. Sign in
1. Select the **Computer Vision - Preview** check box
1. Select the **I agree to the Microsoft Cognitive Services Terms and Microsoft Privacy Statement** check box
1. Click **Subscribe**
1. Copy the Computer Vision Services keys and save them in a text file.  You will need one of them in a subsequent step.

**Deploy the Azure Components**

1. Check to ensure that the build is passing VSTS Build
2. Fork this repository to your GitHub account
3. Click the Deploy to Azure Button
[![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3a%2f%2fraw.githubusercontent.com%2fTylerLu%2fContosoInsurance%2fmaster%2fSrc%2fazuredeploy.json)
4. Fill in the values in the deployment page.
![](Images/Deployment/Define-Deployment-Parameters.png)
5. If deploying from the main repo, use true for ManualIntegration, otherwise use false. This parameter controls whether or not a webhook is created when you deploy. If you don't have permissions to the repo and it tries to create a webhook (i.e., ManualIntegration is false, then deployment will fail).
>**IMPORTANT Note:**: If you set this value to false then follow the steps in the GitHub Authorization section in this document befoe you clikc the Create button to deploy the Azure components.

6. When the deployment steps complete, it will provide a link to the Web App.

>**Notes:** 
>- The deployment creates a Basic (B1) hosting plan in Azure where all of the components are deployed.
>- The deployment process takes about 8 minutes.

**Update the Contoso_ClaimAutoApproverUrl App Setting in the Function App (`<<Your Site Name>>-function`)**
	
1. Get the Url for the ContosoInsuranceClaimAutoApprover Logic App:	

	![Get the ClaimAutoApproverUrl](Images/Deployment/01-get-claim-auto-approver-url.png)

1. Update the **Contoso_ClaimAutoApproverUrl** App Setting for the Function App:	
	
	![Update appsettings of the Function App](Images/Deployment/02-update-appsettings-of-the-function-app.png)

**Initialize databases with sample data**

1. Execute the **/Cloud/InitMobileClaims.sql** SQL script against the MobileClaims database. 
2. Execute the **/Cloud/InitCRMClaims.sql** SQL script against the CRMClaims database. 

**Initialize the Storage Account with sample images**

1. Get the name and key of the Storage Account.
		
	![Get the name and key of the storage account](Images/Deployment/get-name-and-key-of-the-storage-account.png)

1. Execute the **/Cloud/InitStorage.ps1** PowerShell script. 

>**Note:** Use the Storage Account Name and Storage Account Keys associated with your Storage Account.

```
./InitStorage.ps1 <<Your Storage Account Name>> <<Your Storage Account Key>>
```

**Populate demo data**

1. Execute the **/Cloud/Demo Data/MobileClaims.sql** SQL Script against the MobileClaims database. 
1. Update the Storage Account Name in the **/Src/Cloud/Demo Data/CRMClaims.sql** SQL script.

~~~
DECLARE @imageUrlBase nvarchar(255) = 'https://<Your Storage Account Name>.blob.core.windows.net/vehicle-images/vehicle-';
~~~	

For example:

If your storage account name is contosoinsurancestorage then the updated value will look like this:

~~~	
DECLARE @imageUrlBase nvarchar(255) = 'https://contosoinsurancestorage.blob.core.windows.net/vehicle-images/vehicle-';
~~~

1. Execute the **/Cloud/Demo Data/CRMClaims.sql** SQL script against the CRMClaims database. 

**Upload vehicle images:**

1. Execute the **/Cloud/UploadVehicleImages.ps1** PowerShell script. 

>**Note:** Use the Storage Account Name and Storage Account Keys associated with your Storage Account.

~~~
./UploadVehicleImages.ps1 <<Your Storage Account Name>> <<Your Storage Account Key>>
~~~

**Configure the API App (`<<your site name>>-api`) to use Microsoft Authentication.**

[How to configure your App Service application to use Microsoft Account login](https://azure.microsoft.com/en-us/documentation/articles/app-service-mobile-how-to-configure-microsoft-authentication/)

>**IMPORTANT Note:** Ensure that the Action to take when a request is not authenticated is set to **Allow request (no action)**

**Configure the Web App (`<<your site name>>`) to use AAD Authentication.**
	
[How to configure your App Service application to use Azure Active Directory login](https://azure.microsoft.com/en-us/documentation/articles/app-service-mobile-how-to-configure-active-directory-authentication/)
	
Follow the steps in the **Manually configure Azure Active Directory with advanced settings** section.

>**IMPORTANT Note:** Ensure that the Action to take when a request is not authenticated is set to **Log in with Azure Active Directory**

**Authenticate the Office365 API Connection**

1. **Tyler**, please document how this is done.

	![Authenticate the Office365 API Connection](Images/Deployment/04-authenticate-office365-api-connection.png)

## How To: Run the mobile client app for local execution and debugging on the iOS simulator##

>**Note:** Currently, only the iOS version of the mobile app is fully implemented.  The Android version of the app is a prototype and is not fully implemented. 

1. Use Visual Studio 2015 to open the **src/Cloud/ContosoInsurance-Cloud.sln** Visual Studio Solution file.
1.	Set up your Mac computer to act as a remote build machine.
	1.	Click the **Tools** menu and select **Options**.
	1.	Click **Xamarin**.
	1.	Click **iOS Settings**.
	1.	Click **Find Xamarin Mac Agent** and follow the wizard to connect your Mac.
	1.	Click **OK**.
1.  Configure the debugging target device according to the screenshot below.

	![](Images/VS-iOS-Deployment-Settings.png)
1.   Press **F5**.
1.   Observe the iOS Simulator start on the Mac Agent and load the Contoso Insurance mobile app.

## How To: Install the web application for local execution and debugging ##
 
1. Use Visual Studio 2015 to open the **src/Cloud/ContosoInsurance-Cloud.sln** Visual Studio Solution file.
1.   Right click the **ContosoInsurance.Web** project and select **Set as StartUp Project**.  
1.   Press **F5**.
1.   Observe the web browser open and load the Contoso Insurance claims search page.

## How To: View the custom events and metrics in Application Insights to monitor and debug the application

The sample logs status information and exceptions to Application Insights for every step in the process.  This starts the moment a user logs into the mobile application and continues until the very end when a claim is manually approved or rejected by the claims adjuster.

To view the custom events and metrics in Application Insights follow these steps.

1.  Open https://portal.azure.com in a web browser and log in.
1.  Click the Application Insights link in the left menu.
1.	Click contosoinsurance.
1.	Click Search.

	![](images/App-Insights-Search.png)

1.	Observe all of the Custom Events.

	![](images/App-Insights-Search-Results.png)

1.	Click a Custom Event in the list to see the metrics logged for the event.
	
	>**Note:**  You can refer to the Application Insights Logging Matrix in the [Azure Components document](/Azure Components.docx) to see all of the Custom Metrics logged for each Custom Event.  In the example below you can see this custom event was written by the HandleNewClaim Azure Function when it invoked the ClaimAutoApprover Azure Function.

	![](images/App-Insights-Custom-Event.png)

**Track an individual claim**

Each claim has a CorrelationId associated with it.  You can see this in the screenshot above.  The CorrelationId is used to track the claims from the moment they are received from the mobile app until the end of the process.  You can track the flow of a single claim through the Azure components and the web application by using the CorrelationId.  Here's how to do it:

1.  Copy the CorrelationId from a Custom Event.
2.  Click **Search**.

	![](images/App-Insights-Search.png)

2.	Paste the CorrelationId into the **Search textbox** and observe all the Custom Events associated with the CorrelationId.

	>**Note:**  This is an excellent way to debug errors in the system and is also especially helpful to determine how long a given step takes to execute.  This sample typically processes claims from the point where they are submitted in the mobile app to the point where they are ready for manual approval in 15 seconds when running the sample on the most basic App Services service level!
	
	![](images/App-Insights-Search-Results-CorrelationId.png) 

## How To: Customize the service ##

**Authentication**

The web and mobile client supports Microsoft Account Authentication. Follow this tutorial:

[How to configure your App Service application to use Microsoft Account login](https://azure.microsoft.com/en-us/documentation/articles/app-service-mobile-how-to-configure-microsoft-authentication/)

>**Note:** Ensure that the Action to take when request is not authenticated is set to Allow request (no action)  

## GitHub Authorization ##

**Generate Token**

1. Open https://github.com/settings/tokens in your web browser.
2. Sign into your GitHub account where you forked this repository.
3. Click **Generate Token**
4. Enter **ContosoInsurance** in the Token description text box
5. Select all the **check boxes**
6. Click **Generate token**
7. Copy the token

**Add Token To Azure**

1. Open https://resources.azure.com/providers/Microsoft.Web/sourcecontrols/GitHub in your web browser.
1. Paste the token into the **token parameter**.
	![](Images/Deployment/Authorize-GitHub.png)
1. Click **PUT**