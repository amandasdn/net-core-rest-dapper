using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Domain.Entities;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Project.Application.Util
{
    /// <summary>
    /// Useful methods. 
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Get bytes from a IFormFile.
        /// </summary>
        public static async Task<byte[]> GetBytes(this IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Return a status code 500.
        /// </summary>
        public static ActionResult InternalServerError<T>(this ControllerBase controllerBase, Response<T> response, Exception exception)
        {
            response.SetError(exception?.Message);
            return controllerBase.StatusCode(500, response);
        }
    }
}
