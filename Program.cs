using Confluent.Kafka;
using Newtonsoft.Json;

var config = new ConsumerConfig
{
    GroupId = "weather-consumer-group",
    BootstrapServers = "localhost:9092",
    AutoOffsetReset = AutoOffsetReset.Earliest
};
using var consumer = new ConsumerBuilder<Null, string>(config).Build();

consumer.Subscribe("weather-topic"); // burada Producerdan gelen topic olacak
CancellationTokenSource token = new();
try
{
    while (true)
    {
        var response = consumer.Consume(token.Token);
        if (response.Message != null)
        {
            var Weather = JsonConvert.DeserializeObject<Weather>(response.Message.Value);
            Console.WriteLine($"State:{Weather.State}," + $"Temp:{Weather.Temperature}F");
        }
    }
}
catch (Exception ex)
{
    throw;
}
public record Weather(string State, int Temperature);