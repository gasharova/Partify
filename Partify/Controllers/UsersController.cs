using Partify.Filters;
using Partify.Models;
using Partify.Repositories;
using Partify.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Partify.Controllers
{
    public class UsersController : Controller
    {
        [HttpGet]
        public ActionResult Create()
        {
            CreateVM model = new CreateVM();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            User item = new User();
            item.Username = model.Username;
            item.Password = model.Password;
            item.FirstName = model.FirstName;
            item.LastName = model.LastName;

            PartifyDbContext context = new PartifyDbContext();
            context.Users.Add(item);
            context.SaveChanges();

            return RedirectToAction("Login", "Home");
        }

        [AuthFilter]
        [HttpGet]
        public ActionResult Edit()
        {
            PartifyDbContext context = new PartifyDbContext();
            User item = (User)Session["loggedUser"];

            if (item == null)
                return RedirectToAction("Index", "Home");

            EditVM model = new EditVM();
            model.Id = item.Id;
            model.Username = item.Username;
            model.Password = item.Password;
            model.FirstName = item.FirstName;
            model.LastName = item.LastName;

            return View(model);
        }

        [AuthFilter]
        [HttpPost]
        public ActionResult Edit(EditVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            User item = new User();
            item.Id = model.Id;
            item.Username = model.Username;
            item.Password = model.Password;
            item.FirstName = model.FirstName;
            item.LastName = model.LastName;

            PartifyDbContext context = new PartifyDbContext();
            DbEntityEntry entry = context.Entry(item);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}