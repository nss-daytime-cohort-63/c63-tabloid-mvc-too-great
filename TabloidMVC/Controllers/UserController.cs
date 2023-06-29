using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserProfileRepository _userRepo;
        private readonly IUserTypeRepository _userTypeRepo;
        // GET: UserController

        public UserController(IUserProfileRepository userRepo, IUserTypeRepository userTypeRepo)
        {
            _userRepo = userRepo;
            _userTypeRepo = userTypeRepo;
        }
        public ActionResult Index()
        {
            List<UserProfile> users = _userRepo.GetAll();
            return View(users);
        }

        public ActionResult AllDeactive()
        {
            List<UserProfile> users = _userRepo.GetDeactive();
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
            UserProfile user = _userRepo.GetById(id);
            List<UserType> types = _userTypeRepo.GetAll();
            ProfileTypeViewModel pt = new ProfileTypeViewModel
            {
                UserProfile = user,
                UserTypes = types
            };
            return View(pt);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProfileTypeViewModel pt)
        {
            List<UserType> types = _userTypeRepo.GetAll();
            int allAdmins = _userRepo.AllAdmins().Count;
            UserProfile user = _userRepo.GetById(id);
            try
            {
                if (allAdmins < 2 && (user.UserTypeId == 1) && pt.UserProfile.UserTypeId == 2)
                {
                    ModelState.AddModelError("UserProfile", "Cant have zero Admins");
                    pt.UserTypes = types;
                    return View(pt);
                }
                _userRepo.Edit(pt.UserProfile);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(pt);
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
          int myId  = GetCurrentUserProfileId();
        UserProfile myself = _userRepo.GetById(myId);

            if(myself.UserTypeId == 1)
            {
            UserProfile User = _userRepo.GetById(id);
            return View(User);

            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Deactivate(int id, UserProfile userProfile)
        {
            int allAdmins = _userRepo.AllAdmins().Count;
            UserProfile user = _userRepo.GetById(id);
            if(user == null)
            {
                return NotFound();
            }
            try
            {
                if(allAdmins < 2 && (user.UserTypeId == 1)){
                    ModelState.AddModelError("UserTypeId", "Cant have zero Admins");
                    return View(user);
                }
                _userRepo.DeactivateUser(id);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View(userProfile);
            }
        }

        public ActionResult Reactivate(int id)
        {
            int myId = GetCurrentUserProfileId();
            UserProfile myself = _userRepo.GetById(myId);

            if (myself.UserTypeId == 1)
            {
                UserProfile User = _userRepo.GetById(id);
                return View(User);

            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Reactivate(int id, UserProfile user)
        {
            try
            {
                _userRepo.ReactivateUser(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(user);
            }
        }
        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

    }
}
