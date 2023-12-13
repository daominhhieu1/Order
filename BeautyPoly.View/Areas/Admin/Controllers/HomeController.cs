﻿using BeautyPoly.Areas.Admin.Models;
using BeautyPoly.DBContext;
using BeautyPoly.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MessagePack;
using BeautyPoly.Models;
using System.Security.Principal;


namespace BeautyPoly.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private BeautyPolyDbContext _dbcontext;
        public HomeController(BeautyPolyDbContext context)
        {
            _dbcontext = context;
        }

        //  [Authorize(Roles = "Admin")]
        [Route("admin", Name = "index")]
        public IActionResult Index()
        {
            //if (HttpContext.Session.GetString("AccountID") == null)
            //    return RedirectToAction("AdminLogin", "Account");
            return View();
        }
        [Route("admin/login", Name = "Login")]
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        [Route("admin/login", Name = "Login")]
        public async Task<IActionResult> AdminLogin(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Accounts tk = _dbcontext.Accounts
                    .Include(p => p.Roles)
                    .SingleOrDefault(p => p.Email.ToLower() == model.Email.ToLower().Trim());

                    if (tk == null)
                    {
                        ModelState.AddModelError("", "Tài khoản không tồn tại");
                    }
                    string password = string.IsNullOrEmpty(model.Password) ? "" : MaHoaMD5.EncryptPassword(model.Password);
                    //string pass = (model.Password.Trim());
                    // + kh.Salt.Trim()
                    if (tk.Password.Trim() != password)
                    {
                        ModelState.AddModelError("", "Thông tin đăng nhập không đúng. Vui lòng thử lại.");

                        return View(model);
                    }
                    //đăng nhập thành công

                    var taikhoanID = HttpContext.Session.GetString("AccountID");
                    //identity
                    //luuw seccion Makh
                    HttpContext.Session.SetString("AccountID", tk.AccountID.ToString());
                    //identity
                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, tk.FullName),
                        new Claim(ClaimTypes.Email, tk.Email),
                        new Claim("AccountID", tk.AccountID.ToString()),
                        new Claim("RoleID", tk.RoleID.ToString()),
                        new Claim(ClaimTypes.Role, tk.Roles.RoleName)
                    };
                    var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                    var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                    await HttpContext.SignInAsync(userPrincipal);


                   // _notyfService.Success("Đăng nhập thành công");
                    return RedirectToAction("Index", "Home", new { Area = "Admin" });

                }
            }
            catch
            {
                ModelState.AddModelError("", "Đăng nhập thất bại vui lòng kiểm tra lại user name hoặc mật khẩu");
                return View();
            }

            ModelState.AddModelError("", "Đăng nhập thất bại vui lòng kiểm tra lại user name hoặc mật khẩu");
            return View();

        }

        [Route("logout", Name = "Logout")]
        public IActionResult AdminLogout()
        {
            try
            {
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("AccountId");
                return RedirectToAction("AdminLogin", "Home", new { Area = "Admin" });
            }
            catch
            {
                return RedirectToAction("AdminLogin", "Home", new { Area = "Admin" });
            }
        }
    }
}
