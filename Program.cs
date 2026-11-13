using System;
class Program
{
    // Vectores para almacenar la informacion
    static string[] codigos = new string[20];
    static string[] nombres = new string[20];
    static string[] categorias = new string[20];

    // Matriz:
    // Columna 0 = Precio
    // Columna 1 = Cantidad disponible
    // Columna 2 = Unidades vendidas
    static decimal[,] inventario = new decimal[20, 3];

    static int cantidadProductos = 0;
    static decimal totalVendido = 0;

    static void Main()
    {
        int opcion;

        do
        {
            Console.WriteLine("\n======================================");
            Console.WriteLine(" SISTEMA DE INVENTARIO - CAFETERIA");
            Console.WriteLine("======================================");
            Console.WriteLine("1. Registrar producto");
            Console.WriteLine("2. Realizar venta");
            Console.WriteLine("3. Actualizar inventario");
            Console.WriteLine("4. Mostrar productos");
            Console.WriteLine("5. Mostrar productos agotados");
            Console.WriteLine("6. Calcular total vendido del dia");
            Console.WriteLine("7. Salir");
            Console.Write("Seleccione una opcion: ");

            if (!int.TryParse(Console.ReadLine(), out opcion))
            {
                opcion = 0;
            }

            switch (opcion)
            {
                case 1:
                    RegistrarProducto();
                    break;

                case 2:
                    RealizarVenta();
                    break;

                case 3:
                    ActualizarInventario();
                    break;

                case 4:
                    MostrarProductos();
                    break;

                case 5:
                    MostrarAgotados();
                    break;

                case 6:
                    MostrarTotalVendido();
                    break;

                case 7:
                    Console.WriteLine("Programa finalizado.");
                    break;

                default:
                    Console.WriteLine("Opcion no valida.");
                    break;
            }

        } while (opcion != 7);
    }

    // Busca un producto por su codigo
    static int BuscarProducto(string codigo)
    {
        for (int i = 0; i < cantidadProductos; i++)
        {
            if (codigos[i] == codigo)
            {
                return i;
            }
        }

        return -1;
    }

    // Registra un nuevo producto
    static void RegistrarProducto()
    {
        if (cantidadProductos >= 20)
        {
            Console.WriteLine("Inventario lleno. No se pueden registrar mas productos.");
            return;
        }

        Console.Write("\nIngrese el codigo del producto: ");
        string codigo = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(codigo))
        {
            Console.WriteLine("El codigo no puede estar vacio.");
            return;
        }

        if (BuscarProducto(codigo) != -1)
        {
            Console.WriteLine("Error: el codigo ya existe.");
            return;
        }

        Console.Write("Ingrese el nombre: ");
        string nombre = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(nombre))
        {
            Console.WriteLine("El nombre no puede estar vacio.");
            return;
        }

        decimal precio;

        do
        {
            Console.Write("Ingrese el precio unitario: ");

            if (!decimal.TryParse(
                Console.ReadLine(),
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out precio) || precio <= 0)
            {
                precio = -1;
                Console.WriteLine("Precio no valido. Debe ser mayor que cero.");
            }

        } while (precio <= 0);

        int cantidad;

        do
        {
            Console.Write("Ingrese la cantidad disponible: ");

            if (!int.TryParse(Console.ReadLine(), out cantidad) ||
                cantidad < 0)
            {
                cantidad = -1;
                Console.WriteLine("Cantidad no valida.");
            }

        } while (cantidad < 0);

        string categoria = SeleccionarCategoria();

        codigos[cantidadProductos] = codigo;
        nombres[cantidadProductos] = nombre;
        categorias[cantidadProductos] = categoria;

        inventario[cantidadProductos, 0] = precio;
        inventario[cantidadProductos, 1] = cantidad;
        inventario[cantidadProductos, 2] = 0;

        cantidadProductos++;

        Console.WriteLine("\nProducto registrado correctamente.");
    }

    // Permite seleccionar una categoria
    static string SeleccionarCategoria()
    {
        int opcion;

        do
        {
            Console.WriteLine("\nCategorias:");
            Console.WriteLine("1. Bebidas");
            Console.WriteLine("2. Panaderia");
            Console.WriteLine("3. Snacks");
            Console.Write("Seleccione la categoria: ");

            if (!int.TryParse(Console.ReadLine(), out opcion))
            {
                opcion = 0;
            }

            switch (opcion)
            {
                case 1:
                    return "Bebidas";

                case 2:
                    return "Panaderia";

                case 3:
                    return "Snacks";

                default:
                    Console.WriteLine("Categoria no valida.");
                    break;
            }

        } while (true);
    }

    // Registra una venta y descuenta las existencias
    static void RealizarVenta()
    {
        if (cantidadProductos == 0)
        {
            Console.WriteLine("No hay productos registrados.");
            return;
        }

        Console.Write("\nIngrese el codigo del producto: ");
        string codigo = Console.ReadLine() ?? "";

        int posicion = BuscarProducto(codigo);

        if (posicion == -1)
        {
            Console.WriteLine("Producto no encontrado.");
            return;
        }

        int stock = (int)inventario[posicion, 1];

        if (stock == 0)
        {
            Console.WriteLine("Producto agotado. No se puede realizar la venta.");
            return;
        }

        Console.WriteLine("Producto: " + nombres[posicion]);
        Console.WriteLine("Precio: " + inventario[posicion, 0]);
        Console.WriteLine("Stock disponible: " + stock);

        Console.Write("Cantidad que desea vender: ");

        if (!int.TryParse(Console.ReadLine(), out int cantidad) ||
            cantidad <= 0)
        {
            Console.WriteLine("Cantidad no valida.");
            return;
        }

        if (cantidad > stock)
        {
            Console.WriteLine("No hay suficiente inventario.");
            return;
        }

        decimal subtotal = inventario[posicion, 0] * cantidad;

        inventario[posicion, 1] -= cantidad;
        inventario[posicion, 2] += cantidad;

        totalVendido += subtotal;

        Console.WriteLine("\nVENTA REALIZADA");
        Console.WriteLine("Producto: " + nombres[posicion]);
        Console.WriteLine("Unidades: " + cantidad);
        Console.WriteLine("Subtotal: $" + subtotal.ToString("N2"));
        Console.WriteLine("Stock restante: " + inventario[posicion, 1]);
    }

    // Aumenta o disminuye las existencias
    static void ActualizarInventario()
    {
        if (cantidadProductos == 0)
        {
            Console.WriteLine("No hay productos registrados.");
            return;
        }

        Console.Write("\nIngrese el codigo del producto: ");
        string codigo = Console.ReadLine() ?? "";

        int posicion = BuscarProducto(codigo);

        if (posicion == -1)
        {
            Console.WriteLine("Producto no encontrado.");
            return;
        }

        Console.WriteLine("Producto: " + nombres[posicion]);
        Console.WriteLine("Stock actual: " + inventario[posicion, 1]);

        Console.WriteLine("\n1. Agregar unidades");
        Console.WriteLine("2. Retirar unidades");
        Console.Write("Seleccione una opcion: ");

        if (!int.TryParse(Console.ReadLine(), out int opcion) ||
            (opcion != 1 && opcion != 2))
        {
            Console.WriteLine("Opcion no valida.");
            return;
        }

        Console.Write("Ingrese la cantidad: ");

        if (!int.TryParse(Console.ReadLine(), out int cantidad) ||
            cantidad <= 0)
        {
            Console.WriteLine("Cantidad no valida.");
            return;
        }

        int stockActual = (int)inventario[posicion, 1];

        if (opcion == 1)
        {
            inventario[posicion, 1] += cantidad;
            Console.WriteLine("Inventario actualizado correctamente.");
        }
        else
        {
            if (cantidad > stockActual)
            {
                Console.WriteLine("No se pueden retirar mas unidades de las disponibles.");
                return;
            }

            inventario[posicion, 1] -= cantidad;
            Console.WriteLine("Unidades retiradas correctamente.");
        }

        Console.WriteLine("Nuevo stock: " + inventario[posicion, 1]);
    }

    // Muestra todos los productos registrados
    static void MostrarProductos()
    {
        if (cantidadProductos == 0)
        {
            Console.WriteLine("No hay productos registrados.");
            return;
        }

        Console.WriteLine("\n========== LISTADO DE PRODUCTOS ==========");

        for (int i = 0; i < cantidadProductos; i++)
        {
            Console.WriteLine("\nCodigo: " + codigos[i]);
            Console.WriteLine("Nombre: " + nombres[i]);
            Console.WriteLine("Categoria: " + categorias[i]);
            Console.WriteLine("Precio: $" + inventario[i, 0].ToString("N2"));
            Console.WriteLine("Cantidad disponible: " + inventario[i, 1]);
            Console.WriteLine("Unidades vendidas: " + inventario[i, 2]);
        }
    }

    // Identifica los productos que no tienen existencias
    static void MostrarAgotados()
    {
        bool encontrado = false;

        Console.WriteLine("\n========== PRODUCTOS AGOTADOS ==========");

        for (int i = 0; i < cantidadProductos; i++)
        {
            if (inventario[i, 1] == 0)
            {
                Console.WriteLine("Codigo: " + codigos[i] +
                                  " - Producto: " + nombres[i]);

                encontrado = true;
            }
        }

        if (!encontrado)
        {
            Console.WriteLine("No hay productos agotados.");
        }
    }

    // Muestra el dinero acumulado por las ventas
    static void MostrarTotalVendido()
    {
        Console.WriteLine("\n========== VENTAS DEL DIA ==========");
        Console.WriteLine("Total vendido: $" +
                          totalVendido.ToString("N2"));
    }
}