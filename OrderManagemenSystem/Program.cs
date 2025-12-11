using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrderManagemenSystem.APIMiddleware;
using OrderManagemenSystem.Business.Contracts;
using OrderManagemenSystem.Business.Repositories;
using OrderManagemenSystem.Data.Contracts;
using OrderManagemenSystem.Data.Entities;
using OrderManagemenSystem.Data.Entities.Table;
using OrderManagemenSystem.Data.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("https://localhost:7123").AllowAnyHeader().AllowAnyMethod(); ;
    });
});
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddDbContext<OMSContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("OMSDB"));
});

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepostory>();
builder.Services.AddTransient<IItemRepository, ItemRepostory>();
builder.Services.AddTransient<IOrderRepository, OrderRepostory>();
builder.Services.AddTransient<IVwOrderDetailRepository, VwOrderDetailRepostory>();
builder.Services.AddTransient<IBaseRepository<Order>, BaseRepository<Order>>();
builder.Services.AddTransient<IBaseRepository<Category>, BaseRepository<Category>>();
builder.Services.AddTransient<IBaseRepository<Item>, BaseRepository<Item>>();
builder.Services.AddTransient<IBaseRepository<VwOrderDetail>, BaseRepository<VwOrderDetail>>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<APIMiddleware>();
app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.MapControllers();

app.Run();
