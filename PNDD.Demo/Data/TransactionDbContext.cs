using Microsoft.EntityFrameworkCore;
using PNDD.Demo.Models;

namespace PNDD.Demo.Data;

public class TransactionDbContext : DbContext
{
    public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options) { }

    public DbSet<Transaction> Transactions => Set<Transaction>();
}
