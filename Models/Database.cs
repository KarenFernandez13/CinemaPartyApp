using ObligatorioTaller1.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Database
{
    private readonly SQLiteAsyncConnection _database;

    public Database(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Cliente>().Wait();
        _database.CreateTableAsync<Sucursal>().Wait();
    }

    public async Task InicializarSucursalesPredeterminadasAsync()
    {
        // Verificar si ya existen sucursales en la base de datos
        var sucursalesExistentes = await _database.Table<Sucursal>().ToListAsync();

        if (sucursalesExistentes.Count == 0)
        {
            // Si no hay sucursales, agregar las predeterminadas
            var sucursales = new List<Sucursal>
            {
                new Sucursal
                {
                    Name = "Sucursal Pop",
                    Address = "Rafael Pérez del Puerto 936, 20000 Maldonado, Uruguay",
                    Phone = 332411,
                    HoraApertura = 12,
                    HoraCierre = 20,
                    Latitude = (-34.9100831),
                    Longitude = (-54.961363)
                },
                new Sucursal
                {
                    Name = "Sucursal 3D",
                    Address = "Avenida Córdoba 39, 20000 Punta Del Este, Uruguay",
                    Phone = 22334455,
                    HoraApertura = 10,
                    HoraCierre = 22,
                     Latitude = (-34.9168327),
                    Longitude = (-54.9510004)
                },
                new Sucursal
                {
                    Name = "Sucursal PackXL",
                    Address = "Selva Negra 16, 20000 Punta Del Este, Uruguay",
                    Phone = 44556677,
                    HoraApertura = 9,
                    HoraCierre = 21,
                    Latitude = (-34.9365674),
                    Longitude = (-54.9478794)
                },
                new Sucursal
                {
                    Name = "Sucursal Pantalla Gigante",
                    Address = "Avenida José Batlle y Ordóñez 1368, 20000 Maldonado, Uruguay",
                    Phone = 55667788,
                    HoraApertura = 17,
                    HoraCierre = 01,
                    Latitude = (-34.8984386),
                    Longitude = (-54.9533202)
                }
            };

            await _database.InsertAllAsync(sucursales);
        }
    }

    public Task<List<Cliente>> GetClienteAsync()
    {
        return _database.Table<Cliente>().ToListAsync();
    }

    public Task<Cliente> GetClienteAsync(int id)
    {
        return _database.Table<Cliente>().Where(i => i.Id == id).FirstOrDefaultAsync();
    }

    public Task<Cliente> GetClienteByCredentialsAsync(string username, string password)
    {
        return _database.Table<Cliente>()
                        .Where(c => c.NombreUsuario == username && c.Password == password)
                        .FirstOrDefaultAsync();
    }

    public Task<int> SaveClienteAsync(Cliente cliente)
    {
        if (cliente.Id != 0)
        {
            return _database.UpdateAsync(cliente);
        }
        else
        {
            return _database.InsertAsync(cliente);
        }
    }

    public Task<int> DeleteClienteAsync(Cliente cliente)
    {
        return _database.DeleteAsync(cliente);
    }
    public Task<List<Sucursal>> GetSucursalesAsync()
    {
        return _database.Table<Sucursal>().ToListAsync();
    }

    public Task<Sucursal> GetSucursalPorIdAsync(int id)
    {
        return _database.Table<Sucursal>().Where(i => i.id == id).FirstOrDefaultAsync();
    }
    public Task<Sucursal> GetSucursalByNameAsync(string name)
    {
        return _database.Table<Sucursal>()
                        .Where(c => c.Name == name)
                        .FirstOrDefaultAsync();
    }

    public Task<int> GuardarSucursalAsync(Sucursal sucursal)
    {
        if (sucursal.id != 0)
        {
            return _database.UpdateAsync(sucursal);
        }
        else
        {
            return _database.InsertAsync(sucursal);
        }
    }
    public Task<int> DeleteSucursalAsync(Sucursal sucursal)
    {
        return _database.DeleteAsync(sucursal);
    }
}
