# PAM for .NET

This library makes the PAM API available for .NET applications.

# License

The library is released under the [![MIT license](https://img.shields.io/github/license/mashape/apistatus.svg)](http://opensource.org/licenses/MIT).

# Example

The following example creates a PAM transaction and tries to authenticate a user.

```csharp
// Create the service
var pamService = new PamService(
    // The service options
    new OptionsWrapper<PamSessionConfiguration>(new PamSessionConfiguration()),
    // The PAM message handler
    new ConsoleMessageHandler());

// Create a PAM transaction
using (var pamTransaction = pamService.Start())
{
    // Change the user prompt
    pamTransaction.UserPrompt = "my customized user login prompt: ";

    // Authenticate
    pamTransaction.Authenticate();

    // Ensure that the user isn't locked (etc...)
    pamTransaction.AccountManagement();

    Console.WriteLine("User authentication for use {0} successful.", pamTransaction.UserName);
}
```

# Prerequisites

- .NET Core 3.0

# FAQ

## Why .NET Core 3.0 (AKA: WHY???)

TL/DR: It makes my life easier.

This library uses the native library loading mechanism of .NET Core 3.0. This is
required when you want to avoid using `libdl.so`, which is either only available
in a `-dev` package, has a different name, depending on your operating system or
distribution or other problems.

And no: netstandard2.1 doesn't support this API.

# Support the development

[![Patreon](https://img.shields.io/endpoint.svg?url=https:%2F%2Fshieldsio-patreon.herokuapp.com%2FFubarDevelopment&style=for-the-badge)](https://www.patreon.com/FubarDevelopment)
