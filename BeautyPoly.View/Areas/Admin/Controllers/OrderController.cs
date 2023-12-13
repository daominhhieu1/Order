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
        public OrderController(OrderRepo orderRepo)
        {
            this.orderRepo = orderRepo;
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

        [HttpGet("admin/order/get-product")]
        public async Task<IActionResult> GetProduct()
        {
            var result = await orderRepo.GetAllAsync();
            return Json(result.Where(p => p.IsDelete == false || p.IsDelete == null).ToList(), new System.Text.Json.JsonSerializerOptions());
        }

        [HttpPost("admin/order/create")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDTO)
        {
            try
            {
                var checkExistCode = await orderRepo.FirstOrDefaultAsync(p => p.OrderCode == orderDTO.order.OrderCode && p.OrderID != orderDTO.order.OrderID);
                if (checkExistCode != null)
                {
                    return Json("Mã đơn hàng đã tồn tại! Vui lòng nhập mã khác.");
                }
                Order order = new Order();
                if (order.OrderID > 0)
                {
                    var orderID = orderDTO.order.OrderID;
                    order.OrderID = orderID;
                    order.TransactStatusID = 1;
                    order.OrderCode = "HD_"+orderID;
                    order.AccountID = 1;
                    order.AccountName = "ACNAME";
                    order.PotentialCustomerID = 1;
                    order.CustomerName = orderDTO.order.CustomerName;
                    order.OrderDate = orderDTO.order.OrderDate;
                    order.ShipDate = orderDTO.order.ShipDate;
                    order.PaymentDate = orderDTO.order.PaymentDate;
                    order.Note = orderDTO.order.Note;
                    order.TotalMoney = orderDTO.order.TotalMoney;
                    order.Address = orderDTO.order.Address;
                    order.MedthodPayment = orderDTO.order.MedthodPayment;
                    order.IsApproved = orderDTO.order.IsApproved;
                    order.IsDelete = orderDTO.order.IsDelete;

                    await orderRepo.UpdateAsync(order);
                }
                else
                {
                    var orderID = orderDTO.order.OrderID;
                    order.OrderID = orderID;
                    order.TransactStatusID = 1;
                    order.OrderCode = "HD_" + order.OrderID;
                    order.AccountID = 1;
                    order.AccountName = "ACNAME";
                    order.PotentialCustomerID = 1;
                    order.CustomerName = orderDTO.order.CustomerName;
                    order.OrderDate = orderDTO.order.OrderDate;
                    order.ShipDate = orderDTO.order.ShipDate;
                    order.PaymentDate = orderDTO.order.PaymentDate;
                    order.Note = orderDTO.order.Note;
                    order.TotalMoney = orderDTO.order.TotalMoney;
                    order.Address = orderDTO.order.Address;
                    order.MedthodPayment = orderDTO.order.MedthodPayment;
                    order.IsApproved = orderDTO.order.IsApproved;
                    order.IsDelete = false;
                    await orderRepo.InsertAsync(order);
                    return Json("Thêm thành công");
                }
                return Json(1);
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
