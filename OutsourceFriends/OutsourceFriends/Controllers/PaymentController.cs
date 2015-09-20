using Braintree;
using OutsourceFriends.Models;
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
    [RoutePrefix("api/Payment")]
    public class PaymentController : BaseController
    {

        BraintreeGateway gateway = new BraintreeGateway
        {
            Environment = Braintree.Environment.SANDBOX,
            MerchantId = "ybvwjrqv83wpk8tf",
            PublicKey = "h2pkqx6wkvz2z4ws",
            PrivateKey = "e030383ed655c5859b2928e4b9864841"
        };



        [Route("Token")]
        [HttpGet]
        public IHttpActionResult Token()
        {
            return Ok(gateway.ClientToken.generate());
        }


        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreatePurchase(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string uid = User.Identity.GetUserId();

            BookingRequest br = await DomainManager.db.BookingRequests.Where(x => x.Id == model.BookingId && x.TravelerId == uid && x.GuideId == model.GuideId).FirstOrDefaultAsync();

            if (br != null)
            {
                var request = new TransactionRequest
                {
                    Amount = br.MaxAmount,
                    PaymentMethodNonce = model.Nounce
                };

                Result<Transaction> result = gateway.Transaction.Sale(request);

                if (result.IsSuccess())
                {
                    br.TransactionId = result.Transaction.Id;
                    DomainManager.updateEntity(br);
                    await DomainManager.save();
                    return Ok();
                }
                else
                {
                    return BadRequest(string.Join(",", result.Errors.All().Select(x=>x.Message)));
                }
            }

            return NotFound();
        }


    }
}
