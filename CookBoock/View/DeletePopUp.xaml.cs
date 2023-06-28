using CookBoock.Helpers;
using CookBoock.ViewModel;

namespace CookBoock.View;

public partial class DeletePopUp
{
    DeletePopUpViewModel _viewModel { get; set; }
	public DeletePopUp()
	{
		InitializeComponent();
		BindingContext = _viewModel = new DeletePopUpViewModel();
	}

    public override async Task<DialogReturnValue> ShowAsync(bool animate = true)
    {
        await base.ShowAsync(animate);

        return await _viewModel.ReturnValueAsync();
    }

    private async Task ExecuteButtonClickAsync(DialogReturnStatuses status)
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;

        await HideAsync();

        _viewModel.SetReturnValue(status);

        IsBusy = false;
    }

    private  void OnPositiveButtonClicked(object sender, EventArgs e)
    {
        _ = ExecuteButtonClickAsync(DialogReturnStatuses.Positive);
    }

    private void OnNegativeButtonClicked(object sender, EventArgs e)
    {
        _ = ExecuteButtonClickAsync(DialogReturnStatuses.Negative);
    }
}