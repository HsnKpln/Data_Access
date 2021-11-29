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
            //01.01.1998 tarihinden sonra Siparis veren müşterilerin isimlerini ve siparis tarihlerini listeleyiniz.
            //10248 nolu Sipariş hangi kargo sirketi ile gonderilmiştir.
            //TOFU isimli ürün alınan siparişlerin sipariş numaralarını listeleyiniz.
            //DUMON VEYA ALFKI MÜŞTERİLERİNİN ALDIĞI 1 ID Lİ ÇALIŞANIMIN ONAYLADIĞI 1 VEYA 3 NOLU KARGO FİRMASIYLA TAŞINMIŞ SİPARİŞLERİ GETİRİN
        }
    }
}
