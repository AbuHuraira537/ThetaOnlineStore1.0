using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using thetaonlinestore.Models;

namespace thetaonlinestore.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ThetaOnlineStoreContext _context;
        IHostEnvironment ENV;
        public CategoriesController(ThetaOnlineStoreContext context,IHostEnvironment _ihe)
        {
            _context = context;
            ENV = _ihe;
        }

        // GET: Categories
        public async Task<IActionResult> Index(string query)
        {
            if (query != null)
            {
                return View(await _context.Category.Where(a => a.Name.Contains(query)).ToArrayAsync());
            }
            else
            {
                return View(await _context.Category.ToListAsync());
            }
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Picture,Status,ShortDescription,LongDescription,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] Category category, IFormFile CImage)
        {/*
            string afn = "";
            string fp = ENV.ContentRootPath + "\\wwwroot\\Images\\ProductImages\\";
            string fn = Guid.NewGuid() + Path.GetExtension(file.File}Name);
            FileStream fs = new FileStream(fp+fn, FileMode.Create);
            file.CopyTo(fs);
            afn += (fn + ",");
            
            string b = "";
            string a = a.Remove(b.LastIndexOf(","));
            */
            string UniqueFileName = "";
            if (CImage != null) { 
            string PathToRoot = ENV.ContentRootPath;
            string FinalPath = PathToRoot + "\\wwwroot\\Images\\CategoryImages\\";
            UniqueFileName = Guid.NewGuid()+Path.GetExtension(CImage.FileName);
            string FPath = FinalPath + UniqueFileName;
            FileStream FS = new FileStream(FPath, FileMode.Create);
            CImage.CopyTo(FS);
            }
            if (ModelState.IsValid)
            {
                category.Picture = UniqueFileName;
                category.CreatedDate = System.DateTime.Now;
                category.CreatedBy = HttpContext.Session.GetString("UserName");
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Picture,Status,ShortDescription,LongDescription,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    category.ModifiedBy = HttpContext.Session.GetString("UserName");
                    category.ModifiedDate = DateTime.Now.ToString();
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            else
            {
                _context.Category.Remove(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

         
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
        public string deleteproduct(int id)
        {
            Category cg = _context.Category.Where(a => a.Id == id).FirstOrDefault();
            if(cg!=null)
            {
                _context.Category.Remove(cg);
                _context.SaveChanges();
                return "1";
            }
            return "0";
            
        }
    }
}
