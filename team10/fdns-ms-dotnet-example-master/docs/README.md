# How to modify the .NET Core Example microservice

This microservice is just an example meant for others to fork and modify. Modifying it is easy and we'll take you through several scenarios.

## Adding an API call to sum two numbers

If you want to add a new API that returns the sum of two integers, open `CustomerController.cs` in `/src/Controllers` and copy/paste the lines below:

```cs
[HttpGet("add/{first}/{second}")]
public async Task<ActionResult<int>> GetAdditionResult([FromRoute] int first, [FromRoute] int second)
{
    // This is what calls the FDNS Object microservice through the FDNS .NET Core SDK
    int result = first + second;
    return Ok(result);
}
```

You can of course substitute any logic of your choosing for the `int result = first + second;` line.

Note that the `[HttpGet("add/{first}/{second})]` line tells ASP.NET Core that the `first` and `second` variables in the route are mapped to the `first` and `second` variables in the method signature.

## Adding an API call to get a Json Book object from the FDNS Object service

Let's say we want to add a new API that calls the [Object Foundation Service](https://github.com/CDCGov/fdns-ms-object) to retrieve a `Book` object. Because our example only supports CRUD operations on `Customer` objects, we have to do some additional work to support `Book` as well.

> In real-world scenarios you should consider whether `Book` and `Customer` belong in separate microservices. This example shows you how to add support for `Book` while not interfering with the current `Customer` API, all within the same microservice.

### Step 1: Add a `Book` model

In the `/src/Models` folder, add `Book.cs` and copy/paste the following into it:

```cs
namespace Foundation.Example.WebUI.Models
{
    public sealed class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int Pages { get; set; }
    }
}
```

### Step 2: Add a `BookController`

Create a `BookController.cs` file in `src/Controllers` and copy/paste the following inside of it:

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Foundation.Example.WebUI.Models;
using Foundation.Sdk;
using Foundation.Sdk.Data;
using Polly.CircuitBreaker;

namespace Foundation.Example.WebUI.Controllers
{
    [Route("book/api/1.0")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private const string CIRCUIT_BREAKER_ERROR = "Book Service is inoperative, please try later on. (Business message due to Circuit-Breaker)";
        private readonly IObjectRepository<Book> _bookRepository;

        public BookController(IObjectRepository<Book> repository)
        {
            _bookRepository = repository;
        }

        // GET book/api/1.0/5
        [Produces("application/json")]
        [HttpGet("{id}")]
        [Authorize(Common.READ_AUTHORIZATION_NAME)]
        public async Task<ActionResult<Book>> Get([FromRoute] string id)
        {
            try
            {
                ServiceResult<Book> result = await _bookRepository.GetAsync(id);
                switch (result.Code)
                {
                    case HttpStatusCode.OK:
                        return Ok(result.Response);
                    default:
                        return StatusCode((int)result.Code, result.Message);
                }
            }
            catch (BrokenCircuitException)
            {
                return StatusCode(500, CIRCUIT_BREAKER_ERROR);
            }
        }

        // POST api/1.0/6
        [Produces("application/json")]
        [HttpPost("{id}")]
        [Authorize(Common.INSERT_AUTHORIZATION_NAME)]
        public async Task<ActionResult<Book>> Post([FromRoute] string id, [FromBody] Book payload)
        {
            // If the model (and in this case, the model is a Book object) has validation errors, let's catch it and return an HTTP 400
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                ServiceResult<Book> result = await _bookRepository.InsertAsync(id, payload);
                switch (result.Code)
                {
                    case HttpStatusCode.Created:
                        return CreatedAtAction(nameof(Get), new { id = id }, result.Response);
                    default:
                        return StatusCode((int)result.Code, result.Message);
                }
            }
            catch (BrokenCircuitException)
            {
                return StatusCode(500, CIRCUIT_BREAKER_ERROR);
            }
        }
    }
}
```

### Step 3: Add a Book repository to `Startup.cs`

Notice how `BookController` and `CustomerController` both have a constructor that takes a typed `IObjectRepository`. ASP.NET Core's dependency injection engine will determine what concrete class to inject into these constructors at runtime based on the services we create in `Startup.cs`. This example already has a `Startup.cs` configuration for what to do when it encounters an `IObjectRepository<Customer>` but it doesn't have one for `IObjectRepository<Book>`.

Open `Startup.cs` and find the following lines:

```cs
services.AddSingleton<IObjectRepository<Customer>>(provider => new HttpObjectRepository<Customer>(
    provider.GetService<IHttpClientFactory>(),
    provider.GetService<ILogger<HttpObjectRepository<Customer>>>(),
    applicationName));
```

What the above code does is tell ASP.NET Core that every time it encounters a constructor with an `IObjectRepository<Customer>` parameter, it should use a `HttpObjectRepository<Customer>` concrete class. It also specifies that the `HttpObjectRepository<Customer>` be constructed with three arguments:
1. An HTTP client factory,
1. A logger,
1. An application name

Note that arguments #1 and #2 come from elsewhere in Startup.cs.

We need to do the same thing for `Book` as has already been done for `Customer`. Creating the service is a straightforward copy/paste job with some minor edits:

```cs
services.AddSingleton<IObjectRepository<Book>>(provider => new HttpObjectRepository<Book>(
    provider.GetService<IHttpClientFactory>(),
    provider.GetService<ILogger<HttpObjectRepository<Book>>>(),
    "Book"));
```

### Step 4: Add a different HTTP client

We also need to set up a different HTTP client. The HTTP Client that will be created for `CustomerController` will use a `BaseAddress` of `bookstore/customer` on the FDNS Object service. We need `bookstore/book` for the `BookController`.

Find the following lines:

```cs
#region Create an HTTP client with configurable resiliency patterns
services.AddHttpClient(applicationName, client =>
{
    client.BaseAddress = new Uri($"{objectServiceUrl}/bookstore/customer/");
    client.DefaultRequestHeaders.Add("X-Correlation-App", applicationName);
})
.AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
{
    TimeSpan.FromMilliseconds(50),
    TimeSpan.FromMilliseconds(250),
    TimeSpan.FromSeconds(1)
}))
.AddPolicyHandler(GetCircuitBreakerPolicy());
#endregion
```

Below `.AddPolicyHandler(GetCircuitBreakerPolicy());`, copy/paste the following:

```cs
services.AddHttpClient("Book", client =>
{
    client.BaseAddress = new Uri($"{objectServiceUrl}/bookstore/book/");
    client.DefaultRequestHeaders.Add("X-Correlation-App", applicationName);
});
```

### Step 5: Run the application

Go to the **Debug** tab in Visual Studio Code and press the green **Debug** button. The app should start momentarily.

Visit the [Swagger page](https://localhost:5003/swagger) once it's running.

### Step 6: Conduct a test

Open the HTTP POST `/book/api/1.0/{id}` section in Swagger and select **Try it out**.

Enter an `id` of 1 and use the following payload:

```json
{
  "title": "C# in Depth",
  "author": "Jon Skeet",
  "isbn": "9781617291340",
  "pages": 616
}
```

Press **Execute**. Note the response is an HTTP 201 indicating the object was inserted into the FDNS Object microservice.

Now open the HTTP GET `/book/api/1.0/{id}` section in Swagger and select **Try it out**.

Enter an `id` of 1 and press **Execute**. The Json payload you just inserted is returned with an HTTP 200.

## Design notes

The .NET Core example microservice implements:

* Logging
* The circuit-breaker pattern
* Exponential backoffs
* OAuth2 with scope-based authorization (by default this is disabled)
* Auto-generated live API documentation via Swagger
* Cross-origin resource sharing
* Docker containerization via a `Dockerfile` and `Makefile`
* The FDNS .NET Core SDK
* FDNS Object microservice for CRUD operations

## Running locally inside a container
You will need to have the following software installed to run this microservice:

- [Docker](https://docs.docker.com/install/)
- [Docker Compose](https://docs.docker.com/compose/install/)
- **Windows Users**: This project uses `Make`. Please use [Cygwin](http://www.cygwin.com/) or the [Windows Subsystem for Linux](https://docs.microsoft.com/en-us/windows/wsl/install-win10) for running the commands in this README.

1. Open Bash or a Bash-like terminal
1. Build the container image by running `make docker-build`
1. Start the container by running `make docker-start`
1. Open a web browser and point to [http://127.0.0.1:9091/](http://127.0.0.1:9091/)

## Debugging using Visual Studio Code

You will need to have the following software installed to debug this microservice:

- [Visual Studio Code](https://code.visualstudio.com/)
- [C# Extension for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)
- [.NET Core SDK 2.1](https://www.microsoft.com/net/download)

1. Open Visual Studio Code
1. Select **File** > **OpenFolder** and select `fdns-ms-dotnet-example/src`
1. Open Visual Studio Code's **Debug** pane (shortcut key: `CTRL`+`SHIFT`+`D`)
1. Press the green arrow at the top of the **Debug** pane
1. Open a web browser and point to https://localhost:5003

## Debugging unit tests using Visual Studio Code

1. Open Visual Studio Code
1. Select **File** > **OpenFolder** and select `fdns-ms-dotnet-example/tests`
1. Open Visual Studio Code's **Explorer** pane (shortcut key: `CTRL`+`SHIFT`+`E`)
1. Open a Test classfile from the file list
1. Select **Debug test** at the top of any of the test methods or **Debug all tests** from the top of the class definition

## Running from the command line

To run the service from the command line:

1. Open Bash or a Bash-like terminal
1. `cd` to the `fdns-ms-dotnet-example/src` folder
1. Execute `dotnet restore`
1. Execute `dotnet build`
1. Execute `dotnet run`
1. Open a web browser and point to https://localhost:5003

To run tests from the command line:

1. Open Bash or a Bash-like terminal
1. `cd` to the `fdns-ms-dotnet-example/tests` folder
1. Execute `dotnet test`

## Experimenting with API operations

We use Swagger to automatically generate a live design document based on the underlying C# source code and XML code comments. Swagger allows developers to experiment with and test the API on a running microservice. It also shows you exactly what operations this service exposes to developers. To access the Swagger documentation, add `/swagger` to the end of the service's URL in your web browser, e.g. `https://localhost:5003/swagger`.

## Environment variable configuration

* `APP_NAME`: The name of this microservice
* `EXAMPLEAPP_FLUENTD_HOST`: [Fluentd](https://www.fluentd.org/) hostname
* `EXAMPLEAPP_FLUENTD_PORT`: [Fluentd](https://www.fluentd.org/) port
* `OBJECT_URL`: The URL to the [FDNS Object microservice](https://github.com/CDCGov/fdns-ms-object)
* `STORAGE_URL`: The URL to the [FDNS Storage microservice](https://github.com/CDCGov/fdns-ms-storage)
* `INDEXING_URL`: The URL to the [FDNS Indexing microservice](https://github.com/CDCGov/fdns-ms-indexing)
* `RULES_URL`: The URL to the [FDNS Rules microservice](https://github.com/CDCGov/fdns-ms-rules)

The following environment variables can be used to configure this microservice to use your OAuth2 provider:

* `OAUTH2_ACCESS_TOKEN_URI`: This is the introspection URL of your provider, ex: `https://hydra:4444/oauth2/introspect`
* `OAUTH2_PROTECTED_URIS`: This is a path for which routes are to be restricted, ex: `/api/1.0/**`
* `OAUTH2_CLIENT_ID`: This is your OAuth 2 client id with the provider
* `OAUTH2_CLIENT_SECRET`: This is your OAuth 2 client secret with the provider
* `SSL_VERIFYING_DISABLE`: This is an option to disable SSL verification, you can disable this when testing locally but this should be set to `false` for all production systems

For more information on using OAuth2 with this microservice, see **Authorization and security** at the end of this document.

## Authorization and Security

Coming soon