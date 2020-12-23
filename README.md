### ASP.NET-IntegrationTests-Issue

WebApplicationFactory does not properly configure the services, as although the In Memory SQL provider is set, the tests are still usings the one from the Startup class, in this case the SQL Provider.


