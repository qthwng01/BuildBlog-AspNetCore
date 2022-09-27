using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildBlog.Models;
using BuildBlog.Data;
using Microsoft.EntityFrameworkCore;

namespace BuildBlog.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogContext _context;
        public BlogController(BlogContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.BlogEntries.ToListAsync());
        }

        public IActionResult CreatorPage()
        {
            /*if(id != Guid.Empty)
            {
                BlogEntry blog = _context.BlogEntries.FirstOrDefault(x => x.Id == id);

                return View(blog);
            }*/

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatorPage([Bind("Id,Content")] BlogEntry blog)
        {
            if(ModelState.IsValid)
            {
                if (blog.Id == Guid.Empty)
                {
                    BlogEntry newPost = new BlogEntry();
                    newPost.Id = Guid.NewGuid();
                    newPost.Content = blog.Content;
                    _context.Add(newPost);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(blog);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editPost = await _context.BlogEntries.FindAsync(id);
            if (editPost == null)
            {
                return NotFound();
            }
            return View(editPost);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Content")] BlogEntry blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(blog.Id))
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
            return View(blog);
        }

        private bool PostExists(Guid id)
        {
            return _context.BlogEntries.Any(e => e.Id == id);
        }

    }
}
