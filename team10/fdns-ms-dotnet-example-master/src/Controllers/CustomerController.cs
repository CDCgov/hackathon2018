using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Foundation.Example.WebUI.Models;
using Foundation.Example.WebUI.Importers;
using Foundation.Example.WebUI.Converters;
using Foundation.Sdk;
using Foundation.Sdk.Data;
using Foundation.Sdk.Services;
using Swashbuckle.AspNetCore.Annotations;
using Polly.CircuitBreaker;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Foundation.Example.WebUI.Controllers
{
    /// <summary>
    /// Customer controller
    /// </summary>
    [Route("api/1.0")]
    [ApiController]
    public sealed class CustomerController : ControllerBase
    {
        #region Private Members
        private const string CIRCUIT_BREAKER_ERROR = "Customer Service is inoperative, please try later on. (Business message due to Circuit-Breaker)";
        private readonly IObjectRepository<Customer> _customerRepository;
        private readonly ICustomerImporter _customerImporter;
        private readonly IStorageRepository _storageRepository;
        private readonly IIndexingService _indexingService;
        private readonly IRulesService _rulesService;
        #endregion // Private Members

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="customerRepository">FDNS Object Customer repository</param>
        /// <param name="storageRepository">FDNS Storage repository</param>
        /// <param name="indexingService">FDNS Indexing service</param>
        /// <param name="rulesService">FDNS Rules service</param>
        /// <param name="customerImporter">Customer importer</param>
        public CustomerController(IObjectRepository<Customer> customerRepository, IStorageRepository storageRepository, IIndexingService indexingService, IRulesService rulesService, ICustomerImporter customerImporter)
        {
            _customerRepository = customerRepository;
            _storageRepository = storageRepository;
            _indexingService = indexingService;
            _rulesService = rulesService;
            _customerImporter = customerImporter;
        }

        // GET api/1.0/5
        /// <summary>
        /// Gets the Customer with a specified ID
        /// </summary>
        /// <param name="id">The ID of the customer</param>
        /// <returns>Customer</returns>
        [Produces("application/json")]
        [HttpGet("{id}")]
        [SwaggerResponse(200, "If the customer was found successfully", typeof(Customer))]
        [SwaggerResponse(400, "If there was a client error handling this request")]
        [SwaggerResponse(401, "If the HTTP header lacks a valid OAuth2 token")]
        [SwaggerResponse(403, "If the HTTP header has a valid OAuth2 token but lacks the appropriate scope to use this route")]
        [SwaggerResponse(404, "If the customer with this ID was not found")]
        [Authorize(Common.READ_AUTHORIZATION_NAME)]
        public async Task<ActionResult<Customer>> Get([FromRoute] string id)
        {
            try
            {
                ServiceResult<Customer> result = await _customerRepository.GetAsync(id); // This is what calls the FDNS Object microservice through the FDNS .NET Core SDK
                return HandleObjectResult<Customer>(result); // Determine what to do based on the HTTP response code
            }
            catch (BrokenCircuitException)
            {
                return StatusCode(500, CIRCUIT_BREAKER_ERROR);
            }
        }

        // POST api/1.0/6
        /// <summary>
        /// Inserts a Customer with a specified ID
        /// </summary>
        /// <remarks>
        /// Sample request to insert a new customer with an id of 6:
        ///
        ///     POST /api/1.0/6
        ///     {
        ///         "id": "6",
        ///         "firstName": "Jane",
        ///         "lastName": "Doe",
        ///         "age": 46,
        ///         "streetAddress": "string",
        ///         "dateOfBirth": "2018-09-29T02:16:02.023Z"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">The ID of the customer</param>
        /// <param name="payload">The Json representation of the customer</param>
        /// <returns>Customer that was inserted</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPost("{id}")]
        [SwaggerResponse(201, "Returns the customer that was just created", typeof(Customer))]
        [SwaggerResponse(400, "If the route parameters or json payload contain invalid data")]
        [SwaggerResponse(401, "If the HTTP header lacks a valid OAuth2 token")]
        [SwaggerResponse(403, "If the HTTP header has a valid OAuth2 token but lacks the appropriate scope to use this route")]
        [SwaggerResponse(406, "If the content type is invalid")]
        [SwaggerResponse(413, "If the Json payload is too large")]
        [SwaggerResponse(415, "If the media type is invalid")]
        [Authorize(Common.INSERT_AUTHORIZATION_NAME)]
        public async Task<ActionResult<Customer>> Post([FromRoute] string id, [FromBody] Customer payload)
        {
            try
            {
                ServiceResult<Customer> result = await _customerRepository.InsertAsync(id, payload);
                return HandleObjectResult<Customer>(result, id);
            }
            catch (BrokenCircuitException)
            {
                return StatusCode(500, CIRCUIT_BREAKER_ERROR);
            }
        }

        // PUT api/1.0/6
        /// <summary>
        /// Replaces a Customer with a specified ID
        /// </summary>
        /// <remarks>
        /// Sample request to replace a new customer with an id of 6:
        ///
        ///     POST /api/1.0/6
        ///     {
        ///         "id": "6",
        ///         "firstName": "Janet",
        ///         "lastName": "Doe",
        ///         "age": 49,
        ///         "streetAddress": "string",
        ///         "dateOfBirth": "2018-09-29T02:16:02.023Z"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">The ID of the customer</param>
        /// <param name="payload">The Json representation of the customer</param>
        /// <returns>Customer that was replaced</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPut("{id}")]
        [SwaggerResponse(200, "Returns the updated customer", typeof(Customer))]
        [SwaggerResponse(400, "If the route parameters or json payload contain invalid data")]
        [SwaggerResponse(401, "If the HTTP header lacks a valid OAuth2 token")]
        [SwaggerResponse(403, "If the HTTP header has a valid OAuth2 token but lacks the appropriate scope to use this route")]
        [SwaggerResponse(404, "If the customer to update cannot be found")]
        [SwaggerResponse(406, "If the content type is invalid")]
        [SwaggerResponse(413, "If the Json payload is too large")]
        [SwaggerResponse(415, "If the media type is invalid")]
        [Authorize(Common.UPDATE_AUTHORIZATION_NAME)]
        public async Task<ActionResult<Customer>> Put([FromRoute] string id, [FromBody] Customer payload)
        {
            try
            {
                ServiceResult<Customer> result = await _customerRepository.ReplaceAsync(id, payload);
                return HandleObjectResult<Customer>(result);
            }
            catch (BrokenCircuitException)
            {
                return StatusCode(500, CIRCUIT_BREAKER_ERROR);
            }
        }

        // DELETE api/1.0/5
        /// <summary>
        /// Deletes the Customer with a specified ID
        /// </summary>
        /// <param name="id">The ID of the customer</param>
        /// <returns>true if the deletion was a success; false if it wasn't</returns>
        [HttpDelete("{id}")]
        [SwaggerResponse(200, "If the customer was deleted successfully", typeof(bool))]
        [SwaggerResponse(400, "If the route parameters or json payload contain invalid data")]
        [SwaggerResponse(401, "If the HTTP header lacks a valid OAuth2 token")]
        [SwaggerResponse(403, "If the HTTP header has a valid OAuth2 token but lacks the appropriate scope to use this route")]
        [SwaggerResponse(404, "If the customer to delete cannot be found")]
        [Authorize(Common.DELETE_AUTHORIZATION_NAME)]
        public async Task<ActionResult<DeleteResult>> Delete(string id)
        {
            try
            {
                ServiceResult<DeleteResult> result = await _customerRepository.DeleteAsync(id);
                return HandleObjectResult<DeleteResult>(result);
            }
            catch (BrokenCircuitException)
            {
                return StatusCode(500, CIRCUIT_BREAKER_ERROR);
            }
        }

        // POST api/1.0/find
        /// <summary>
        /// Finds matching Customers based on the MongoDB find syntax
        /// </summary>
        /// <remarks>
        /// Sample request to find one or more customers with an age between 25 and 35 and a name of either "John" or "Jane":
        ///
        ///     POST /api/1.0/find
        ///     {
        ///         firstName:
        ///         {
        ///             $in: [ "Jane", "John" ]
        ///         },
        ///         age: { $gte: 25, $lte: 35 }
        ///     }
        ///
        /// </remarks>
        /// <param name="findCriteria">The MongoDB find syntax to use</param>
        /// <returns>Array of Customers that match the provided regular expression and inputs</returns>
        [Consumes("text/plain")]
        [Produces("application/json")]
        [HttpPost("find")]
        [SwaggerResponse(200, "Returns the objects that match the inputs to the find operation", typeof(List<Customer>))]
        [SwaggerResponse(400, "If the find expression contains any invalid inputs")]
        [SwaggerResponse(401, "If the HTTP header lacks a valid OAuth2 token")]
        [SwaggerResponse(403, "If the HTTP header has a valid OAuth2 token but lacks the appropriate scope to use this route")]
        [SwaggerResponse(406, "If the find expression is submitted as anything other than text/plain")]
        [SwaggerResponse(413, "If the find expression is too large")]
        [SwaggerResponse(415, "If the media type is invalid")]
        [Authorize(Common.READ_AUTHORIZATION_NAME)]
        public async Task<ActionResult<SearchResults<Customer>>> Find([FromBody] string findCriteria)
        {
            try
            {
                ServiceResult<SearchResults<Customer>> result = await _customerRepository.FindAsync(0, 10, "name", findCriteria, false);
                return HandleObjectResult<SearchResults<Customer>>(result);
            }
            catch (BrokenCircuitException)
            {
                return StatusCode(500, CIRCUIT_BREAKER_ERROR);
            }
        }

        // POST api/1.0/import
        /// <summary>
        /// Bulk imports customers from a CSV file into the FDNS Object and Storage services
        /// </summary>
        /// <remarks>
        /// Sample request to find one or more customers with an age between 25 and 35 and a name of either "John" or "Jane":
        ///
        ///     POST /api/1.0/import
        ///     "1","John","Doe",24,"1234 Main St"
        ///     "2","Jane","Doe",25,"2345 Main St"
        ///     "3","Mary","Sue",45,"3456 Main St"
        ///     "4","Drew","Lee",56,"4567 Main St"
        ///
        /// </remarks>
        /// <returns>Customer IDs</returns>
        [Produces("application/json")]
        [Consumes("text/plain")]
        [HttpPost("import")]
        [SwaggerResponse(200, "Returns the customer IDs that were inserted, updated, skipped, and includes metadata about where the CSV file is stored in S3", typeof(ImportResult))]
        [SwaggerResponse(400, "If the route parameters or payload contain invalid data")]
        [SwaggerResponse(401, "If the HTTP header lacks a valid OAuth2 token")]
        [SwaggerResponse(403, "If the HTTP header has a valid OAuth2 token but lacks the appropriate scope to use this route")]
        [SwaggerResponse(406, "If the content type is invalid")]
        [SwaggerResponse(413, "If the payload is too large")]
        [SwaggerResponse(415, "If the media type is invalid")]
        [Authorize(Common.INSERT_AUTHORIZATION_NAME)]
        public async Task<ActionResult<ImportResult>> BulkImport([FromBody] string payload)
        {
            /* Stuff the CSV file, exactly as it was received, into the FDNS Storage service.
             * The FDNS Storage service is the immutable layer of the FDNS data lake and represents the
             * 'source of truth'. */
            var importId = System.Guid.NewGuid().ToString();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(payload);
            ServiceResult<StorageMetadata> storageResult = null;
            try
            {
                var createDrawerResult = await _storageRepository.CreateDrawerAsync();

                storageResult = await _storageRepository.CreateNodeAsync(importId, $"csv-import-{importId}", data);
                if (storageResult == null)
                {
                    return StatusCode(400, "Unknown error");
                }
                if (storageResult.IsSuccess == false)
                {
                    return StatusCode((int)storageResult.Code, storageResult.Message);
                }
            }
            catch (BrokenCircuitException)
            {
                return StatusCode(500, CIRCUIT_BREAKER_ERROR);
            }

            // Now import each Customer into FDNS Object
            var customers = new Converters.CsvToCustomersConverter().Convert(payload);
            ImportResult importResult = await _customerImporter.ImportAsync(customers);
            importResult.StorageMetadata = storageResult.Response;
            return Ok(importResult);
        }

        // POST api/1.0/reset-storage
        /// <summary>
        /// Removes all storage nodes from the S3 drawer
        /// </summary>
        [Produces("application/json")]
        [HttpPost("reset-storage")]
        [SwaggerResponse(200, "All nodes are removed from the S3 drawer")]
        [SwaggerResponse(401, "If the HTTP header lacks a valid OAuth2 token")]
        [SwaggerResponse(403, "If the HTTP header has a valid OAuth2 token but lacks the appropriate scope to use this route")]
        [Authorize(Common.DELETE_AUTHORIZATION_NAME)]
        public async Task<ActionResult> DeleteAllNodes()
        {
            var getDrawerResult = await _storageRepository.GetDrawerAsync();

            if (getDrawerResult.IsSuccess)
            {
                var listAllNodesResult = await _storageRepository.GetAllNodesAsync();

                foreach (var node in listAllNodesResult.Response)
                {
                    var id = node.Id.ToString();
                    var deleteNodeResult = await _storageRepository.DeleteNodeAsync(id);
                }

                var deleteDrawerResult = await _storageRepository.DeleteDrawerAsync();
            }

            return Ok();
        }

        // POST api/1.0/validate
        /// <summary>
        /// Validates a customer
        /// </summary>
        [Produces("application/json")]
        [HttpPost("validate")]
        [SwaggerResponse(200, "Customer validation check completes")]
        [SwaggerResponse(400, "If the route parameters or payload contain invalid data")]
        [SwaggerResponse(401, "If the HTTP header lacks a valid OAuth2 token")]
        [SwaggerResponse(403, "If the HTTP header has a valid OAuth2 token but lacks the appropriate scope to use this route")]
        [Authorize(Common.READ_AUTHORIZATION_NAME)]
        public async Task<ActionResult> Validate([FromBody] Customer payload, [FromQuery] bool explain)
        {
            // upsert the config in case it's not there; remove this for production, though, likely would handle this differently
            var upsertResult = await _rulesService.UpsertProfileAsync("bookstore-customer", "{ \"$gte\": { \"$.age\": 18 } }");

            switch (upsertResult.Code)
            {
                case HttpStatusCode.OK:
                    break;
                case HttpStatusCode.Created:
                    break;
                default:
                    return StatusCode((int)upsertResult.Code, upsertResult.Message);
            }

            string customerString = Newtonsoft.Json.JsonConvert.SerializeObject(payload, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            var validationResult = await _rulesService.ValidateAsync("bookstore-customer", customerString, explain);

            switch (validationResult.Code)
            {
                case HttpStatusCode.OK:
                    return Content(validationResult.Response, "application/json");
                default:
                    return StatusCode((int)validationResult.Code, validationResult.Message);
            }
        }

        private ActionResult<T> HandleObjectResult<T>(ServiceResult<T> result, string id = "")
        {
            switch (result.Code)
            {
                case HttpStatusCode.OK:
                    return Ok(result.Response);
                 case HttpStatusCode.Created:
                    return CreatedAtAction(nameof(Get), new { id = id }, result.Response);
                default:
                    return StatusCode((int)result.Code, result.Message);
            }
        }
    }
}
