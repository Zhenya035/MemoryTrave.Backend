using MemoryTrave.Application.Interfaces;
using MemoryTrave.Application.Interfaces.User;
using MemoryTrave.Domain.Interfaces;

namespace MemoryTrave.Application.Services.User;

public class UserService(IUserRepository repository, ICurrentUserProvider user) : IUserService
{

}