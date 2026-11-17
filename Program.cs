using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using Saul_Ap1_P2.Components;
using Saul_Ap1_P2.Context;
using Saul_Ap1_P2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBlazorBootstrap();

builder.Services.AddDbContextFactory<Contexto>(o => 
    o.UseSqlite(
        builder.Configuration.GetConnectionString("ConStr")
    )
);

builder.Services.AddScoped<ProductosServices>();
builder.Services.AddScoped<VentasServices>();
builder.Services.AddScoped<ToastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
