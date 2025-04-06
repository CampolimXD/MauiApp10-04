namespace MauiApp1;

public partial class ListaProdutosPage : ContentPage
{
	public ListaProdutosPage()
	{
		InitializeComponent();
		produtosListView.ItemsSource = MainPage.Produtos;
       
    }
}