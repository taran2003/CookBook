using CookBoock.ViewModel;

namespace CookBoock.View;

public partial class DeletePopUp
{
	public DeletePopUp()
	{
		InitializeComponent();
		BindingContext = new DeletePopUpViewModel();
	}
}