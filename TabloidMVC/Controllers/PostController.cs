using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using TabloidMVC.Models;
using System;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPostReactionRepository _postReactionRepository;
        private readonly IReactionRepository _reactionRepository;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository,IPostReactionRepository postReactionRepository,
         IReactionRepository reactionRepository   )
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _postReactionRepository = postReactionRepository;
            _reactionRepository = reactionRepository;
        }
        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }
        public IActionResult PostByIdIndex()
        {
            int userId = GetCurrentUserProfileId();

            var currentUsersPosts = _postRepository.CurrentUsersPosts(userId);
            return View(currentUsersPosts);
        }

        public IActionResult Details(int id)
        {
            List <PostReaction> pr = _postReactionRepository.GetAllByPost(id);
            List<Reaction> AllReactions = _reactionRepository.GetAll();
            var post = _postRepository.GetPublishedPostById(id);
            PostReactionViewModel prvw = new PostReactionViewModel()
            {
                Post = post,
                PostReactions = pr,
                Reactions = AllReactions
            };
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            return View(prvw);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Details(int id, PostReactionViewModel prvw)
        {
            int userId = GetCurrentUserProfileId();
            PostReaction pr = new PostReaction
            {
                PostId = id,
                ReactionId = prvw.ReactionId,
                UserProfileId = userId
            };
            try
            {
                _postReactionRepository.Add(pr);
                return RedirectToAction(nameof(Details), new {id});
            }
            catch(Exception ex)
            {
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            } 
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }
        public ActionResult Edit(int id)
        {
            List<Category> categories = _categoryRepository.GetAll();

            PostEditViewModel vm = new PostEditViewModel()
            {
                Post = new Post(),
                CategoryOptions = categories
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Post post)
        {
            post.Id = id;
            try
            {
                _postRepository.UpdatePost(post);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(post);
            }
        }

        public ActionResult Delete(int id)
        {
            Post post = _postRepository.GetPublishedPostById(id);
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.DeletePost(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(post);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
