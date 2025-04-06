using System.Threading.Tasks;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        public static List<Produto> Produtos { get; private set; } = new List<Produto>();
        public MainPage()
        {
            InitializeComponent();
        }
        private async void AdicionarProduto_Clicked(object sender, EventArgs e)
        {
            string nome = nomeEntry.Text;
            string categoria = categoriaEntry.Text;
            DateTime? validade = validadeDatePicker.Date;

            if (double.TryParse(precoEntry.Text, out double preco) && !string.IsNullOrWhiteSpace(nome) && !string.IsNullOrWhiteSpace(categoria) && validade > DateTime.Now)
            {
                MainPage.Produtos.Add(new Produto { Nome = nome, Preco = preco, Categoria = categoria, Validade = validade });

                mensagemLabel.Text = "Produto Cadastrado com Sucesso!";
                nomeEntry.Text = string.Empty;
                precoEntry.Text = string.Empty;
                categoriaEntry.Text = string.Empty;
                validadeDatePicker.Date = DateTime.Now;
                await Task.Delay(3000);
                mensagemLabel.Text = "";
            }
            else
            {
                mensagemLabel.Text = "Preencha os campos corretamente!";
            }
        }
        private async void IrParaLista_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaProdutosPage());
        }
        public class Produto
        {
            public string Nome { get; set; }
            public double Preco { get; set; }
            public string Categoria { get; set; }
            public DateTime? Validade { get; set; }
            public string PrecoFormatado => $"R$ {Preco:F2}";
            public Produto(string nome, double preco, string categoria, DateTime validade)
            {
                Nome = nome ?? throw new ArgumentNullException(nameof(nome), "O nome não pode ser nulo");
                Preco = preco;
                Categoria = categoria ?? throw new ArgumentNullException(nameof(Categoria), "A Categoria não pode ser nula");
                Validade = validade; 
            }
            public Produto() { }
        }
    }
}
