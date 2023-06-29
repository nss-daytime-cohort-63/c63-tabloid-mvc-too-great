using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabloidMVC.Repositories;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using System;

namespace TabloidMVC.Controllers
{
    public class TagController : Controller
    {
        // GET: HomeController1
            private readonly ITagRepository _tagRepository;

            public TagController(ITagRepository tagRepository)
            {
                _tagRepository = tagRepository;
            }
            public IActionResult Index()
            {
                var tags = _tagRepository.GetAll();
                return View(tags);
            }

            // GET: HomeController1/Details/5
            public IActionResult Details(int id)
            {
                return View();
            }

            // GET: HomeController1/Create
            public IActionResult Create()
            {
                var vm = new TagCreateViewModel();
                return View(vm);
            }

            // POST: HomeController1/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Create(TagCreateViewModel vm)
            {
                try
                {
                    _tagRepository.AddTag(vm.Tag);
                    return RedirectToAction("Index", new { id = vm.Tag.Id });
                }
                catch
                {
                    return View(vm);
                }
            }

            // GET: HomeController1/Edit/5
            public IActionResult Edit(int id)
            {
            Tag tag = _tagRepository.GetTagById(id);

            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

            // POST: HomeController1/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Edit(int id, Tag tag)
            {
                try
                {
                _tagRepository.UpdateTag(tag);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View(tag);
                }
            }

            // GET: HomeController1/Delete/5
            public IActionResult Delete(int id)
            {
            Tag tag = _tagRepository.GetTagById(id);
                return View(tag);
            }

            // POST: HomeController1/Delete/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Delete(int id, Tag tag)
            {
                try
                {
                  _tagRepository.DeleteTag(id);
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    return View(tag);
                }
            }
        }
    }

