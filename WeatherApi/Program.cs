using StackExchange.Redis;

namespace WeatherApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var sentinelConnection = new ConfigurationOptions
            {
                EndPoints = { "redis:26379", "redis2:26379", "redis3:26379" },
                ServiceName = "mymaster",
                TieBreaker = "",
                CommandMap = CommandMap.Default,
                Ssl = false,
                AllowAdmin = true,
                ConnectTimeout = 5000,
                SyncTimeout = 5000,
                AbortOnConnectFail = false,
            };

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = "WeatherApiCache";
                options.ConfigurationOptions = sentinelConnection;
            });

            //add redis sentinel service


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}