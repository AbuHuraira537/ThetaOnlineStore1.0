using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using thetaonlinestore.Models;

namespace thetaonlinestore.Controllers
{
    public class SystemUsersController : Controller
    {
        private readonly ThetaOnlineStoreContext _context;

        public SystemUsersController(ThetaOnlineStoreContext context)
        {
           
            _context = context;
        }

        // GET: SystemUsers
        public async Task<IActionResult> Index(string query)
        {
            if (query != null)
            {
                return View(await _context.SystemUser.Where(a => a.DisplayName.Contains(query)).ToListAsync());
            }
            else
            {
                return View(await _context.SystemUser.ToListAsync());
            }
        }

        // GET: SystemUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemUser = await _context.SystemUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemUser == null)
            {
                return NotFound();
            }

            return View(systemUser);
        }

        // GET: SystemUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SystemUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Password,DisplayName,Email,Mobile,Status,Role,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] SystemUser systemUser)
        {
            if (ModelState.IsValid)
            {
                systemUser.CreatedBy = HttpContext.Session.GetString("UserName");
                systemUser.CreatedDate = System.DateTime.Now;
                _context.Add(systemUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(systemUser);
        }

        // GET: SystemUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemUser = await _context.SystemUser.FindAsync(id);
            if (systemUser == null)
            {
                return NotFound();
            }
            return View(systemUser);
        }

        // POST: SystemUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Password,DisplayName,Email,Mobile,Status,Role,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] SystemUser systemUser)
        {
            if (id != systemUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    systemUser.ModifiedBy = HttpContext.Session.GetString("UserName");
                    systemUser.ModifiedDate = System.DateTime.Now;
                    _context.Update(systemUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SystemUserExists(systemUser.Id))
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
            return View(systemUser);
        }

        // GET: SystemUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemUser = await _context.SystemUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemUser == null)
            {
                return NotFound();
            }
            else
            {
                
                _context.SystemUser.Remove(systemUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: SystemUsers/Delete/5
      
        private bool SystemUserExists(int id)
        {
            return _context.SystemUser.Any(e => e.Id == id);
        }
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(SystemUser user)
        {
            SystemUser tempuser = await _context.SystemUser.Where(a => a.UserName == user.UserName && a.Password == user.Password).FirstOrDefaultAsync();
            if(tempuser!=null)
            {
                HttpContext.Session.SetString("DisplayName", tempuser.DisplayName);
                HttpContext.Session.SetString("Role", tempuser.Role);
                HttpContext.Session.SetString("UserName", tempuser.UserName);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "invalid credentials");
                return View();
            }

        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(LogIn));
        }
        public string deleteUser(int id)
        {
            try { 
            SystemUser su = _context.SystemUser.Where(a => a.Id == id).FirstOrDefault();
            if(su!=null)
            {
                _context.SystemUser.Remove(su);
                _context.SaveChanges();
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch
            {
                return "0";
             
            }
        }

    }
}
