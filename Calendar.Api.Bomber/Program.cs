using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;
using NBomber.Plugins.Network.Ping;

var step = Step.Create("book an appointment",
                       clientFactory: HttpClientFactory.Create(),
                       execute: context =>
                       {
                           var request = Http.CreateRequest("POST", "http://localhost:5043/appointments")
                                                         .WithHeader("Content-Type", "application/json")
                                                         .WithBody(new StringContent("{}"));

                           return Http.Send(request, context);
                       });


var scenario = ScenarioBuilder
    .CreateScenario("simple_http", step)
    .WithWarmUpDuration(TimeSpan.FromSeconds(5))
    .WithLoadSimulations(
        Simulation.InjectPerSec(rate: 100, during: TimeSpan.FromSeconds(30))
    );

// creates ping plugin that brings additional reporting data
var pingPluginConfig = PingPluginConfig.CreateDefault(new string[] { });
var pingPlugin = new PingPlugin(pingPluginConfig);

NBomberRunner
    .RegisterScenarios(scenario)
    .WithWorkerPlugins(pingPlugin)
    .Run();