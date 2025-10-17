using System.Threading.Tasks;

namespace Infrastructure.SceneManagement
{
    public interface ILoadingScreen
    {
        Task ShowAsync();
        Task HideAsync();
    }
}