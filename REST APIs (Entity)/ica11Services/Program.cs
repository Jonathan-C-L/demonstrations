// 'WebApplication' is the class provided by Microsoft - used to configure the HTTP pineline and routes
using ica11Services.Models;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(); // switches from minimal APIs to controller-based APIs (alternative approach)
var app = builder.Build();

app.UseCors(x => x
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .SetIsOriginAllowed(origin => true) // allows any origin
                  ); // 'UserCors()' allows calling the webservice(ws) from any website

app.UseDeveloperExceptionPage(); // developer error messages displayed

Random rnd = new Random();

// web requests below
app.MapGet("/", () =>
{
    string[] message = { "Welcome to Tim Hortons" };
    return message;
}); // MapGet handles all GET requests received

app.MapGet("/location", () =>
{
    using (var db = new Jle16RestaurantDbContext())
    {
        return db.Locations
                 .Select(e => new { e.LocationName })
                 .ToList();
    }
});
app.MapGet("/menu", () =>
{

    using (var db = new Jle16RestaurantDbContext())
    {
        IList items = db.Items
            .Select(e => new { e.ItemName, e.ItemPrice })
            .ToList();
        IList locations = db.Locations
            .Select(e => new { e.LocationName })
            .ToList();
        return Results.Ok(new
        {
            items = items,
            locations = locations
        });
    }
});
app.MapDelete("/deleteOrder/{orderId}/{customerId}/{location}", (string orderId, string customerId, string location) =>
{
    bool error = false;
    string errorString = "";
    if (!int.TryParse(orderId, out int oId) || oId < 0)
    {
        error = true;
        errorString += "<p>Order Id not found</p>";
    }
    if (customerId == "")
    {
        error = true;
        errorString += "<p>Customer ID cannot be empty</p>";
    }
    if (!int.TryParse(customerId, out int id) || id <= 0)
    {
        error = true;
        errorString += "<p>Customer ID must be a positive integer</p>";
    }
    if (error)
    {
        return Results.Ok(new
        {
            status = errorString
        });
    }
    using (var db = new Jle16RestaurantDbContext())
    {
        db.Remove(db.Orders.First(o => o.OrderId == oId));
        try
        {
            db.SaveChanges();
            IList orders = db.Orders
            .Where(e => e.Location.LocationName.ToLower() == location.ToLower() && e.Cid == id)
            .Select(o => new { o.OrderId, o.OrderDate, o.PaymentMethod, o.Item.ItemName, o.Item.ItemPrice, o.ItemCount })
            .ToList();
            // these queries return IQueryable collections, can only get with a collection type
            List<string> fname = db.Customers
                .Where(c => c.Cid == id)
                .Select(c => c.Fname)
                .ToList();
            List<string> lname = db.Customers
                .Where(c => c.Cid == id)
                .Select(c => c.Lname)
                .ToList();
            return Results.Ok(new
            {
                data = orders,
                status = (orders.Count > 0) ? $"Orders placed by {fname[0]} {lname[0]} at location: {location}" : "Unable to find customer"
            });
        }
        catch (Exception e)
        {
            return Results.Ok(new
            {
                status = $"Error: {e.Message}"
            });
        }
    }
});
app.MapPut("/updateOrder", (UpdateInfo update) =>
{
    bool error = false;
    string errorString = "";
    // validation
    if (update.time < 0)
    {
        error = true;
        errorString += "<p>Error in estimated time</p>";
    }
    if (!int.TryParse(update.orderId, out int oId) || oId < 0)
    {
        error = true;
        errorString += "<p>Order Id must be a positive integer</p>";
    }
    if (update.itemName == "") // no item selected
    {
        error = true;
        errorString += "<p>Please select an item</p>";
    }
    if (!int.TryParse(update.itemQuantity, out int iQuantity) || iQuantity < 0) // non positive or non-numeric value for quanitity
    {
        error = true;
        errorString += "<p>Order quantity must be a positive integer</p>";
    }
    if (!int.TryParse(update.id, out int customerId) || customerId < 0) // non positive integer
    {
        error = true;
        errorString += "<p>Customer id must be a positive integer</p>";
    }
    if (update.payment == "") // no payment type selected
    {
        error = true;
        errorString += "<p>Please select a payment type</p>";
    }
    if (update.location == "") // no location selected
    {
        error = true;
        errorString += "<p>Please select a pick up location</p>";
    }
    if (error)
    {
        return Results.Ok(new
        {
            status = errorString
        });
    }
    using (var db = new Jle16RestaurantDbContext())
    {
        // THIS CAN BE SO MUCH EASIER IF I SAVED THE IDS IN THE FIRST PLACE
        int itemId;
        // getting the new item id
        using (var getItemId = new Jle16RestaurantDbContext())
        {
            itemId = getItemId.Items
                .Where(o => o.ItemName == update.itemName)
                .OrderBy(o => o.Itemid)
                .Select(o => o.Itemid)
                .FirstOrDefault();
        }
        int locationId;
        // adding order information 
        Order addOrder = db.Orders.First(o => o.OrderId == oId);
        addOrder.Itemid = itemId;
        addOrder.PaymentMethod = update.payment;
        addOrder.ItemCount = iQuantity;
        //db.Orders.Update(addOrder); // adding this to the Orders collection

        // committing changes
        try
        {
            db.SaveChanges();
            IList items = db.Items
                .Select(e => new { e.ItemName, e.ItemPrice })
                .ToList();
            IList locations = db.Locations
                .Select(e => new { e.LocationName })
                .ToList();
            int orderId;
            // getting the new order id
            using (var getOrderId = new Jle16RestaurantDbContext())
            {
                orderId = getOrderId.Orders
                    .Where(o => o.Cid == customerId)
                    .OrderBy(o => o.OrderId)
                    .Select(o => o.OrderId)
                    .LastOrDefault();
            }
            return Results.Ok(new
            {
                time = update.time,
                status = $"Order updated! Your order will be ready in {update.time} minute(s). Feel free to edit your order.",
                orderId = orderId,
                selectedItem = update.itemName,
                payment = update.payment,
                selectedQuantity = iQuantity,
                selectedLocation = update.location,
                customerId = customerId,
                items = items,
                locations = locations
            });
        }
        catch (Exception e)
        {
            db.ChangeTracker.Clear();
            return Results.Ok(new
            {
                status = $"Error in the database: {e.Message}"
            });
        }
    }
});
app.MapPost("/confirmOrder", (OrderInfo order) =>
{
    bool error = false;
    string errorString = "";
    // validation
    if (order.itemName == "") // no item selected
    {
        error = true;
        errorString += "<p>Please select an item</p>";
    }
    if (!int.TryParse(order.itemQuantity, out int iQuantity) || iQuantity < 0) // non positive or non-numeric value for quanitity
    {
        error = true;
        errorString += "<p>Order quantity must be a positive integer</p>";
    }
    if (!int.TryParse(order.id, out int customerId) || customerId < 0) // non positive integer
    {
        error = true;
        errorString += "<p>Customer id must be a positive integer</p>";
    }
    if (order.payment == "") // no payment type selected
    {
        error = true;
        errorString += "<p>Please select a payment type</p>";
    }
    if (order.location == "") // no location selected
    {
        error = true;
        errorString += "<p>Please select a pick up location</p>";
    }
    if (error)
    {
        return Results.Ok(new
        {
            status = errorString
        });
    }
    using (var db = new Jle16RestaurantDbContext())
    {
        int itemId;
        // getting the new order id
        using (var getItemId = new Jle16RestaurantDbContext())
        {
            itemId = getItemId.Items
                .Where(o => o.ItemName == order.itemName)
                .OrderBy(o => o.Itemid)
                .Select(o => o.Itemid)
                .FirstOrDefault();
        }
        int locationId;
        // getting the new order id
        using (var getLocation = new Jle16RestaurantDbContext())
        {
            locationId = getLocation.Locations
                .Where(o => o.LocationName == order.location)
                .OrderBy(o => o.Locationid)
                .Select(o => o.Locationid)
                .FirstOrDefault();
        }
        Order addOrder = new Order();
        addOrder.Itemid = itemId;
        addOrder.OrderDate = DateTime.Now;
        addOrder.PaymentMethod = order.payment;
        addOrder.ItemCount = iQuantity;
        addOrder.Locationid = locationId;
        addOrder.Cid = customerId;
        db.Orders.Add(addOrder);
        try
        {
            db.SaveChanges();
            IList items = db.Items
                .Select(e => new { e.ItemName, e.ItemPrice })
                .ToList();
            IList locations = db.Locations
                .Select(e => new { e.LocationName })
                .ToList();
            int orderId;
            // getting the new order id
            using (var getOrderId = new Jle16RestaurantDbContext())
            {
                orderId = getOrderId.Orders
                    .Where(o => o.Cid == customerId)
                    .OrderBy(o => o.OrderId)
                    .Select(o => o.OrderId)
                    .LastOrDefault();
            }
            // get random time so timing is consistent
            int time = rnd.Next(5, 30);

            return Results.Ok(new
            {
                status = $"Thank you for your order. Your order will be ready in {time} minute(s). Feel free to edit your order.",
                time = time,
                orderId = orderId,
                selectedItem = order.itemName,
                payment = order.payment,
                selectedQuantity = iQuantity,
                selectedLocation = order.location,
                customerId = customerId,
                items = items,
                locations = locations
            });
        }
        catch (Exception e)
        {
            db.ChangeTracker.Clear();
            return Results.Ok(new
            {
                status = $"Error in the database: {e.Message}"
            });
        }
    }
});
app.MapGet("/customerOrders/{customerId}/{location}", (string customerId, string location) =>
{
    bool error = false;
    string errorString = "";
    if (customerId == "")
    {
        error = true;
        errorString += "<p>Customer ID cannot be empty</p>";
    }
    if (!int.TryParse(customerId, out int id) || id <= 0)
    {
        error = true;
        errorString += "<p>Customer ID must be a positive integer</p>";
    }
    if (error)
    {
        return Results.Ok(new
        {
            status = errorString
        });
    }
    using (var db = new Jle16RestaurantDbContext())
    {
        IList orders = db.Orders
            .Where(e => e.Location.LocationName.ToLower() == location.ToLower() && e.Cid == id)
            .Select(o => new { o.OrderId, o.OrderDate, o.PaymentMethod, o.Item.ItemName, o.Item.ItemPrice, o.ItemCount })
            .ToList();
        // these queries return IQueryable collections, can only get with a collection type
        List<string> fname = db.Customers
            .Where(c => c.Cid == id)
            .Select(c => c.Fname)
            .ToList();
        List<string> lname = db.Customers
            .Where(c => c.Cid == id)
            .Select(c => c.Lname)
            .ToList();

        return Results.Ok(new
        {
            data = orders,
            status = (orders.Count > 0) ? $"Orders placed by {fname[0]} {lname[0]} at location: {location}" : "Unable to find customer"
        });
    }

});

app.Run();

// add all JSON fields to a the record for automatic binding
record class ProcessOrder
{
    public string? location { get; set; }
    public string? name { get; set; }
    public string? item { get; set; }
    public int? quantity { get; set; }
    public string? payment { get; set; }
}

record class OrderInfo(string itemName, string itemQuantity, string location, string payment, string id);
record class UpdateInfo(int time, string itemName, string itemQuantity, string location, string payment, string id, string orderId);