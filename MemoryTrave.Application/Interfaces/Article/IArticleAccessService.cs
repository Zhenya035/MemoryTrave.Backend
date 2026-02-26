using MemoryTrave.Application.Dto.Requests;
using MemoryTrave.Application.Dto.Requests.Article.Access;

namespace MemoryTrave.Application.Interfaces.Article;

public interface IArticleAccessService
{
    public Task AddList(AddListAccessDto dto);
    public Task Add(AddAccessDto dto);
    public Task DeleteList(ListIdDto id);
    public Task Delete(Guid id);
}