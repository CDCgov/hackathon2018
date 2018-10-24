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
    /// Class for importing a list of customers into an Object repository
    /// </summary>
    public sealed class HttpCustomerImporter : ICustomerImporter
    {
        private readonly IObjectRepository<Customer> _customerRepository;

        public HttpCustomerImporter(IObjectRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<ImportResult> ImportAsync(List<Customer> customers)
        {
            var importedIds = new Dictionary<string, string>();
            var skippedIds = new Dictionary<string, string>();

            var distinctResult = await _customerRepository.GetDistinctAsync("id", "{}");
            var ids = distinctResult.Response;

            foreach (var customer in customers)
            {
                ServiceResult<Customer> result = null;
                try
                {
                    if (ids.Contains(customer.Id))
                    {
                        result = await _customerRepository.ReplaceAsync(customer.Id, customer);
                    }
                    else
                    {
                        result = await _customerRepository.InsertAsync(customer.Id, customer);
                    }

                    if (result.IsSuccess)
                    {
                        importedIds.Add(customer.Id, result.Code == HttpStatusCode.Created ? "inserted" : "updated");
                    }
                    else
                    {
                        skippedIds.Add(customer.Id, result.Message);
                    }
                }
                catch (Exception ex)
                {
                    skippedIds.Add(customer.Id, ex.Message);
                }
            }

            var importResult = new ImportResult()
            {
                SkippedIds = skippedIds,
                ImportedIds = importedIds
            };

            return importResult;
        }
    }
}

#pragma warning restore 1591