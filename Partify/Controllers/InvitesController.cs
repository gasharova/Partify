using Partify.Filters;
using Partify.Models;
using Partify.Repositories;
using Partify.ViewModels.Invites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Partify.Controllers
{
    [AuthFilter]
    public class InvitesController : Controller
    {
        // GET: Invites
        public ActionResult Index()
        {
            User loggedUser = (User)Session["loggedUser"];

            IndexVM model = new IndexVM();

            PartifyDbContext context = new PartifyDbContext();

            List<int> partyIds = context.Invites
                                    .Where(inv => inv.Receiver.Id == loggedUser.Id)
                                    .Select(inv => inv.Party.Id)
                                    .ToList();

            int NumberOfParties = partyIds.Count();

            Party[] parties = new Party[NumberOfParties];
            List<string>[] InvitedToTheParties = new List<string>[NumberOfParties];

            int i = 0;
            foreach (int partyId in partyIds)
            {
                Party party = context.Parties
                            .Where(p => p.Id == partyId)
                            .FirstOrDefault();
                if (party != null)
                {
                    parties[i] = party;
                    i++;
                }
            }

            model.Items = parties;

            foreach (Party p in parties)
            {
                int index = Array.IndexOf(parties, p);
                InvitedToTheParties[index] = context.Invites
                                                .Where(inv => inv.Party.Id == p.Id)
                                                .Select(inv => inv.Receiver.Username)
                                                .ToList();
            }

            model.Invited = InvitedToTheParties;

            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int partyId)
        {
            CreateVM model = new CreateVM();
            model.PartyId = partyId;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateVM model)
        {
            if (!ModelState.IsValid)
                return View(model);


            PartifyDbContext context = new PartifyDbContext();
            Party P = context.Parties // get the party object from db
                                .Where(p => p.Id == model.PartyId)
                                .FirstOrDefault();
            User Sender = (User)Session["loggedUser"]; // get the sender obj from session
            User Receiver = context.Users // get the receiver object from db
                                        .Where(u => u.Username == model.ReceiverUsername)
                                        .FirstOrDefault();

            if (Receiver == null || P == null) // if receiver username or party id wrong/invalid
            {
                ModelState.AddModelError(string.Empty, "Wrong username or party");
                return View(model);
            }

            if (Receiver.Id == Sender.Id) // if user tries to invite themselves
            {
                ModelState.AddModelError(string.Empty, "You cannot invite yourself");
                return View(model);
            }

            if (P.OwnerId == Receiver.Id) // hosts cannot be invited to their parties
            {
                ModelState.AddModelError(string.Empty, "You cannot invite the host");
                return View(model);
            }

            bool IsThisTheOwner = false;
            if (P.OwnerId == Sender.Id) IsThisTheOwner = true;
            else // if the sender of the invite is not the party owner
            {
                object isSenderInvited = context.Invites // check db if the sender is at least invited
                                        .Where(i => (i.Sender.Id == P.OwnerId)
                                        && (i.Receiver.Id == Sender.Id))
                                        .FirstOrDefault();
                if(isSenderInvited == null) // if not
                {
                    ModelState.AddModelError(string.Empty, "You are unauthorised to invite people to this party");
                    return View(model); // return
                }
            }

            Invite inv = new Invite();
            inv.Sender = context.Users
                            .Where(u => u.Id == Sender.Id)
                            .FirstOrDefault();
            inv.Receiver = Receiver;
            inv.Party = P;

            Invite check = context.Invites.SingleOrDefault(dbInvite => (dbInvite.Party.Id == inv.Party.Id)
                                                        && (dbInvite.Receiver.Id == inv.Receiver.Id)
                                                        && (dbInvite.Sender.Id == inv.Sender.Id)
                                            );

            if (check == null)
            {
                context.Invites.Add(inv);
                context.SaveChanges();
            }

            if(IsThisTheOwner)
                return RedirectToAction("Index", "Parties");

            return RedirectToAction("Index", "Invites");
        }

        public ActionResult Delete(int partyId)
        {
            PartifyDbContext context = new PartifyDbContext();

            User u = (User)Session["loggedUser"];

            Invite inv = context.Invites
                                .Where(i => (i.Party.Id == partyId) && (i.Receiver.Id == u.Id))
                                .FirstOrDefault();

            context.Invites.Remove(inv);
            context.SaveChanges();

            return RedirectToAction("Index", "Invites");
        }
    }
}