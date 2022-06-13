// See https://aka.ms/new-console-template for more information

using MaoXianCat.Files;
using Microsoft.Extensions.DependencyInjection;

IServiceCollection serviceCollection=new ServiceCollection();
//serviceCollection,confi
serviceCollection.AddScoped<IMoveFile, MoveFile>();

Console.WriteLine("Hello, World!");