﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEATS.Models;

namespace SEATS.Controllers
{
    [Authorize(Roles="Admin")]
    public class LocationsController : Controller
    {
        private ApplicationDbContext db;

        //private SeatsContext db { get; set; }

        public LocationsController()
        {
            this.db = new ApplicationDbContext();
        }
        // GET: Locations
        public async Task<ActionResult> Index()
        {
            return View(await db.Locations.ToListAsync());
        }

        // GET: Locations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = await db.Locations.FindAsync(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // GET: Locations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name")] Location location)
        {
            if (ModelState.IsValid)
            {
                db.Locations.Add(location);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(location);
        }

        // GET: Locations/Edit/5
        public ActionResult Edit(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Location location = await db.Locations.FindAsync(id);
            //if (location == null)
            //{
            //    return HttpNotFound();
            //}
            // return View(location);

            ViewBag.Message = "Edit disabled!";
            return View("Error");
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name")] Location location)
        {
            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(location);
        }

        // GET: Locations/Delete/5
        public ActionResult Delete(int? id)
        {
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Location location = await db.Locations.FindAsync(id);
        //    if (location == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(location);
              ViewBag.Message = "Delete disabled!";
            return View("Error");
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Location location = await db.Locations.FindAsync(id);
            db.Locations.Remove(location);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
