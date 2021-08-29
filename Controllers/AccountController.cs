using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExpenseManagement.Models;
using ExpenseManagement.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System;
using ExpenseManagement.Data;
using static ExpenseManagement.Helpers.ProcessCollectionHelper;

namespace ExpenseManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly ExpenseContext _context;

        public AccountController(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager, ExpenseContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [Route("account/login")]
        public IActionResult Login()
        {
            var model = new LoginViewModel
            {

            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("account/login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync("", "", false, true);

            if (ModelState.IsValid)
            {
                try
                {
                    var user = _userManager.FindByEmailAsync(model.Email).Result;

                    if (user.IsActive != false)
                    {
                        result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);
                        if (!result.Succeeded)
                        {
                            ViewBag.LoginError = "E-Posta veya Şifre Hatalı Girildi! Lütfen Tekrar Deneyiniz!";
                        }
                        else
                        {
                            user.LastLoginDate = DateTime.Now;
                            _context.Update(user);
                            await _context.SaveChangesAsync();
                            return Redirect("~/Home");
                        }
                    }
                    else
                    {
                        ViewBag.LoginError = "Hesabınız Pasif Durumdadır! Lütfen Yöneticiniz ile İletişime Geçiniz!";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.LoginError = ex.Message;
                    return View(model);
                }
            }
            return View(model);
        }

        [Route("account/logout")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/account/login");
        }

        [Route("account/forget-password")]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("account/forget-password")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                AppIdentityUser user = await _userManager.FindByEmailAsync(email);

                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var resetLink = Url.Action("ResetPassword", "Account", new { token }, protocol: HttpContext.Request.Scheme);

                    SendEmail(user.Email, resetLink);

                    ViewBag.Message = "Şifre Sıfırlama Linki E-Postanıza Gönderilmiştir!";

                    return View();
                }
                ViewBag.Message = "E-Posta Hatalı Girildi! Lütfen Tekrar Deneyiniz!";
                return View();
            }
            ViewBag.Message = "Lütfen E-Posta Alanını Doldurunuz!";
            return View();
        }

        public void SendEmail(string userEmail, string resetLink)
        {
            MailMessage msg = new MailMessage();
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");

            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = null;
            smtp.Credentials = new System.Net.NetworkCredential("banazsigorta@gmail.com", "Banaz26.,");

            msg.From = new MailAddress("banazsigorta@gmail.com", "Banaz Sigorta");

            msg.To.Add(userEmail);

            msg.Subject = "Şifrenizi Sıfırlayın";
            msg.Body = "Şifrenizi sıfırlamak için lütfen tıklayınız: " + resetLink;

            smtp.Send(msg);
        }

        [Route("account/reset-password")]
        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        [Route("account/reset-password")]
        public async Task<IActionResult> ResetPassword(RegisterViewModel model)
        {
            try
            {
                AppIdentityUser user = await _userManager.FindByEmailAsync(model.Email);

                IdentityResult result = _userManager.ResetPasswordAsync(user, model.Token, model.Password).Result;

                if (!result.Succeeded)
                {
                    foreach (var validateItem in result.Errors)
                        ModelState.AddModelError("", validateItem.Description);

                    return View(model);
                }

                ViewBag.Message = "Şifreniz Başarıyla Yenilenmiştir!";
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [Authorize(Roles = ("Admin, Banaz"))]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = ("Admin, Banaz"))]
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var requestFormData = Request.Form;

            List<AppIdentityUser> users = await _context.Users
                .AsNoTracking()
                .ToListAsync();

            List<AppIdentityUser> listItems = ProcessCollection(users, requestFormData);

            var response = new PaginatedResponse<AppIdentityUser>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = users.Count,
                RecordsTotal = users.Count
            };

            return Ok(response);
        }

        [Authorize(Roles = ("Admin, Banaz"))]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = ("Admin, Banaz"))]
        [HttpPost]
        public async Task<IActionResult> Create(string email, string password)
        {
            if (email != null && password != null)
            {
                var user = new AppIdentityUser
                {
                    UserName = email,
                    Email = email,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors.FirstOrDefault().Description);
                }
                return Ok(new { Result = true, Message = "Kullanıcı Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("Tüm Alanları Doldurunuz!");
        }

        [Authorize(Roles = ("Admin, Banaz"))]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return View("Error");
            }

            RegisterViewModel regUser = new RegisterViewModel
            {
                Email = user.Email,
                Password = ""
            };
            return View(regUser);
        }

        [Authorize(Roles = ("Admin, Banaz"))]
        [HttpPost]
        public async Task<IActionResult> Edit(string email, string password)
        {
            if (email != null && password != null)
            {
                RegisterViewModel model = new RegisterViewModel()
                {
                    Email = email,
                    Password = password
                };

                AppIdentityUser user = await _userManager.FindByEmailAsync(model.Email);

                var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;

                IdentityResult result = _userManager.ResetPasswordAsync(user, token, model.Password).Result;

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors.FirstOrDefault().Description);
                }
                return Ok(new { Result = true, Message = "Kullanıcı Başarıyla Güncellendi!" });
            }
            return BadRequest("Tüm Alanları Doldurunuz!");
        }

        [Authorize(Roles = ("Admin, Banaz"))]
        [HttpPost]
        public async Task<IActionResult> Passive(string passiveUserId)
        {
            var user = await _context.Users.FindAsync(passiveUserId);
            if (user == null)
            {
                return BadRequest("Kullanıcı Bulunamadı!");
            }

            user.IsActive = false;

            _context.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { Result = true, Message = "Kullanıcı Pasif Olarak Ayarlanmıştır!" });
        }

        [Authorize(Roles = ("Admin, Banaz"))]
        [HttpPost]
        public async Task<IActionResult> Active(string passiveUserId)
        {
            var user = await _context.Users.FindAsync(passiveUserId);
            if (user == null)
            {
                return BadRequest("Kullanıcı Bulunamadı!");
            }

            user.IsActive = true;

            _context.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { Result = true, Message = "Kullanıcı Aktif Olarak Ayarlanmıştır!" });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
