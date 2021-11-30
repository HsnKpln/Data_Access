using Data_Access.Models;
using Data_Access.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Data_Access
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private NorthwindContext _dbContext = new NorthwindContext();
        private void Form1_Load(object sender, EventArgs e)
        {
            var query1 = _dbContext.Categories.Select(x => new CategoryViewModel()
            {
                CategoryName = x.CategoryName,
                Description = x.Description,
                Picture = x.Picture,
                ProductCount = x.Products.Count
            }).ToList();
            dataGridView1.DataSource = query1;

            var query2 = from cat in _dbContext.Categories
                         join prod in _dbContext.Products on cat.CategoryId equals prod.CategoryId
                         //where prod.UnitPrice>20
                         select new
                         {
                             cat.CategoryName,
                             prod.ProductName,
                             prod.UnitPrice
                         };
            dataGridView1.DataSource = query2
                .OrderBy(x => x.CategoryName)
                .ThenByDescending(x=>x.UnitPrice)
                .ToList();

            var query3 = _dbContext.Products.Select(x => new
            {
                x.Category.CategoryName,
                x.ProductName,
                x.UnitPrice
            }).OrderBy(x => x.CategoryName).ThenByDescending(x => x.UnitPrice).ToList();
            dataGridView1.DataSource = query3;

            //FEDERAL SHIPPING iLE TAŞINMIŞVE NANCY'NİN ALMIŞ OLDUĞU SİPARİŞLERİ GÖSTERİNİZ.
            var query4 = from ord in _dbContext.Orders
                         join ship in _dbContext.Shippers on ord.ShipVia equals ship.ShipperId
                         join empl in _dbContext.Employees on ord.EmployeeId equals empl.EmployeeId
                         where empl.FirstName =="Nancy" && ship.CompanyName =="Federal Shipping"
                         select new
                         {
                             ord.OrderId,
                             ord.OrderDate,
                             ship.CompanyName,
                             empl.FirstName,
                             empl.LastName
                         };
            dataGridView1.DataSource = query4.ToList();
            //MÜŞTERİ ADI,ONAYLAYAN ÇALIŞANIN ADI,ALDIĞI ÜRÜNLERİN ADINI LİSTELEYİN
            var query5 = from empl in _dbContext.Employees
                         join ord in _dbContext.Orders on empl.EmployeeId equals ord.EmployeeId
                         join ordD in _dbContext.OrderDetails on ord.OrderId equals ordD.OrderId
                         join cus in _dbContext.Customers on ord.CustomerId equals cus.CustomerId
                         join pro in _dbContext.Products on ordD.ProductId equals pro.ProductId
                         select new
                         {
                             cus.ContactName,
                             empl.FirstName,
                             empl.LastName,
                             pro.ProductName
                         };
            dataGridView1.DataSource = query5.OrderBy(x=>x.FirstName).ToList();
            //01.01.1998 tarihinden sonra Siparis veren müşterilerin isimlerini ve siparis tarihlerini listeleyiniz.
            var query6 = from ord in _dbContext.Orders
                         join cus in _dbContext.Customers on ord.CustomerId equals cus.CustomerId
                        where ord.OrderDate > new DateTime(1998,01,01)
                         select new
                         {
                             cus.ContactName,
                             ord.OrderDate
                         };
            dataGridView1.DataSource = query6.ToList();

            //10248 nolu Sipariş hangi kargo sirketi ile gonderilmiştir.

            var query7 = from ord in _dbContext.Orders
                         join ship in _dbContext.Shippers on ord.ShipVia equals ship.ShipperId
                         where ord.OrderId == 10248
                         select new
                         {
                             ship.CompanyName,
                             ord.OrderId
                         };
            dataGridView1.DataSource = query7.ToList();

            //siparişler hangi müşteriye hangi kargo şirketleriyle kim tarafından onaylanarak gönderilmiştir

            var query8 = from ord in _dbContext.Orders
                         //join ship in _dbContext.Shippers on ord.ShipVia equals ship.ShipperId
                         join cus in _dbContext.Customers on ord.CustomerId equals cus.CustomerId
                         join empl in _dbContext.Employees on ord.EmployeeId equals empl.EmployeeId
                         select new
                         {
                             ord.OrderId,
                             cus.CompanyName,
                             cus.ContactName,
                             empl.FirstName
                         };
            dataGridView1.DataSource = query8.OrderBy(x=>x.CompanyName).ThenByDescending(x=>x.FirstName).ToList();
            //TOFU isimli ürün alınan siparişlerin sipariş numaralarını listeleyiniz.

            var query9 = from ord in _dbContext.Orders
                         join ordD in _dbContext.OrderDetails on ord.OrderId equals ordD.OrderId
                         join prod in _dbContext.Products on ordD.ProductId equals prod.ProductId
                         where prod.ProductName == "Tofu"
                         select new
                         {
                             prod.ProductName,
                             ord.OrderId
                         };
            dataGridView1.DataSource=query9.ToList();


            //DUMON VEYA ALFKI MÜŞTERİLERİNİN ALDIĞI 1 ID Lİ ÇALIŞANIMIN ONAYLADIĞI 1 VEYA 3 NOLU KARGO FİRMASIYLA TAŞINMIŞ SİPARİŞLERİ GETİRİN

            var query10 = from ord in _dbContext.Orders
                          join empl in _dbContext.Employees on ord.EmployeeId equals empl.EmployeeId
                          join ship in _dbContext.Shippers on ord.ShipVia equals ship.ShipperId
                          //where ord.CustomerId == "DUMON" && ord.CustomerId == "ALFKI"
                          where empl.EmployeeId == 1
                          where ship.ShipperId == 1 || ship.ShipperId == 3
                          select new
                          {
                              ord.CustomerId,
                              empl.EmployeeId,
                              ship.ShipperId,
                              ship.CompanyName
                          };
            dataGridView1.DataSource=query10.OrderBy(x=>x.CustomerId).ThenByDescending(x=>x.ShipperId).ToList();
        }
    }
}
