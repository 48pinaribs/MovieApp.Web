using LINQSampless.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

//model kullanımı önemli DİKKATTTTTT
class ProductModel
{
	public int ProductId { get; set; }
	public string Name { get; set; }
	public decimal? Price { get; set; }
	public int Quantity { get; set; }

}

class OrderModel
{
	public int OrderId { get; set; }
	public decimal Total { get; set; }
	public List<ProductModel> Products { get; set; }
}
class CustomerModel
{
	public CustomerModel()
	{
		this.Orders = new List<OrderModel>();
	}
	public string CustomerId { get; set; }
	public string CustomerName { get; set; }
	public int OrderCount { get; set; }
	public List<OrderModel> Orders { get; set; }
}
internal class Program
{
	private static void Main(string[] args)
	{
		using (var db = new NorthwindContext())
		{
			

		}

		Console.ReadLine();
	}

	private static void Ders12(NorthwindContext db)
	{
		//Müşterilerin verdiği sipariş toplamı ?
		var customers = db.Customers
			   .Where(cus => cus.CustomerId == "PERIC")
			   .Select(cus => new CustomerModel
			   {
				   CustomerId = cus.CustomerId,
				   CustomerName = cus.ContactName,
				   OrderCount = cus.Orders.Count,
				   Orders = cus.Orders.Select(order => new OrderModel
				   {
					   OrderId = order.OrderId,
					   Total = order.OrderDetails.Sum(od => od.Quantity * od.UnitPrice),
					   Products = order.OrderDetails.Select(od => new ProductModel
					   {
						   ProductId = od.ProductId,
						   Name = od.Product.ProductName,
						   Price = od.UnitPrice,
						   Quantity = od.Quantity

					   }).ToList()
				   }).ToList()
			   })
			   .OrderBy(i => i.OrderCount)
			   .ToList();
		foreach (var customer in customers)
		{
			Console.WriteLine(customer.CustomerId + " => " + customer.CustomerName + " => " + customer.OrderCount);
			Console.WriteLine("Siparişler");

			foreach (var order in customer.Orders)
			{
				Console.WriteLine("********************************");
				Console.WriteLine(order.OrderId + "=>" + order.Total);
				foreach (var product in order.Products)
				{
					Console.WriteLine(product.ProductId + "=>" + product.Name + "=>" + product.Price + "=>" + product.Quantity);
				}
			}
		}
	}

	private static void Ders11(NorthwindContext db)
	{
		//var products = db.Products.Where(p => p.CategoryId == 1).ToList();
		//var products = db.Products.Include(p => p.Category).Where(p => p.Category.CategoryName == "Beverages").ToList();

		//var products = db.Products
		//	.Where(p => p.Category.CategoryName == "Beverages")
		//	.Select(p => new
		//	{
		//		name = p.ProductName,
		//		id = p.CategoryId,
		//		categoryName = p.Category.CategoryName

		//	})
		//	.ToList();

		//foreach (var item in products)
		//{
		//	Console.WriteLine(item.name + ' ' + item.id + ' ' + item.categoryName );
		//}

		//var categories = db.Categories.Where(c => c.Products.Count() > 0).ToList();

		//foreach (var item in categories)
		//{
		//	Console.WriteLine(item.CategoryName);
		//}


		//var products = db.Products
		//	.Select(p =>
		//	new
		//	{
		//                companyName = p.Supplier.CompanyName,
		//	   contactName = p.Supplier.ContactName,
		//	   p.ProductName
		//	}).ToList();

		//bu aşamaya kadar extension metot türlerini gördük 
		//şimdi query expressions görcez.
		//var products = (from p in db.Products
		//				where p.UnitPrice>10
		//				select p).ToList();


		var products = (from p in db.Products
						join s in db.Suppliers on p.SupplierId equals s.SupplierId
						select new
						{
							p.ProductName,
							contactName = s.ContactName,
							companyName = s.CompanyName

						}).ToList();


		foreach (var item in products)
		{
			Console.WriteLine(item.ProductName + ' ' + item.companyName + ' ' + item.contactName);
		}
	}

	private static void Ders10(NorthwindContext db)
	{
		var p1 = new Product() { ProductId = 1078 };
		var p2 = new Product() { ProductId = 1079 };

		var products = new List<Product>() { p1, p2 };
		db.Products.RemoveRange(products);
		db.SaveChanges();
	}

	private static void Ders9(NorthwindContext db)
	{
		var p = new Product() { ProductId = 78 };
		//db.Entry(p).State = EntityState.Deleted;
		db.Products.Remove(p);
		db.SaveChanges();
	}

	private static void Ders8(NorthwindContext db)
	{
		var product = db.Products.Find(1);
		if (product != null)
		{
			product.UnitPrice = 28;
			db.Update(product);
			db.SaveChanges();
		}
	}

	private static void Ders7(NorthwindContext db)
	{
		var p = new Product() { ProductId = 1 };
		db.Products.Attach(p);
		p.UnitsInStock = 50;
		db.SaveChanges();
	}

	private static void Ders6(NorthwindContext db)
	{
		//Change tracking
		var product = db.Products.FirstOrDefault(p => p.ProductId == 1);

		if (product != null)
		{
			product.UnitsInStock += 10;
			db.SaveChanges();
			Console.WriteLine("Veri Güncellendi");

		}
	}

	private static void Ders5(NorthwindContext db)
	{
		//var result = db.Products.Count();
		//var result = db.Products.Count(i => i.UnitPrice>10 && i.UnitPrice<30);
		//var result = db.Products.Count(i => !i.Discontinued); (satışta olan ürün sayısı)

		//var result = db.Products.Min(p => p.UnitPrice);
		//var result = db.Products.Where(p => p.CategoryId==2).Max(p => p.UnitPrice);

		//var result = db.Products.Where(p => !p.Discontinued).Average(p => p.UnitPrice);
		//var result = db.Products.Where(p => !p.Discontinued).Sum(p => p.UnitPrice);

		//var result = db.Products.OrderBy(p => p.UnitPrice).ToList();
		var result = db.Products.OrderByDescending(p => p.UnitPrice).ToList();


		foreach (var item in result)
		{
			Console.WriteLine(item.ProductName + ' ' + item.UnitPrice);
		}
	}

	private static void Ders4(NorthwindContext db)
	{
		//var category = db.Categories.Where(i => i.CategoryName == "Beverages").FirstOrDefault();

		//var p1 = new Product() { ProductName = "Yeni ürün 1", Category = new Category() { CategoryName = "Yeni Category 1" } };
		//var p2 = new Product() { ProductName = "Yeni ürün 2", Category = new Category() { CategoryName = "Yeni Category 2" } };

		////category.Products.Add(p1);
		////category.Products.Add(p2);
		//var products = new List<Product>() { p1, p2 };
		//db.Products.AddRange(products);
		//db.SaveChanges();

		//Console.WriteLine("veriler eklendi");
		//Console.WriteLine(p1.ProductId);
		//Console.WriteLine(p2.ProductId);




	}

	private static void Ders3(NorthwindContext db)
	{
		//Tüm müşteri kayıtlarını getiriniz. (Customers)
		//var customers = db.Customers.ToList();
		//foreach (var customer in customers)
		//{
		//	Console.WriteLine(customer.ContactName);
		//}

		//******************************************************

		//Tüm müsterilerin sadece CustomerId ve ContactName kolonlarını getiriniz .
		//var customers = db.Customers.Select(c => new {c.CustomerId,c.ContactName}).ToList();
		//foreach (var customer in customers)
		//{
		//	Console.WriteLine(customer.CustomerId + ' ' + customer.ContactName);
		//}

		//*********************************************************

		//Almanya ' da yaşayan müşterilerin adlarını getiriniz.
		//var customers = db.Customers.Select(c => new { c.ContactName, c.Country }).Where(c => c.Country == "Germany").ToList();
		//foreach (var customer in customers)
		//{
		//	Console.WriteLine(customer.ContactName);
		//}

		//********************************************************

		//"Diego Roel" isimli müşteri nerede çalışmaktadır?
		//var customers = db.Customers.Where(c => c.ContactName == "Diego Roel").FirstOrDefault();
		//Console.WriteLine(customers.ContactName + ' ' + customers.CompanyName);

		//*************************************************************

		//Stokta olmayan ürünler hangileridir?
		//var products = db.Products.Where(p => p.UnitsInStock == 0).ToList();
		//foreach (var product in products)
		//{
		//	//Console.WriteLine(product.ProductName);
		//}

		//*******************************************************

		//Tüm çalışanların ad ve soyadlarını tek kolon şeklinde getiriniz.
		//var employees = db.Employees.Select(i => new { FullName = i.FirstName + ' ' + i.LastName }).ToList();
		//foreach (var emp in employees)
		//{
		//	Console.WriteLine(emp.FullName);
		//}

		//**************************************************************

		//Ürünler tablosundaki ilk 5 kaydı alınız.
		//var products = db.Products.Take(5).ToList();
		//foreach (var p in products)
		//{
		//	Console.WriteLine(p.ProductName + ' ' + p.ProductId);
		//}

		//******************************************************************

		//Ürünler tablosundaki ikinci 5 kaydı alınız.(Take,Skip)
		var products = db.Products.Skip(5).Take(5).ToList();
		foreach (var p in products)
		{
			Console.WriteLine(p.ProductName + ' ' + p.ProductId);
		}
	}

	private static void Ders2(NorthwindContext db)
	{
		//var products = db.Products.Select(p => new {p.ProductName, p.UnitPrice}).Where(p => p.UnitPrice>18).ToList();
		//var products = db.Products.Where(p => p.UnitPrice>18 && p.UnitPrice<30).ToList();
		//var products = db.Products.Where(p => p.CategoryId >= 1 && p.CategoryId <= 5).ToList();
		//var products = db.Products.Where(p => p.CategoryId == 1 || p.CategoryId == 5).ToList();
		//var products = db.Products.Where(p => p.CategoryId == 1).Select(p => new {p.ProductName, p.UnitPrice}).ToList();
		var products = db.Products.Where(p => p.ProductName.Contains("Ch")).ToList();


		foreach (var p in products)
		{
			Console.WriteLine(p.ProductName + ' ' + p.UnitPrice);
		}
	}

	private static void Ders1(NorthwindContext db)
	{
		//var products = db.Products.ToList();
		//var products = db.Products.Select(p => p.ProductName).ToList();
		//var products = db.Products.Select(p => new { p.ProductName, p.UnitPrice }).ToList();

		//var products = db.Products.Select(p =>
		//new ProductModel 
		//{
		//	Name = p.ProductName,
		//	Price = p.UnitPrice

		//}).ToList();

		//var product = db.Products.First();
		var product = db.Products.Select(p => new { p.ProductName, p.UnitPrice }).FirstOrDefault();


		Console.WriteLine(product.ProductName + ' ' + product.UnitPrice);


		//foreach (var p in products)
		//         {
		//             Console.WriteLine(p.Name + ' ' + p.Price);
		//         }
	}
}
