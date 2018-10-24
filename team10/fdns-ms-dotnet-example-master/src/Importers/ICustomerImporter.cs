#pragma warning disable 1591 // disables the warnings about missing Xml code comments
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
using Swashbuckle.AspNetCore.Annotations;
using Polly.CircuitBreaker;

namespace Foundation.Example.WebUI.Importers
{
    /// <summary>
    /// Interface for importing a list of customers into a database
    /// </summary>
    public interface ICustomerImporter
    {
        Task<ImportResult> ImportAsync(List<Customer> customers);
    }
}
#pragma warning restore 1591