using Academy.Core.Services.Interfaces;
using AcademyDataLayer.Context;
using AcademyDataLayer.Entities.Order;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Core.Services
{

    public class OrderServise : IOrderServise
    {
        AcademyContext _context;

        public OrderServise(AcademyContext context)
        {
            _context = context;
        }
        public int AddOrder(string userName, int courseId)
        {
            int userid = _context.Users.Single(u => u.UserName == userName).UserId;

            Order order = _context.Orders.FirstOrDefault(o => o.UserId == userid && !o.IsFinaly);

            var course = _context.Courses.Find(courseId);
            if (order == null)
            {
                order = new Order()
                {
                    UserId = userid,
                    IsFinaly = false,
                    CreateDate = DateTime.Now,
                    OrderSum = course.CoursePrice,
                    OrderDetails = new List<OrderDetail>()
                    {
                        new OrderDetail()
                        {
                              CourseId=courseId,
                              Count=1,
                              Price=course.CoursePrice,
                        }
                    }
                };
                _context.Orders.Add(order);
                _context.SaveChanges();
            }
            else
            {
                OrderDetail detail = _context.OrderDetails.SingleOrDefault(d => d.OrderId == order.OrderId && d.CourseId == courseId);
                if (detail != null)
                {
                    detail.Count += 1;
                    _context.OrderDetails.Update(detail);
                }
                else
                {
                    detail = new OrderDetail()
                    {
                        OrderId = order.OrderId,
                        Count = 1,
                        CourseId = courseId,
                        Price = course.CoursePrice,
                    };
                    _context.OrderDetails.Add(detail);
                }
                _context.SaveChanges();
                UpdatePriceOrder(order.OrderId);
            }
           
            return order.OrderId;
        }

        public void UpdatePriceOrder(int orderId)
        {
            var order = _context.Orders.Find(orderId);

            order.OrderSum = _context.OrderDetails.Where(o => o.OrderId == orderId).Sum(o => o.Price);

            _context.Orders.Update(order);
            _context.SaveChanges();
        }
        public Order GetOrderForUserPanel(string userName, int orderId)
        {
            int userid = _context.Users.Single(u => u.UserName == userName).UserId;

            return _context.Orders.Include(o => o.OrderDetails).ThenInclude(od=>od.Course).FirstOrDefault(o => o.UserId == userid && o.OrderId == orderId);
        }

    }
}
