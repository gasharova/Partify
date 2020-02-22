using Partify.Filters;
using Partify.Models;
using Partify.Repositories;
using Partify.ViewModels.Parties;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Partify.Controllers
{
    [AuthFilter]
    public class PartiesController : Controller
    {
        public ActionResult Index()
        {
            User loggedUser = (User)Session["loggedUser"];

            IndexVM model = new IndexVM();

            PartifyDbContext context = new PartifyDbContext();
            Party[] MyParties = context.Parties
                                    .Where(i => i.OwnerId == loggedUser.Id)
                                    .ToArray();
            int NumberOfParties = MyParties.Count();
            List<string>[] InvitedToMyParties = new List<string>[NumberOfParties];

            foreach(Party p in MyParties)
            {
                int index = Array.IndexOf(MyParties, p);
                InvitedToMyParties[index] = context.Invites
                                                .Where(inv => inv.Party.Id == p.Id)
                                                .Select(inv=>inv.Receiver.Username)
                                                .ToList();
            }

            model.Items = MyParties;
            model.Invited = InvitedToMyParties;

            return View(model);
        }

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

            User loggedUser = (User)Session["loggedUser"];

            Party item = new Party();
            item.Title = model.Title;
            item.Description = model.Description;
            item.OwnerId = loggedUser.Id;

            PartifyDbContext context = new PartifyDbContext();
            context.Parties.Add(item);
            context.SaveChanges();

            return RedirectToAction("Index", "Parties");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            PartifyDbContext context = new PartifyDbContext();
            Party item = context.Parties
                                    .Where(i => i.Id == id)
                                    .FirstOrDefault();

            if (item == null)
                return RedirectToAction("Index", "Parties");

            EditVM model = new EditVM();
            model.Id = item.Id;
            model.Title = item.Title;
            model.Description = item.Description;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            User loggedUser = (User)Session["loggedUser"];

            Party item = new Party();
            item.Id = model.Id;
            item.Title = model.Title;
            item.Description = model.Description;
            item.OwnerId = loggedUser.Id;

            PartifyDbContext context = new PartifyDbContext();
            DbEntityEntry entry = context.Entry(item);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("Index", "Parties");
        }

        public ActionResult Delete(int id)
        {
            PartifyDbContext context = new PartifyDbContext();
            Party item = context.Parties
                                    .Where(i => i.Id == id)
                                    .FirstOrDefault();

            context.Parties.Remove(item);
            context.SaveChanges();

            return RedirectToAction("Index", "Parties");
        }
    }
}