using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using thetaonlinestore.Models;
using HeyRed.Mime;

namespace thetaonlinestore.Controllers
{
    public class SystemUsersController : Controller
    {
        private readonly ThetaOnlineStoreContext _context;
        private readonly IHostEnvironment ENV;

        public object MimeGuesser { get; private set; }

        public SystemUsersController(ThetaOnlineStoreContext context,IHostEnvironment _ENV)
        {
                _context = context;
                ENV = _ENV;
            
        }

        // GET: SystemUsers
        public async Task<IActionResult> Index(string query)
        {
            if (HttpContext.Session.GetString("Role") == "Admin" || HttpContext.Session.GetString("Role") == "Staff")
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
            else
            {
                return RedirectToAction(nameof(LogIn));
            }
        }

        // GET: SystemUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("Role") == "Admin" || HttpContext.Session.GetString("Role") == "Staff")
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
            else
            {
                return RedirectToAction(nameof(LogIn));
            }
            
        }

        // GET: SystemUsers/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction(nameof(LogIn));
            }
        }

        // POST: SystemUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Password,DisplayName,Email,Mobile,Status,Role,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] SystemUser systemUser,IFormFile UCV,IFormFile UImage)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                if(UImage!=null && UCV != null)
                { 
                string RootPath = ENV.ContentRootPath;
                string ImagePath = RootPath + "\\Images\\SystemUserImages\\";
                string ImageName = Guid.NewGuid() + Path.GetExtension(UImage.FileName);
                FileStream FS = new FileStream(ImagePath + ImageName, FileMode.Create);
                UImage.CopyTo(FS);
                    
                    string CVPath = RootPath + "\\Docs\\Cvs\\";
                    string CvName = Guid.NewGuid() + Path.GetExtension(UCV.FileName);
                    FileStream fs = new FileStream(CVPath + CvName, FileMode.Create);
                    UCV.CopyTo(fs);

                    systemUser.Image = ImageName;
                    systemUser.CV = CvName;
                }
                if (ModelState.IsValid)
                {
                    systemUser.CreatedBy = HttpContext.Session.GetString("UserName");
                    systemUser.CreatedDate = System.DateTime.Now;
                    _context.Add(systemUser);
                    await _context.SaveChangesAsync();

                    //sending sms
                    string smsurl = "https://sendpk.com/api/sms.php?username=923036405108&password=bsef17m537&sender=Masking&mobile=923104866129&message=messagefromhuraira";

                    WebRequest request = HttpWebRequest.Create(smsurl);
                    WebResponse response = request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string urlText = reader.ReadToEnd(); // it takes the response from your url. now you can use as your need  
                  
                    //mail
                    MailMessage email = new MailMessage();
                    email.From = new MailAddress("abouhuraira967@gmail.com", "ABU HURAIRA");
                    email.To.Add("bsef17m537@pucit.edu.pk");
                    email.Subject = "Testing Email";
                    email.Body = "<h1>Testing completed:"+urlText+"</h1>";
                    email.IsBodyHtml = true;
                   string mt= MimeTypesMap.GetMimeType(ENV.ContentRootPath + "\\Images\\Docs\\Cvs\\" +systemUser.CV);
                   // email.Attachments.Add(new Attachment("\\Images\\Docs\\Cvs\\" +systemUser.CV,mt));
                    /*if(System.IO.File.Exists("/Docs/Cvs" + systemUser.CV))
                    { 
                    email.Attachments.Add(new Attachment("/Docs/Cvs" + systemUser.CV));
                    }*/
                    SmtpClient server = new SmtpClient();
                    server.Host = "smtp.gmail.com";
                    server.Credentials = new System.Net.NetworkCredential("abouhuraira967@gmail.com", "ABUHURAIRA");
                    server.Port = 587; //other ports are 25 465
                    server.EnableSsl = true;
                    server.Send(email);


                    //sending sms
                    


                    return RedirectToAction(nameof(Index));
                }
                return View(systemUser);
            }
            else
            {
                return RedirectToAction(nameof(LogIn));
            }
        }

        // GET: SystemUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("Role") == "Admin" || HttpContext.Session.GetString("Role") == "Staff")
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
            else
            {
                return RedirectToAction(nameof(LogIn));
            }
        }

        // POST: SystemUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Password,DisplayName,Email,Mobile,Status,Role,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] SystemUser systemUser)
        {
            if (HttpContext.Session.GetString("Role") == "Admin" || HttpContext.Session.GetString("Role") == "Staff")
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
            else
            {
                return RedirectToAction(nameof(LogIn));
            }
        }

        // GET: SystemUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
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
            else
            {
                return RedirectToAction(nameof(LogIn));
            }
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
                if(!string.IsNullOrEmpty(tempuser.DisplayName))
                {
                    HttpContext.Session.SetString("DisplayName", tempuser.DisplayName);
                }
                if(!string.IsNullOrEmpty(tempuser.Role))
                {

                    HttpContext.Session.SetString("Role", tempuser.Role);
                }
                if(!string.IsNullOrEmpty(tempuser.UserName))
                { 
                HttpContext.Session.SetString("UserName", tempuser.UserName);
                }
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
            if (HttpContext.Session.GetString("Role") == "Admin")
            { 

            try
            { 
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
            else
            {
                return "0";
            }
        }

    }
}
