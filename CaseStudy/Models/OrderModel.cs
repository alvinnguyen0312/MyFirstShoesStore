using Casestudy.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseStudy.Models
{
    public class OrderModel
    {
        private AppDbContext _db;
        public string msgBackOrder = "";
        public OrderModel(AppDbContext ctx)
        {
            _db = ctx;
        }
        public int AddOrder(Dictionary<string, object> items, ApplicationUser user)
        {
            int orderId = -1;
            using (_db)
            {
                // we need a transaction as multiple entities involved
                using (var _trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        Order order = new Order();
                        order.UserId = user.Id;
                        order.OrderDate = System.DateTime.Now;
                        order.OrderAmount = 0;
                        // calculate the totals and then add the order row to the table
                        foreach (var key in items.Keys)
                        {
                            ProductViewModel item =
                            JsonConvert.DeserializeObject<ProductViewModel>(Convert.ToString(items[key]));
                            if (item.Qty > 0)
                            {
                                order.OrderAmount += (decimal)(item.Qty * item.MSRP);
                            }
                        }
                        _db.Orders.Add(order); 
                        _db.SaveChanges();
                        // then add each item to the orderlineItem table
                        foreach (var key in items.Keys)
                        {
                            ProductViewModel item =
                            JsonConvert.DeserializeObject<ProductViewModel>(Convert.ToString(items[key]));
                            if (item.Qty > 0)
                            {
                                OrderLineItem oItem = new OrderLineItem();
                                Product pr = (from p in _db.Products
                                              where p.Id == item.Id
                                              select p).FirstOrDefault();
                                oItem.OrderId = order.Id;
                                if(item.Qty < item.QTYOnHand)
                                {
                                    item.QTYOnHand -= item.Qty;
                                    pr.QtyOnHand = item.QTYOnHand;
                                    oItem.QtyOrdered = item.Qty;
                                    oItem.QtyBackOrdered = 0;
                                    oItem.QtySold = item.Qty;
                                }
                                else
                                {
                                    oItem.QtySold = item.QTYOnHand;
                                    item.QTYOnBackOrder += (item.Qty - item.QTYOnHand);
                                    item.QTYOnHand = 0;
                                    pr.QtyOnHand = 0;
                                    pr.QtyOnBackOrder = item.QTYOnBackOrder;
                                    oItem.QtyOrdered = item.Qty;
                                    oItem.QtyBackOrdered = oItem.QtyOrdered - oItem.QtySold;//oItem.QtyOrdered - oItem.QtySold
                                    msgBackOrder = "Some goods were backordered!";
                                }
                                oItem.SellingPrice = (decimal)item.MSRP;
                                oItem.ProductId = item.Id;
                                _db.OrderLineItems.Add(oItem);
                                _db.Products.Update(pr);
                                _db.SaveChanges();
                            }
                        }
                        _trans.Commit();
                        orderId = order.Id;
                    }
                    catch (Exception ex)
                    {
                        orderId = -1;
                        Console.WriteLine(ex.Message);
                        _trans.Rollback();
                    }
                }
            }
            return orderId;
        }
    }
}