using System.Threading.Tasks;
using static MauiApp1.ListaProdutosPage;
using static MauiApp1.MainPage;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        public static List<Produto> Produtos { get; set; } = ProdutoStorage.CarregarProdutos();

        public MainPage()
        {
            InitializeComponent();
        }

        private async void AdicionarProduto_Clicked(object sender, EventArgs e)
        {
            string nome = nomeEntry.Text;
            string categoria = CategoriaPicker.SelectedItem?.ToString();
            DateTime? validade = validadeDatePicker.Date;
            

            if (double.TryParse(precoEntry.Text, out double preco) &&
                !string.IsNullOrWhiteSpace(nome) &&
                !string.IsNullOrWhiteSpace(categoria))
            {
                MainPage.Produtos.Add(new Produto
                {
                    Nome = nome,
                    Preco = preco,
                    Categoria = categoria,
                    Validade = validade,
                    CaminhoImagem = caminhoImagemSelecionada
                });
               
                ProdutoStorage.SalvarProdutos(Produtos);
                mensagemLabel.Text = "Produto Cadastrado com Sucesso!";
                nomeEntry.Text = string.Empty;
                precoEntry.Text = string.Empty;
                CategoriaPicker.SelectedIndex = -1;
                validadeDatePicker.Date = DateTime.Now;
                previewImagem.Source = null;
                caminhoImagemSelecionada = null;

                await Task.Delay(3000);
                mensagemLabel.Text = string.Empty;
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
            public string Nome { get; set; } = string.Empty;
            public string Descricao { get; set; } = string.Empty;
            public string Categoria { get; set; } = string.Empty;
            public double Preco { get; set; }
            public DateTime? Validade { get; set; }
            public string? CaminhoImagem { get; set; }
            public string ValidadeFormatada => Validade?.ToString("dd/MM/yyyy") ?? "Sem validade";
            public string PrecoFormatado => $"R$ {Preco:F2}";
            public bool IsValidadeVencida => Validade.HasValue && Validade.Value < DateTime.Now;


            public Produto(string nome, double preco, string categoria, DateTime validade)
            {
                Nome = nome ?? throw new ArgumentNullException(nameof(nome), "O nome não pode ser nulo");
                Preco = preco;
                Categoria = categoria ?? throw new ArgumentNullException(nameof(Categoria), "A Categoria não pode ser nula");
                Validade = validade;
            }

            public Produto() { }
        }
        private string caminhoImagemSelecionada;
        private async void SelecionarImagem_Clicked(object sender, EventArgs e)
        {
            var resultado = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Selecione uma imagem",
                FileTypes = FilePickerFileType.Images
            });

            if (resultado != null)
            {
                caminhoImagemSelecionada = resultado.FullPath;
                previewImagem.Source =
                    ImageSource.FromFile(caminhoImagemSelecionada);
            }
        }
    }
}
