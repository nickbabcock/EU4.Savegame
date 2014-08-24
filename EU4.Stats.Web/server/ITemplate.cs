using System.Threading.Tasks;
namespace EU4.Stats.Web
{
    public interface ITemplate
    {
        Task<string> Render(object obj);
    }
}
