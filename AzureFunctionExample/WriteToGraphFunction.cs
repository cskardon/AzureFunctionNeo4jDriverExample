namespace AzureFunctionExample
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Neo4j.Driver.V1;
    using ILogger = Microsoft.Extensions.Logging.ILogger;

    /// <summary>
    /// Just writes a new 'AzureFunction' node to a Neo4j instance.
    /// </summary>
    public class WriteToGraphFunction
    {
        /// <summary>Create a new instance of the <see cref="WriteToGraphFunction"/>.</summary>
        /// <param name="driver">This is injected by the <see cref="Startup"/> class.</param>
        public WriteToGraphFunction(IDriver driver)
        {
            Driver = driver;
        }

        /// <summary>
        /// The <see cref="IDriver"/> instance to use within the function. This is a singleton, and each use of the DB should use an <see cref="Neo4j.Driver.V1.ISession"/> object.
        /// </summary>
        private IDriver Driver { get; }

        /// <summary>
        /// Simply writes a new `AzureFunction` node to the DB.
        /// </summary>
        /// <param name="req">The <see cref="HttpRequest"/> that called this.</param>
        /// <param name="log">The <see cref="ILogger"/> instance to log to Azure.</param>
        /// <returns>OK (200) if it succeeds writing to the DB, Bad Request (400) if it fails. The reason for failure is logged to the screen.</returns>
        [FunctionName("WriteToGraphFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("Adding data!");
                using (var session = Driver.Session())
                {
                    using (var tx = await session.BeginTransactionAsync())
                    {
                        var resp = await tx.RunAsync("CREATE (az:AzureFunction {timestamp: $timestampParam})", new { timestampParam = DateTime.UtcNow.Ticks });
                        await resp.ConsumeAsync();
                        await tx.CommitAsync();
                    }
                }

                return new OkObjectResult("Data entered into Neo4j");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception thrown attempting to hit up Neo4j{Environment.NewLine}{ex}");
            }

            return new BadRequestObjectResult("The function failed :(");
        }
    }
}