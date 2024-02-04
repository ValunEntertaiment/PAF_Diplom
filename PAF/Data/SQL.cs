using PAF.Data.Entityies;
using PAF.Data.Classes;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Entity;

namespace PAF.Data
{
    class SQL
    {
        #region Employee
        public List<Employee> SelectEmployee()
        {
            using (var context = new MyDbContext())
            {
                return (from e in context.Employees
                        select new Employee
                        {
                            Id = e.Id,
                            LastName = e.LastName,
                            FirstName = e.FirstName,
                            MiddleName = e.MiddleName,
                            Gender = e.Gender,
                            Salary = e.Salary
                        }).ToList();
            }
        }
        public void UpdateEmployee(List<Employee> employees)
        {
            using (var context = new MyDbContext())
            {
                context.Entry(employees).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
        #endregion

        #region Client

        /// <summary>выводит список клиентов</summary>
        /// <returns>коллекция клиентов</returns>
        public List<Client> SelectClient()
        {
            using (var context = new MyDbContext())
            {
                return (from client in context.Clients select new Client
                {
                    LastName = client.LastName,
                    FirstName = client.FirstName,
                    MiddleName = client.MiddleName,
                    Gender = client.Gender,
                    Phone = client.Phone
                }).ToList();    
            }
        }

        /// <summary>изменяет данные в таблице</summary>
        /// <param name="clients">коллекция данных</param>
        public void UpdateClient(List<Clients> clients)
        {
            using (var context = new MyDbContext())
            {
                foreach (var item in clients)
                {
                    context.Entry(item).State = EntityState.Modified;
                }
                context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// добавляет данные в таблице
        /// </summary>
        /// <param name="client">строка данных</param>
        public void InsertClient(Clients client)
        {
            using (var context = new MyDbContext())
            {
                context.Entry(client).State = EntityState.Added;
                context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// удаляет данные в таблице
        /// </summary>
        /// <param name="clients">коллекция данных которые нужно удалить</param>
        public void DeleteClient(List<Clients> clients)
        {
            using (var context = new MyDbContext())
            {
                foreach (var item in clients)
                {
                    context.Entry(item).State = EntityState.Deleted;
                }
                context.SaveChangesAsync();
            }
        }
        #endregion


        internal List<Supplies> SelectSupply()
        {
            using (var context = new MyDbContext())
            {
                return null; //(from e in context.Supplies select e).ToList();
            }
        }
        public List<Sales> SelectSalay()
        {
            using (var context = new MyDbContext())
            {
                return null;//(from e in context.Sales select e).ToList();
            }
        }
        #region Component
        public List<Components> SelectComponent()
        {
            using (var context = new MyDbContext())
            {
                return (from e in context.Components select e).ToList();
            }
        }

        internal List<SalesCompositions> SelectSalayComposition()
        {
            using (var context = new MyDbContext())
            {
                return (from e in context.SalesCompositions select e).ToList();
            }
        }

        public List<DeliveriesCompositions> SelectDeliveriesCompositions()
        {
            using (var context = new MyDbContext())
            {
                return (from e in context.DeliveriesCompositions select e).ToList();
            }
        }

        public void UpdateComponent(List<Components> components)
        {
            using (var context = new MyDbContext())
            {
                context.Components.RemoveRange(context.Components);
                context.Components.AddRange(components);
                context.SaveChanges();
            }
        }
        #endregion

        #region Type
        public List<Types> SelectType()
        {
            using (var context = new MyDbContext())
            {
                return null; //(from e in context.Types select e).ToList<Types>();
            }
        }
        public void UpdateType(List<Types> types)
        {
            using (var context = new MyDbContext())
            {
                context.Types.RemoveRange(context.Types);
                context.Types.AddRange(types);
                context.SaveChanges();
            }
        }
        #endregion

        #region dELIVERY

        public List<Deliveries> SelectDelivery()
        {
            using (var context = new MyDbContext())
            {
                return (from e in context.Deliveries select e).ToList();
            }
        }

        #endregion


    }
}