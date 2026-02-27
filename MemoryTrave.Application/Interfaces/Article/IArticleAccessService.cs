using MemoryTrave.Application.Dto.Requests;
using MemoryTrave.Application.Dto.Requests.Article.Access;
using MemoryTrave.Domain.Common;

namespace MemoryTrave.Application.Interfaces.Article;

public interface IArticleAccessService
{
    public Task<Result> AddList(AddListAccessDto dto);
    public Task<Result> Add(AddAccessDto dto);
    public Task<Result> DeleteList(ListIdDto id);
    public Task<Result> Delete(Guid id);
}