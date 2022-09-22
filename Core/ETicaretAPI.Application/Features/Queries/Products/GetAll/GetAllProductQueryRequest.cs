using ETicaretAPI.Application.RequestParameters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Products.GetAll
{
    //requeste karsılık dönen response 
   public class GetAllProductQueryRequest:IRequest<GetAllProductQueryResponse>
    {
        //public Pagination Pagination { get; set; }  //GetAllProduct bizden istedigi parametre
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;
    }
}
