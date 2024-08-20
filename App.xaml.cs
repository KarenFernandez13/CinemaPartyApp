using ObligatorioTaller1.Servicios;

namespace ObligatorioTaller1
{
    public partial class App : Application
    {
        static Database database;

        public static Database Database
        {
            get
            {
                if (database == null)
                {
                    database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Clientes.db3"));
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();
            var services = new PeliculasPopularesService();
            Task.Run(async () => await Database.InicializarSucursalesPredeterminadasAsync());
            DependencyService.RegisterSingleton<IPeliculasPopularesService>(services);
            MainPage = new NavigationPage(new Login());
        }
    }
}
