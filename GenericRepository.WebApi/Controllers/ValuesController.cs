using GenericRepository.WebApi.Models;
using GenericRepository.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GenericRepository.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public ValuesController(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Add(User user, CancellationToken cancellationToken)
    {
        await _userRepository.AddAsync(user,cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Ok(new {message="Kullanıcı kaydı başarıyla tamamlandı!" });
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        IList<User> users = await _userRepository.GetAll().ToListAsync(cancellationToken).ConfigureAwait(false);
        return Ok(users);
    }
}
