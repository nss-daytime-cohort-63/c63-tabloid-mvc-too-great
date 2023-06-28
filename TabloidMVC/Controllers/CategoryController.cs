using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabloidMVC.Repositories;
using TabloidMVC.Models;
using System.Collections.Generic;
using System;

namespace TabloidMVC.Controllers
{
    public class CategoryController : Controller
    {
        // GET: CategoryController

        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public ActionResult Index()
        {
            List<Category> categories = _categoryRepository.GetAll();
            return View(categories);

        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            try
            {
                _categoryRepository.Add(category);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(category);
            }
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            Category category = _categoryRepository.GetById(id);    
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category category)
        {
            try
            {
                _categoryRepository.Update(category);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(category);
            }
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            Category category = _categoryRepository.GetById(id);

            return View(category);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id,Category category)
        {
            try
            {
                _categoryRepository.Delete(id);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(category);
            }
        }
    }
}
