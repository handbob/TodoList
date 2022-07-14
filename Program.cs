using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

public class Program {
    public static void Main(String[] args) {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        builder.Services.AddControllers();
        builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));

        WebApplication app = builder.Build();
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.Environment.IsDevelopment();
        app.MapControllers();
        app.Run();
    }
};
