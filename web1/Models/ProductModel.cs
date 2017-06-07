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
        [Column(IsPrimaryKey = true)] public int OrderId;
        [Column] public string Firstname;
        [Column] public string Lastname;
        [Column] public string Address;
        [Column] public string PostTown;
        [Column] public string PostalCode;
    }

    [Table]
    public class Cart
    {
        [Column(IsPrimaryKey = true)] public Guid CartId;
        [Column(IsPrimaryKey = true)] public int ProductId;
        [Column] public int Amount;
    }
}