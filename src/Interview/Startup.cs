using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Interview
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IEnumerable<PersonRepresentation>>(serviceProvider =>
            {
                using (var stream = File.OpenRead("data.json"))
                {
                    var data = new StreamReader(stream).ReadToEnd();
                    return JsonConvert.DeserializeObject<IEnumerable<PersonRepresentation>>(data);
                }
            });

            services.AddSingleton<object>(serviceProvider =>
            {
                var people = serviceProvider.GetService<IEnumerable<PersonRepresentation>>();
                var personMatchingId = people.FirstOrDefault(x => x.Id == "5aabbca3e58dc67745d720b1");

                return new
                {
                    Over50 = people.Count(x => x.Age > 50),
                    LastRegisteredActivePerson = people
                        .Where(x => x.Registered.HasValue && x.IsActive)
                        .OrderByDescending(x => x.Registered)
                        .FirstOrDefault(),
                    FavoriteFruitCounts = people
                        .GroupBy(x => x.FavoriteFruit).Select(x => new
                        {
                            Fruit = x.Key,
                            Count = x.Count()
                        }),
                    MostCommonEyeColor = people
                        .GroupBy(x => x.EyeColor)
                        .OrderByDescending(x => x.Count())
                        .FirstOrDefault()
                        .Key,
                    TotalBalance = people
                        .Select(x => double.Parse(x.Balance.Replace("$", string.Empty)))
                        .Sum()
                        .ToString("$#,##0.00"),
                    FullNameUsingSpecificId = personMatchingId == null
                        ? "<not found>"
                        : $"{personMatchingId.Name.Last}, {personMatchingId.Name.First}"
                };
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                var response = app.ApplicationServices.GetService<object>();

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            });
        }
    }
}
