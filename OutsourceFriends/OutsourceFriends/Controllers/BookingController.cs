using OutsourceFriends.Context;
using OutsourceFriends.Models;
using SelfieJobs.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using OutsourceFriends.Helpers;
using System.Data.Entity.SqlServer;

namespace OutsourceFriends.Controllers
{
    [Authorize]
    [RequireSecureConnectionFilter]
    [RoutePrefix("api/Booking")]
    public class BookingController : BaseController
    {


        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> AddBooking(BookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string uid = User.Identity.GetUserId();
            if (uid != model.GuideId)
            {
                Traveler t = await DomainManager.db.Travelers.FirstOrDefaultAsync(x => x.UserId == uid);
                if (t != null)
                {
                    Guide g = await DomainManager.db.Guides.FirstOrDefaultAsync(x => x.UserId == model.GuideId && x.MinBudget <= model.MinAmount);
                    if (g != null)
                    {
                        BookingRequest br = new BookingRequest()
                        {
                            Created = DateTime.UtcNow,
                            AcceptedDate = null,
                            Dates = model.Dates.Select(x => new BookingDate()
                            {
                                Date = DateTimeFromUnixTimestampMillis(x)
                            }).ToList(),
                            TravelerId = t.UserId,
                            Traveler = t,
                            MinAmount = model.MinAmount,
                            MaxAmount = model.MaxAmount,
                            Guide = g,
                            GuideId = g.UserId
                        };

                        t.Bookings.Add(br);


                        DomainManager.updateEntity(t);
                        await DomainManager.save();


                        return Ok(BookingHelper.bookingViewFunc(br));
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }

            return BadRequest();
        }

        [Route("New")]
        [HttpGet]
        public async Task<IHttpActionResult> GetNewBookings()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string uid = User.Identity.GetUserId();

            var l = await (from ls in DomainManager.db.BookingRequests
                           where ls.GuideId == uid
                           where ls.AcceptedDate == null
                           where ls.Dates.Any()
                           select BookingHelper.bookingViewSelector)
                    .ToListAsync();

            return Ok(l);
        }

        [Route("Rejected")]
        [HttpGet]
        public async Task<IHttpActionResult> GetRejectedBookings()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string uid = User.Identity.GetUserId();

            var l = await (from ls in DomainManager.db.BookingRequests
                           where ls.GuideId == uid
                           where !ls.AcceptedDate.HasValue
                           where !ls.Dates.Any()
                           select BookingHelper.bookingViewSelector)
                    .ToListAsync();

            return Ok(l);
        }


        [Route("Accepted")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAcceptedBookings()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string uid = User.Identity.GetUserId();

            var l = await (from ls in DomainManager.db.BookingRequests
                           where ls.GuideId == uid
                           where ls.AcceptedDate.HasValue
                           select BookingHelper.bookingViewSelector)
                    .ToListAsync();

            return Ok(l);
        }



        [Route("{bookingid}/Accept/{dateid}")]
        [HttpPost]
        public async Task<IHttpActionResult> AcceptedBooking(Int64 bookingid, Int64 dateid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string uid = User.Identity.GetUserId();

            BookingRequest br = await (from b in DomainManager.db.BookingRequests
                                       where b.Id == bookingid
                                       where b.GuideId == uid
                                       select b).Include(x => x.Dates).FirstOrDefaultAsync();

            if (br != null)
            {
                BookingDate d = br.Dates.Where(x => x.Id == dateid).FirstOrDefault();
                if (d != null)
                {
                    br.AcceptedDate = d.Date;
                    br.Dates.Clear();
                }

                DomainManager.updateEntity(br);
                await DomainManager.save();
            }

            return Ok();
        }

        [Route("{bookingid}/Reject/{time}")]
        [HttpPost]
        public async Task<IHttpActionResult> RejectBooking(Int64 bookingid, Int64 dateid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string uid = User.Identity.GetUserId();

            BookingDate br = await (from b in DomainManager.db.BookingDates
                                    where b.Id == dateid
                                    where b.BookingId == bookingid
                                    where b.Booking.GuideId == uid
                                    select b).FirstOrDefaultAsync();

            if (br != null)
            {
                DomainManager.db.BookingDates.Remove(br);
                await DomainManager.save();
            }

            return Ok();
        }


        [Route("{bookingid}/Plan")]
        [HttpPost]
        public async Task<IHttpActionResult> AddPlanItem(Int64 bookingid, BookingPlanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string uid = User.Identity.GetUserId();

            BookingRequest br = await (from b in DomainManager.db.BookingRequests
                                       where b.Id == bookingid
                                       where b.GuideId == uid
                                       select b).Include(x => x.PlanItems).FirstOrDefaultAsync();

            if (br != null)
            {
                br.PlanItems.Add(new BookingPlanItem()
                {
                    Amount = model.Amount,
                    Duration = model.Duration,
                    Title = model.Title,
                    BookingId = bookingid
                });

                DomainManager.updateEntity(br);
                await DomainManager.save();
            }

            return Ok();
        }


        [Route("{bookingid}/Plan/{planid}")]
        [HttpDelete]
        public async Task<IHttpActionResult> RemovePlanItem(Int64 bookingid, Int64 planId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string uid = User.Identity.GetUserId();

            BookingPlanItem br = await (from b in DomainManager.db.BookingPlanItems
                                        where b.BookingId == bookingid
                                        where b.Id == planId
                                        where b.Booking.GuideId == uid
                                        select b).FirstOrDefaultAsync();

            if (br != null)
            {
                DomainManager.db.BookingPlanItems.Remove(br);
                await DomainManager.save();
            }

            return Ok();
        }


    }
}
