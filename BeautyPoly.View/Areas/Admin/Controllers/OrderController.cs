using BeautyPoly.Data.Models.DTO;
using BeautyPoly.Data.Repositories;
using BeautyPoly.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeautyPoly.View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        OrderRepo orderRepo;
        ProductRepo prodRepo;
        DetailOrderRepo detailOrderRepo;
        ProductSkuRepo productSkuRepo;
        public OrderController(OrderRepo orderRepo, ProductRepo prodRepo, DetailOrderRepo detailOrderRepo, ProductSkuRepo productSkuRepo)
        {
            this.orderRepo = orderRepo;
            this.prodRepo = prodRepo;
            this.detailOrderRepo = detailOrderRepo;
            this.productSkuRepo = productSkuRepo;
        }
        [Route("admin/order")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("admin/order/getall")]
        public async Task<IActionResult> GetAllOrder()
        {
            return Json(await orderRepo.GetAllAsync(), new System.Text.Json.JsonDocumentOptions());
        }
        [HttpGet("admin/order/{statusId}")]
        public async Task<IActionResult> GetOrder(int statusId)
        {
            var result = await orderRepo.FindAsync(x=>x.TransactStatusID == statusId);
            return Json(result.ToList(), new System.Text.Json.JsonSerializerOptions());
        }
        [HttpGet("admin/order/get-product")]
        public async Task<IActionResult> GetProduct()
        {
            var result = await prodRepo.GetAllAsync();
            return Json(result.Where(p => p.IsDelete == false || p.IsDelete == null).ToList(), new System.Text.Json.JsonSerializerOptions());
        }

        [HttpPost("admin/order/create")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDTO)
        {
            try
            {
                Order order = new Order();
                order.OrderCode = "HD_" + DateTime.Now.Ticks;
                var checkExistCode = await orderRepo.FirstOrDefaultAsync(p => p.OrderCode == order.OrderCode && p.OrderID != orderDTO.OrderID);
                if (checkExistCode != null)
                {
                    return Json("Mã đơn hàng đã tồn tại! Vui lòng nhập mã khác.");
                }
                if (orderDTO.prods.Count()<0)
                {

                    return Json("Mã đơn hàng đã tồn tại! Vui lòng nhập mã khác.");
                }
                if (order.OrderID > 0)
                {
                    var orderID = orderDTO.OrderID;
                    order.OrderID = orderID;
                    order.TransactStatusID = 1;
                    order.AccountID = 2;
                    order.AccountName = "ACNAME";
                    order.PotentialCustomerID = 1;
                    order.CustomerName = orderDTO.CustomerName;
                    order.OrderDate = DateTime.Now;
                    order.ShipDate = DateTime.Now.AddDays(3);
                    order.PaymentDate = DateTime.Now;
                    order.Note = orderDTO.Note;
                    order.TotalMoney = orderDTO.prods.Sum(x=>x.Price);
                    order.Address = orderDTO.Address;
                    order.MedthodPayment = orderDTO.MedthodPayment;
                    order.CustomerPhone = orderDTO.CustomerPhone;
                    await orderRepo.UpdateAsync(order);
                }
                else
                {
                    order.TransactStatusID = 1;
                    order.AccountID = 2;
                    order.AccountName = "ACNAME";
                    order.PotentialCustomerID = 1;
                    order.CustomerName = orderDTO.CustomerName;
                    order.OrderDate = DateTime.Now;
                    order.ShipDate = DateTime.Now.AddDays(3);
                    order.PaymentDate = orderDTO.PaymentDate;
                    order.Note = orderDTO.Note;
                    order.TotalMoney = orderDTO.prods.Sum(x => x.Price);
                    order.Address = orderDTO.Address;
                    order.MedthodPayment = orderDTO.MedthodPayment;
                    order.CustomerPhone = orderDTO.CustomerPhone;
                    await orderRepo.InsertAsync(order);

                }
                checkExistCode = await orderRepo.FirstOrDefaultAsync(p => p.OrderCode == order.OrderCode);

                if (checkExistCode != null)
                {
                    foreach (var prod in orderDTO.prods) 
                    {
                        var checkExistDetailOrder = await detailOrderRepo.FirstOrDefaultAsync(p => p.ProductSkusID == prod.ProductID && p.OrderID == checkExistCode.OrderID);
                       
                        if (checkExistDetailOrder != null)
                        {
                            checkExistDetailOrder.Price = prod.Price;
                            checkExistDetailOrder.Quantity = prod.Quantity;
                            await detailOrderRepo.UpdateAsync(checkExistDetailOrder);

                        }
                        else
                        {
                            OrderDetails orderDetail = new OrderDetails
                            {
                                OrderID = checkExistCode.OrderID,
                                Price = prod.Price,
                                Quantity = prod.Quantity,
                                ProductSkusID = prod.ProductID
                            };
                            await detailOrderRepo.InsertAsync(orderDetail);

                        }
                    }
                }
                else
                {
                    return Json("Lỗi insert update");
                }


                return Json("Thêm thành công");
            }
            catch (Exception ex)
            {
                return Json("Lỗi insert update");
            }

        }

        [HttpDelete("admin/order/delete")]
        public async Task<IActionResult> DeleteOrder([FromBody] int orderID)
        {
            await orderRepo.DeleteAsync(await orderRepo.GetByIdAsync(orderID));
            return Json(1);
        }
    }
}
