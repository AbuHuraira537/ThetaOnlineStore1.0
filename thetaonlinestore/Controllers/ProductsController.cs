using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using thetaonlinestore.Models;

namespace thetaonlinestore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ThetaOnlineStoreContext _context;
        private readonly IHostEnvironment ENV;

        public ProductsController(ThetaOnlineStoreContext context,IHostEnvironment _ENV)
        {
            ENV = _ENV;
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string query)
        {
            if (query != null)
            {
                return View(await _context.Product.Where(a=>a.Name.Contains(query)).ToListAsync());
            }
            else
            {
                return View(await _context.Product.ToListAsync());
            }
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            if(HttpContext.Session.GetString("Role")=="Admin")
            { 
            return View();
            }
            else
            {
                return RedirectToAction("/SystemUsers/LogIn");
            }
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ShortDescription,LongDescription,CurrentStock,CostPrice,SalePrice,Images,ProductCode,Status,OpeningStock,OpeningDate,ProductFeatures")] Product product, IList<IFormFile> PImages)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                string AllImages = "";
                if (PImages != null && PImages.Count > 0)
                {
                    foreach (IFormFile image in PImages)
                    {
                        string FullPath = ENV.ContentRootPath + "\\wwwroot\\Images\\ProductImages\\";
                        string FileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                        string FPath = FullPath + FileName;
                        FileStream FS = new FileStream(FPath, FileMode.Create);
                        image.CopyTo(FS);
                        AllImages += FileName + ",";

                    }
                }



                if (ModelState.IsValid)
                {
                    if(!string.IsNullOrEmpty(AllImages))
                    { 
                    AllImages = AllImages.Remove(AllImages.LastIndexOf(','));
                    product.Images = AllImages;
                    }
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(product);
            }
            else
            {
                return RedirectToAction("/SystemUsers/LogIn");
            }
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("Role") == "Admin" || HttpContext.Session.GetString("Role") == "Staff")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var product = await _context.Product.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
            else
            {
                return RedirectToAction("/SystemUsers/LogIn");
            }
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ShortDescription,LongDescription,CurrentStock,CostPrice,SalePrice,Images,ProductCode,Status,OpeningStock,OpeningDate,ProductFeatures")] Product product)
        {if(HttpContext.Session.GetString("Role")=="Admin"||HttpContext.Session.GetString("Role")=="Staff")
            { 
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
            }
            else
            {
                return RedirectToAction("/SystemUsers/LogIn");
            }
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var product = await _context.Product
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    _context.Product.Remove(product);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("/SystemUsers/LogIn");
            }
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
        public string deleteproduct(int id)
        {if (HttpContext.Session.GetString("Role") == "Admin")
            {
                Product prod = _context.Product.Where(a => a.Id == id).FirstOrDefault();
                if (prod != null)
                {
                    _context.Product.Remove(prod);
                    _context.SaveChanges();
                    return "1";
                }
            }
            return "0";
            
        }
    }
}
