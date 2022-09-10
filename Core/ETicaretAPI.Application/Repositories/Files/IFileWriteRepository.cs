using ETicaretAPI.Domain.Entities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Repositories.Files
{
    public interface IFileWriteRepository : IWriteRepository<File>
    {
    }
}
