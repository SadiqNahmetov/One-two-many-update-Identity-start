using EntityFrameworkProject.Data;
using EntityFrameworkProject.Helpers;
using EntityFrameworkProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        public BlogController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Blog> blogs = await _context.Blogs.Where(m => !m.IsDeleted).AsNoTracking().ToListAsync();
            return View(blogs);
        }


        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            try
            {
                if (!ModelState.IsValid) return View();

                if (!blog.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Please choose correct image type");
                    return View();
                }

                if (!blog.Photo.CheckFileSize(300))
                {
                    ModelState.AddModelError("Photo", "Please choose correct image size");
                    return View();
                }

                string fileName = Guid.NewGuid().ToString() + "_" + blog.Photo.FileName;



                string path = Helper.GetFilePath(_env.WebRootPath, "img", fileName);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await blog.Photo.CopyToAsync(stream);
                }

                blog.Image = fileName;

                await _context.Blogs.AddAsync(blog);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
          
        }



        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            Blog blog = await _context.Blogs.FindAsync(id);

            if (blog == null) return NotFound();

            return View(blog);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            try
            {
                if (id is null) return BadRequest();

                Blog blog = await _context.Blogs.FirstOrDefaultAsync(m => m.Id == id);

                if (blog is null) return NotFound();

                return View(blog);

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Blog blog)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(blog);
                }

                if (!blog.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Please choose correct image type");
                    return View();
                }

                if (!blog.Photo.CheckFileSize(400))
                {
                    ModelState.AddModelError("Photo", "Please choose correct image size");
                    return View();
                }

                string fileName = Guid.NewGuid().ToString() + "-" + blog.Photo.FileName;

                Blog dbblog = await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

                if (dbblog == null) return NotFound();

                if (dbblog.Title.Trim().ToLower() == blog.Title.Trim().ToLower() && dbblog.Photo == blog.Photo)
                {
                    return RedirectToAction(nameof(Index));
                }

                string path = Helper.GetFilePath(_env.WebRootPath, "img", fileName);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await blog.Photo.CopyToAsync(stream);
                }

                blog.Image = fileName;

                _context.Blogs.Update(blog);

                await _context.SaveChangesAsync();

                string dbPath = Helper.GetFilePath(_env.WebRootPath, "img", dbblog.Image);

                Helper.DeleteFile(dbPath);

                return RedirectToAction(nameof(Index));
             
            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }




        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Blog blog = await _context.Blogs.FindAsync(id);

            if (blog == null) return NotFound();

            string path = Helper.GetFilePath(_env.WebRootPath, "img", blog.Image);

            Helper.DeleteFile(path);

            _context.Blogs.Remove(blog);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
