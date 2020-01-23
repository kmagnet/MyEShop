using MyEShop.Core.Contracts;
using MyEShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEShop.DataAccess.SQL
{
    public class SQLContainer<placeholder> : IContainer<placeholder> where placeholder : BaseClass
    {
        /// <summary>
        /// The context references that DataContext class which implements DbContext
        /// DbSet is the underlying table that we want to access
        /// </summary>
        /// 
        internal DataContext context;
        internal DbSet<placeholder> dbSet;

        public SQLContainer(DataContext context) {
            this.context = context;
            dbSet = context.Set<placeholder>(); // Sets the underlying table by referencing the DbContext 
                                                // by calling the Set method and passing in the model we want
                                                // to work against ie Product or Category table
        }

        public void Confirm() { context.SaveChanges(); }

        public IQueryable<placeholder> Container() { return dbSet; }

        public void Delete(string Id)
        {
            var p = this.Search(Id);
            if (context.Entry(p).State == EntityState.Detached) { dbSet.Attach(p); }

            dbSet.Remove(p); 
        }

        public void Insert(placeholder p) { dbSet.Add(p); }

        public placeholder Search(string Id) { return dbSet.Find(Id); }

        public void Update(placeholder p) 
        {
            dbSet.Attach(p);
            context.Entry(p).State = EntityState.Modified;
        }
    }
}
