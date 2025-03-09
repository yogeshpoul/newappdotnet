using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


List<Product> products = new()
{
    new Product { Id = 1, Name = "Laptop", Price = 1200 },
    new Product { Id = 2, Name = "Phone", Price = 800 }
};

// Get all products
app.MapGet("/products", () => Results.Ok(products));

// Get product by ID
app.MapGet("/products/{id:int}", (int id) =>
{
    var product = products.Find(p => p.Id == id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

// Create a new product
app.MapPost("/products", ([FromBody] Product product) =>
{
    products.Add(product);
    return Results.Created($"/products/{product.Id}", product);
});

// Delete a product
app.MapDelete("/products/{id:int}", (int id) =>
{
    var product = products.Find(p => p.Id == id);
    if (product is not null)
    {
        products.Remove(product);
        return Results.NoContent();
    }
    return Results.NotFound();
});

app.Run();

// Define the Product model
record Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
