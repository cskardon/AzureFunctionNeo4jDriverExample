# Example Azure Function using the Neo4j.Driver

Here we show an example of using the `Neo4j.Driver` ([nuget](https://www.nuget.org/packages/Neo4j.Driver)) to write to a database.

## What's in it?

First up is using an `ISession` to actually do the deed, disposing of it and tidying things up.
Secondly, we inject the `IDriver` as a Singleton to allow it to be shared for the lifetime of the functions.
It will create a new `AzureFunction` node in the database it connects to.

## How do I use it?

You will probably need to change a couple of things:

* `startup.cs` - you'll need to make sure the user/pass is correct
* `WriteToGraphFunction.cs` - just make sure you're happy with the writing code.

Other than that a compile should be all you need to do - then you just start it and call the endpoint:

`http://localhost:7071/api/WriteToGraphFunction`

_(Port might be different)_

