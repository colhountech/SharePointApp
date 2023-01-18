# Get SharePoint List

This builds off SharePointApp, but moves the AuthenicationManager to a separate class for resuse

## Notes:

- The List must already exist

If the List name has spaces, don't encode in %20, insted just add to appsettings with spaces and it will be encoded directly. For Example:

- Don''t use `A%20Test%20List`
- Do use `A Test List`