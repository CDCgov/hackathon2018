# FDNS .NET Core 2.1 Example Microservice
This is a repository containing an example ASP.NET Core 2.1 microservice. It is intended for C# / .NET Core developers to fork and modify. It includes implementations for:

* Dependency injection
* Logging
* [FDNS .NET Core SDK](https://gitlab.com/eknudsen/fdns-dotnet-sdk)
* Circuit breakers
* OAuth2 scope-based authorization
* [FDNS Object microservice](https://github.com/CDCGov/fdns-ms-object) for all database CRUD operations
* [FDNS Storage microservice](https://github.com/CDCGov/fdns-ms-storage) for storing CSV files used for bulk record imports
* Auto-generated, live API documentation via Swagger pages and C# XML code comments
* Cross-origin resource sharing
* Exponential backoff for HTTP requests
* Easy containerization via a `Dockerfile` and `Makefile`
* Two-stage Docker builds
* Health monitoring at `/api/1.0/health`

## Modifying this microservice
See [USAGE.md](/docs/USAGE.md) for instructions on how to debug and containerize this microservice in your local environment.

See [the docs folder](/docs/README.md) for an example of how to modify this microservice.

## License
The repository utilizes code licensed under the terms of the Apache Software License and therefore is licensed under ASL v2 or later.

This source code in this repository is free: you can redistribute it and/or modify it under
the terms of the Apache Software License version 2, or (at your option) any later version.

This source code in this repository is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE. See the Apache Software License for more details.

You should have received a copy of the Apache Software License along with this program. If not, see https://www.apache.org/licenses/LICENSE-2.0.html.

The source code forked from other open source projects will inherit its license.