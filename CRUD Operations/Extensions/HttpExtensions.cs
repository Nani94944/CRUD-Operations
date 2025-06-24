using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace CRUD_Operations.Helpers
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader ( this HttpResponse response , int currentPage , int itemsPerPage , int totalItems , int totalPages )
        {
            var paginationHeader = new
            {
                currentPage ,
                itemsPerPage ,
                totalItems ,
                totalPages
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            response.Headers.Add ( "Pagination" , JsonSerializer.Serialize ( paginationHeader , options ) );
            response.Headers.Add ( "Access-Control-Expose-Headers" , "Pagination" );
        }
    }
}
