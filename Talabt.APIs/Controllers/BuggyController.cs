using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Repository.Data;
using Talabt.APIs.Errors;

namespace Talabt.APIs.Controllers
{
    
    public class BuggyController : BaseAPIController
    {
        private readonly StoreContext _dbcontext;

        public BuggyController(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet("nofound")]
        public ActionResult GetNotFoundRequest()
        {
            var product = _dbcontext.Products.Find(100);
            if(product == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(product);
        }

        [HttpGet("servererror")]
        public ActionResult GetBadRequest()
        {
            var product = _dbcontext.Products.Find(100);
            var productToReturn = product.ToString();
            return Ok(productToReturn);
        }


        [HttpGet("badrequest")]
        public ActionResult GetServerError()
        {
            return BadRequest(new ApiResponse(400));
        }



        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }      
        
        [HttpGet("unauthorized")]
        public ActionResult GetUnauthorizedError()
        {
            return Unauthorized(new ApiResponse(401));
        }
    }
}
