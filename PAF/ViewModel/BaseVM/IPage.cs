using System.Data;

namespace PAF.ViewModel.BaseVM
{
    public interface IPage
    {
        DataTable DataTable { get; set; }
    }
}
