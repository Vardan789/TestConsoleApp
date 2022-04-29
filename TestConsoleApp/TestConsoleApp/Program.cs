using System.Net.Mime;
using System.Text;

Console.WriteLine("Start Generating Load");
Console.WriteLine(await ParallelHttpRequestsInBatchesWithSemaphoreSlim());
await Task.Delay(100000);
Console.WriteLine("Completed.");

async Task<HttpResponseMessage[]> ParallelHttpRequestsInBatchesWithSemaphoreSlim()
{
    var tasks = new List<Task<HttpResponseMessage>>();
    const int numberOfRequests = 7000;
    const int maxParallelRequests = 7000;
    var semaphoreSlim = new SemaphoreSlim(maxParallelRequests, maxParallelRequests);

    for (var i = 0; i < numberOfRequests; ++i)
    {
        tasks.Add(MakeRequestWithSemaphoreSlimAsync(new Uri("http://atomconstruct.com/PerformanceTest/bet"),
            semaphoreSlim));
    }

    return await Task.WhenAll(tasks.ToArray());
}

async Task<HttpResponseMessage> MakeRequestWithSemaphoreSlimAsync(Uri uri, SemaphoreSlim semaphoreSlim)
{
    try
    {
        const string body =
            @"{""user"": ""8303"",""transaction_uuid"": ""fugiat irure"",""token"": ""6494a591-6dfb-cba6-3ab4-f5c524626f03"",""round_closed"": true,""game_code"": ""evo_liveextremetexasholdem"",""currency"": ""USD"",""amount"": 1000 } ";
        var data = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json);
        await semaphoreSlim.WaitAsync();

        using var httpClient = new HttpClient();

        return await httpClient.PostAsync(uri, data);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        return new HttpResponseMessage();
    }
    finally
    {
        semaphoreSlim.Release();
    }
}

//
// async System.Threading.Tasks.Task RunTest()
// {
//     const int maxIterations = 500;
//     const int maxParallelRequests = 1;
//     const int delay = 100;
//
//     const string uri = "http://atomconstruct.com/PerformanceTest/bet";
//
//     using var httpClient = new HttpClient();
//     //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.xxxxxx");
//     const string body = @"{""user"": ""8303"",""transaction_uuid"": ""fugiat irure"",""token"": ""6494a591-6dfb-cba6-3ab4-f5c524626f03"",""round_closed"": true,""game_code"": ""evo_liveextremetexasholdem"",""currency"": ""USD"",""amount"": 1000 } ";
//     var data = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json);
//     var tasks = new List<Task<HttpResponseMessage>>();
//     for (var step = 1; step < maxIterations; step++)
//     {
//         Console.WriteLine($"Started iteration: {step}");
//        
//
//         for (var i = 0; i < maxParallelRequests; i++)
//         {
//             tasks.Add(httpClient.PostAsync(uri, data));
//         }
//
//         // Run all  tasks in parallel
//        
//     }
//     var result = await Task.WhenAll(tasks);
//     Console.WriteLine($"Completed Iteration:");
//
//     // Some delay before new iteration
//     await Task.Delay(delay);
// }