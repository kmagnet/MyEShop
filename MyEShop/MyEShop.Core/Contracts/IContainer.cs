using MyEShop.Core.Models;
using System.Linq;

namespace MyEShop.Core.Contracts
{
    public interface IContainer<placeholder> where placeholder : BaseClass
    {
        void Confirm();
        IQueryable<placeholder> Container();
        void Delete(string Id);
        void Insert(placeholder p);
        placeholder Search(string Id);
        void Update(placeholder p);
    }
}