using StockFlow.Repository.Contexts;
using StockFlow.Repository.Entities;
using StockFlow.Repository.Interfaces;

namespace StockFlow.Repository.Implementations;
public sealed class UserAuthRepository(AppDbContext dbContext) : GenericRepository<UserAuth>(dbContext), IUserAuthRepository
{
}
