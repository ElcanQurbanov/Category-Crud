﻿using Entity_Framework_Slider.Data;
using Entity_Framework_Slider.Models;
using Entity_Framework_Slider.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data.Common;

namespace Entity_Framework_Slider.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly AppDbContext _context;

        public CategoryController(ICategoryService categoryService,
                                  AppDbContext context)
        {
            _categoryService = categoryService;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _categoryService.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {   
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            try
            {
                var existData = await _context.Categories.FirstOrDefaultAsync(m => m.Name.Trim().ToLower() == category.Name.Trim().ToLower());
                if (existData != null)
                {
                    ModelState.AddModelError("Name", "This data already exist");
                    return View();
                }

                //int num = 1;
                //int num2 = 0;
                //int result = num / num2;

                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //ViewBag.error = ex.Message;
                return RedirectToAction("Error", new { msj = ex.Message });
            }
            
        }
        public IActionResult Error(string msj)
        {
            ViewBag.error = msj;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            Category category = await _context.Categories.FindAsync(id);

            if (category is null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoftDelete(int? id)
        {
            if (id == null) return BadRequest();

            Category category = await _context.Categories.FindAsync(id);

            if (category is null) return NotFound();

            category.SoftDelete = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
           
            Category category = await _context.Categories.FindAsync(id);

            if (category is null) return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Category category)
        {
            if (id == null) return BadRequest();

            if (ModelState.IsValid)
            {
                return View(category);
            }

            Category dbCategory = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(m=>m.Id == id);

            if (dbCategory is null) return NotFound();

            if(dbCategory.Name.Trim().ToLower() == category.Name.Trim().ToLower())
            {
                return RedirectToAction(nameof(Index));
            }

            //dbCategory.Name = category.Name;

            _context.Categories.Update(category);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            Category category = await _context.Categories.FindAsync(id);

            if (category is null) return NotFound();

            return View(category);
        }

    }
}
