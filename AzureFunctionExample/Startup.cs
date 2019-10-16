using AzureFunctionExample;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

// This lets AzureFunctions know that this is the Startup object.
[assembly: FunctionsStartup(typeof(Startup))]

namespace AzureFunctionExample
{
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;
    using Neo4j.Driver.V1;

    /// <summary>
    /// Provides the services needed by the functions.
    /// </summary>
    public class Startup : FunctionsStartup
    {

        /// <summary>
        /// Adds the services needed throughout the Functions.
        /// </summary>
        /// <remarks>
        /// Creates the following<br/>
        /// <list type="bullet">
        ///     <item><see cref="IDriver"/> (Neo4j)</item>
        /// </list>
        /// </remarks>
        /// <param name="builder">The <see cref="IFunctionsHostBuilder"/> instance the services are added to.</param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton(d => GraphDatabase.Driver("bolt://127.0.0.1:7687", AuthTokens.Basic("neo4j", "neo")));
        }
    }
}