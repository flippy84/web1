using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System;

namespace web1.Models
{
    class ProductModel
    {
        public ProductModel(int length) : this(1, length) { }
        public ProductModel(int start, int length)
        {
            BookshopDatabase d = new BookshopDatabase();
            var h = from product in d.Products select product;
        }
    }

    [Database]
    public class BookshopDatabase : DataContext
    {
        public BookshopDatabase() : base(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True") { }
        
        public Table<Product> Products;
        public Table<Order> Orders;
        public Table<Cart> Carts;
        public Table<OrderRow> OrderRows;
    }

    [Table]
    public class Product
    {
        [Column(IsPrimaryKey = true)] public int ProductId;
        [Column] public string Name;
        [Column] public string Description;
        [Column] public decimal PurchasePrice;
        [Column] public int Stock;
    }

    [Table]
    public class Order
    {
        public Order(OrderDetails details)
        {
            Firstname = details.Firstname;
            Lastname = details.Lastname;
            PostAddress = details.PostAddress;
            PostCode = details.PostCode;
            PostTown = details.PostTown;
            Email = details.Email;
            PhoneNumber = details.PhoneNumber;
        }

        public Order() { }

        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int OrderId;
        [Column] public string Firstname;
        [Column] public string Lastname;
        [Column] public string PostAddress;
        [Column] public string PostCode;
        [Column] public string PostTown;
        [Column] public string Email;
        [Column] public string PhoneNumber;
    }

    [Table]
    public class OrderRow
    {
        public OrderRow(int orderId, int productId, int quantity)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
        }

        public OrderRow() { }

        [Column(IsPrimaryKey = true)] public int OrderId;
        [Column(IsPrimaryKey = true)] public int ProductId;
        [Column] public int Quantity;    
    }

    [Table]
    public class Cart
    {
        [Column(IsPrimaryKey = true)] public Guid CartId;
        [Column(IsPrimaryKey = true)] public int ProductId;
        [Column] public int Amount;
    }
}