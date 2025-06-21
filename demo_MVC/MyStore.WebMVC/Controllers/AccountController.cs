using Microsoft.AspNetCore.Mvc;
using MyStore.Business;
using MyStore.Services;  // Sử dụng IAccountService
using System.Threading.Tasks;

namespace MyStore.WebMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;  // Inject IAccountService vào controller

        // Constructor để inject IAccountService
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // Hiển thị thông tin tài khoản theo ID
        public async Task<IActionResult> Index(string accountId)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                return NotFound();
            }

            var account = await _accountService.GetAccountByIdAsync(accountId);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // Hiển thị trang tạo tài khoản
        public IActionResult Create()
        {
            return View();
        }

        // Tạo tài khoản mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,FullName,EmailAddress,MemberPassword")] AccountMember account)
        {
            if (ModelState.IsValid)
            {
                await _accountService.SaveAccountAsync(account);  // Gọi AccountService để lưu tài khoản
                return RedirectToAction(nameof(Index), new { accountId = account.MemberId });
            }
            return View(account);
        }

        // Hiển thị trang chỉnh sửa tài khoản
        public async Task<IActionResult> Edit(string accountId)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                return NotFound();
            }

            var account = await _accountService.GetAccountByIdAsync(accountId);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // Cập nhật tài khoản
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string accountId, [Bind("MemberId,FullName,EmailAddress,MemberPassword")] AccountMember account)
        {
            if (accountId != account.MemberId.ToString())
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _accountService.UpdateAccountAsync(account);  // Gọi AccountService để cập nhật tài khoản
                }
                catch (Exception)
                {
                    // Lỗi cập nhật có thể được xử lý tại đây
                    return RedirectToAction(nameof(Index), new { accountId = account.MemberId });
                }
                return RedirectToAction(nameof(Index), new { accountId = account.MemberId });
            }
            return View(account);
        }

        // Xóa tài khoản
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string accountId)
        {
            var account = await _accountService.GetAccountByIdAsync(accountId);
            if (account != null)
            {
                await _accountService.DeleteAccountAsync(account);  // Gọi AccountService để xóa tài khoản
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
