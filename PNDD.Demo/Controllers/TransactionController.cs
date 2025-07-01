using Microsoft.AspNetCore.Mvc;
using PNDD.Demo.Data;
using PNDD.Demo.Models;
using PNDD.Demo.Attributes;

namespace PNDD.Demo.Controllers;

public class TransactionController : Controller
{
    private readonly TransactionDbContext _db;

    public TransactionController(TransactionDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [PathNode("/Transaction/Create", View = "Create", Fixture = "TransactionFixture.json")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Transaction model)
    {
        if (!ModelState.IsValid)
            return View(model);

        _db.Transactions.Add(model);
        await _db.SaveChangesAsync();

        return RedirectToAction("Confirmation", new { id = model.Id });
    }

    [HttpGet]
    [PathNode("/Transaction/Confirmation", View = "Confirmation")]
    public async Task<IActionResult> Confirmation(int id)
    {
        var txn = await _db.Transactions.FindAsync(id);
        if (txn == null)
            return NotFound();

        return View(txn);
    }

    [HttpGet]
    [PathNode("/Transaction/History", View = "History")]
    public IActionResult History()
    {
        var txns = _db.Transactions.ToList();
        return View(txns);
    }

    [HttpGet]
    [PathNode("/Transaction/Refund", View = "Refund")]
    public IActionResult Refund()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Refund(int transactionId)
    {
        var txn = await _db.Transactions.FindAsync(transactionId);
        if (txn == null)
        {
            TempData["RefundStatus"] = $"Transaction {transactionId} not found.";
        }
        else
        {
            _db.Transactions.Remove(txn);
            await _db.SaveChangesAsync();
            TempData["RefundStatus"] = $"Transaction {transactionId} refunded.";
        }

        return RedirectToAction("RefundConfirm");
    }

    [HttpGet]
    [PathNode("/Transaction/RefundConfirm", View = "RefundConfirm")]
    public IActionResult RefundConfirm()
    {
        var status = TempData["RefundStatus"]?.ToString() ?? "No refund processed.";
        return Content(status);
    }
}
