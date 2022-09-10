using ETicaretAPI.Application.Repositories.Files;
using ETicaretAPI.Domain.Entities.Files;
using ETicaretAPI.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Repositories.Files
{
   public class FileReadRepository : ReadRepository<File>, IFileReadRepository
    {
        public FileReadRepository(ETicaretAPIDbContext context) : base(context)
        {

        }
    }
}
