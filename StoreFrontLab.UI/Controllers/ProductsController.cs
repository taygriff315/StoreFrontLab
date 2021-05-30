using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StoreFrontLab.Data.EF;
using StoreFrontLab.UI.Utilities;
using PagedList;
using PagedList.Mvc;
using StoreFrontLab.UI.Models;

namespace StoreFrontLab.UI.Controllers
{
    public class ProductsController : Controller
    {
        private StoreFrontEntities db = new StoreFrontEntities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Manufacturer).Include(p => p.ProductType);
            return View(products.ToList());
        }
        //public ActionResult BooksGrid()
        //{
        //    List<Product> products = db.Products.Include(foo => foo.BookRoyalty).Include(x => x.BookStatus).Include(g => g.Genre).Include(p => p.Publisher).ToList();
        //    return View(books);
        //}
        public ActionResult ProductDataTables()
        {
            var products = db.Products.Include(p => p.Manufacturer).Include(p => p.ProductType);
            return View(products.ToList());
        }

        public ActionResult TreatsDataTables()
        {
            var treats = db.Products.Include(x => x.Manufacturer).Include(x => x.ProductType).Where(x => x.ProductType.ProductTypeName.ToLower() == "treats");
            return View(treats.ToList());
        }
        public ActionResult ToysDataTables()
        {
            var treats = db.Products.Include(x => x.Manufacturer).Include(x => x.ProductType).Where(x => x.ProductType.ProductTypeName.ToLower() == "toys");
            return View(treats.ToList());
        }
        public ActionResult AccDataTables()
        {
            var treats = db.Products.Include(x => x.Manufacturer).Include(x => x.ProductType).Where(x => x.ProductType.ProductTypeName.ToLower() == "accessories");
            return View(treats.ToList());
        }

        public ActionResult GridView()
        {
            List<Product> products = db.Products.ToList();
            return View(products);
        }

        public ActionResult ToysGridView()
        {
            var toys = db.Products.Where(x => x.ProductType.ProductTypeName.ToLower() == "toys").ToList();
            return View(toys);
        }

        public ActionResult AccGridView()
        {
            var acc = db.Products.Where(x => x.ProductType.ProductTypeName.ToLower() == "accessories").ToList();
            return View(acc);
        }

        public ActionResult TreatsGridView()
        {
            var treats = db.Products.Where(x => x.ProductType.ProductTypeName.ToLower() == "treats").ToList();
            return View(treats);
        }

        public ActionResult KongGridView()
        {
            var kong = db.Products.Where(x => x.Manufacturer.ManufacturerName.ToLower() == "kong").ToList();
            return View(kong);
        }

        public ActionResult ChewyGridView()
        {
            var chewy = db.Products.Where(x => x.Manufacturer.ManufacturerName.ToLower() == "chewy").ToList();
            return View(chewy);
        }

        public ActionResult OlRoyGridView()
        {
            var olRoy = db.Products.Where(x => x.Manufacturer.ManufacturerName == "Ol'Roy").ToList();
            return View(olRoy);
        }


        // GET: Products/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult AddToCart(int qty, int productID)
        {
            Dictionary<int, CartItemViewModel> shoppingCart = null;
            if (Session["cart"] != null)
            {
                shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];
            }
            else
            {
                shoppingCart = new Dictionary<int, CartItemViewModel>();
            }

            Product product = db.Products.Where(p => p.ProductID == productID).FirstOrDefault();
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                CartItemViewModel item = new CartItemViewModel(qty, product);

                if (shoppingCart.ContainsKey(product.ProductID))
                {
                    shoppingCart[product.ProductID].Qty += qty;

                }
                else
                {
                    shoppingCart.Add(product.ProductID, item);    
                }
                Session["cart"] = shoppingCart;
            }

            return RedirectToAction("Index", "ShoppingCart");

        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "ManufacturerName");
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ProductTypeID", "ProductTypeName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductTypeID,ManufacturerID,UnitsSold,InStock,ProductImage,Price,ProductName,Description")] Product product, HttpPostedFileBase coverImg)
        {
            if (ModelState.IsValid)
            {
                string file = "noImage.png";

                if (coverImg != null)
                {
                    file = coverImg.FileName;
                    string ext = file.Substring(file.LastIndexOf("."));
                    string[] goodExts = { ".jpeg", ".jpg", ".png", ".gif",".jfif" };
                    if (goodExts.Contains(ext.ToLower()) && coverImg.ContentLength <= 4194304)
                    {
                        file = Guid.NewGuid() + ext;

                        string savePath = Server.MapPath("~/Content/css/img/prodImgs/");
                        Image convertedImage = Image.FromStream(coverImg.InputStream);
                        int maxImageSize = 500;
                        int maxThumbSize = 100;
                        ImageServices.ResizeImage(savePath, file, convertedImage, maxImageSize, maxThumbSize);
                    }
                    else
                    {
                        file = "noImage.png";
                    }

                }
                product.ProductImage = file;

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "ManufacturerName", product.ManufacturerID);
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ProductTypeID", "ProductTypeName", product.ProductTypeID);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "ManufacturerName", product.ManufacturerID);
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ProductTypeID", "ProductTypeName", product.ProductTypeID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductTypeID,ManufacturerID,UnitsSold,InStock,ProductImage,Price,ProductName,Description")] Product product, HttpPostedFileBase coverImg)
        {
            if (ModelState.IsValid)
            {
                string file = "noImage.png";

                if (coverImg != null)
                {
                    file = coverImg.FileName;
                    string ext = file.Substring(file.LastIndexOf("."));
                    string[] goodExts = { ".jpeg", ".jpg", ".png", ".gif",".jfif" };
                    if (goodExts.Contains(ext.ToLower()) && coverImg.ContentLength <= 4194304)
                    {
                        file = Guid.NewGuid() + ext;

                        string savePath = Server.MapPath("~/Content/css/img/prodImgs/");
                        Image convertedImage = Image.FromStream(coverImg.InputStream);
                        int maxImageSize = 500;
                        int maxThumbSize = 100;
                        ImageServices.ResizeImage(savePath, file, convertedImage, maxImageSize, maxThumbSize);

                        if (product.ProductImage != null && product.ProductImage != "noImage.png")
                        {
                            string path = Server.MapPath("~/Content/img/prodImgs");
                            ImageServices.Delete(path, product.ProductImage);
                        }

                    }

                }
                product.ProductImage = file;

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "ManufacturerName", product.ManufacturerID);
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ProductTypeID", "ProductTypeName", product.ProductTypeID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Product product = db.Products.Find(id);

            string path = Server.MapPath("~/Content/css/img/prodImgs");
            ImageServices.Delete(path, product.ProductImage);

            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ProductPaging(int page =1)
        {
            int pageSize = 5;
            var products = db.Products.OrderBy(b => b.ProductName).ToList();
            return View(products.ToPagedList(page, pageSize));
        }
    }
}
