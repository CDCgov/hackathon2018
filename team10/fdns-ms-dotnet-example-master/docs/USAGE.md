# How to use this Example microservice

## Running locally inside a container
You will need to have the following software installed to run this microservice:

- [Docker](https://docs.docker.com/install/)
- [Docker Compose](https://docs.docker.com/compose/install/)
- **Windows Users**: This project uses `Make`, please use [Cygwin](http://www.cygwin.com/) or the [Windows Subsystem for Linux](https://docs.microsoft.com/en-us/windows/wsl/install-win10) for running commands in this README

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

__Scopes__: This application uses the following scope: `fdns.example.*`

For more information on using OAuth2 with this microservice, see **Authorization and security** at the end of this document.

## Authorization and Security

Coming soon