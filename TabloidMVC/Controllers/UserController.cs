using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserProfileRepository _userRepo;
        // GET: UserController

        public UserController(IUserProfileRepository userRepo)
        {
            _userRepo = userRepo;
        }
        public ActionResult Index()
        {
            List<UserProfile> users = _userRepo.GetAll();
            return View(users);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            UserProfile User = _userRepo.GetById(id);
            return View(User);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Deactivate(int id)
        {
          
            UserProfile User = _userRepo.GetById(id);
            return View(User);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Deactivate(int id, UserProfile user)
        {
            try
            {
                _userRepo.DeactivateUser(id);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View(user);
            }
        }

       
    }
}
