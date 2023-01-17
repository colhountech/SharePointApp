
# The Basics

Configuring an application in Azure AD

Below steps will help you create and configure an application in Azure Active Directory:

* Go to Azure AD Portal via https://aad.portal.azure.com
* Select Azure Active Directory and on App registrations in the left navigation
* Select New registration
* Enter a name for your application and select Register
* Go to API permissions to grant permissions to your application, select Add a permission, choose SharePoint, Delegated permissions and select for example AllSites.Manage
* Select Grant admin consent to consent the application's requested permissions
	- See Below
* Select Authentication in the left navigation
* Change Allow public client flows from No to Yes
* Select Overview and copy the application ID to the clipboard (you'll need it later on)


# Grant admin consent in App registrations

When granting tenant-wide admin consent using either method described above, a window opens from the Azure portal to prompt for tenant-wide admin consent. 
If you know the client ID (also known as the application ID) of the application, you can build the same URL to grant tenant-wide admin consent.

The tenant-wide admin consent URL follows the following format:

```

https://login.microsoftonline.com/{tenant-id}/adminconsent?client_id={client-id}

```
where:

{client-id} is the application's client ID (also known as app ID).

{tenant-id} is your organization's tenant ID or any verified domain name.

As always, carefully review the permissions an application requests before granting consent.


# Next Step

remove clientID and TenantID from code and place in usersecrets, or azure value or somewhere safe

Then check in your code

I find the quickest way to navigate to Lists, is to :
- create a Teams channel, and 
- create a List from the List AddOns 
- add this List to the Teams Channel Tab

# References

* https://learn.microsoft.com/en-us/sharepoint/dev/sp-add-ins/using-csom-for-dotnet-standard?source=recommendations
* https://learn.microsoft.com/en-us/azure/active-directory/manage-apps/grant-admin-consent?pivots=portal#construct-the-url-for-granting-tenant-wide-admin-consent
