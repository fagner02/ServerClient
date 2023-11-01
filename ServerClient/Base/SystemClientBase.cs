using System.Reflection;

namespace SD
{
    /// <summary>
    /// A classe base para cliente de sistema. Cria um conjunto de clientes em apenas um sistema.
    /// </summary>
    public class SystemClientBase
    {
        /// <summary>
        /// Dicionário de clientes
        /// </summary>
        public Dictionary<Type, Client> clients = new();

        /// <summary>
        /// Cria clientes para cada server do sistema definido por systemType
        /// </summary>
        /// <param name="systemType">Tipo do SystemServerBase do qual esse SystemClientBase tem relação</param>
        public void Initialize(Type systemType)
        {
            foreach (FieldInfo field in systemType!.GetFields())
            {
                if (field.CustomAttributes.Any(x => x.AttributeType == typeof(SystemServer)))
                {
                    clients.Add(field.FieldType.BaseType!.GenericTypeArguments.First(), new Client(field.FieldType, systemType));
                }
            }
        }

        /// <summary>
        /// Recebe comandos do console para rodar os métodos de request do cliente 
        /// </summary>
        public void Run()
        {
            List<MethodInfo> methods = new();
            Thread notification = new(MulticastReceiver.Run);
            notification.Start();
            foreach (var x in GetType().GetMethods())
            {
                if (x.DeclaringType != GetType()) continue;
                methods.Add(x);
            }
            while (true)
            {
                Console.WriteLine("enter one of the command numbers or s to exit");
                for (int i = 0; i < methods.Count; i++) { Console.WriteLine(i.ToString() + ": " + methods[i].Name); }
                string? input = Console.ReadLine();
                int num;
                while (true)
                {
                    try
                    {
                        if (input == "s") return;
                        num = int.Parse(input!);
                        if (num < 0 || num >= methods.Count) throw new Exception();
                        methods[num].Invoke(this, Array.Empty<object>());
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("insert a valid command number");
                        input = Console.ReadLine();
                    }
                }
            }
        }

        /// <summary>
        /// Lê uma entidade de tipo T do console
        /// </summary>
        /// <typeparam name="T">Tipo da entidade</typeparam>
        /// <returns>Uma nova instancia da entidade de tipo T</returns>
        public static T ReadInstance<T>()
        {
            Console.WriteLine("insert " + typeof(T).Name + " data");
            var obj = Activator.CreateInstance(typeof(T));
            foreach (FieldInfo field in typeof(T).GetFields())
            {
                ReadField(field, obj);
            }
            return (T)obj!;
        }

        public static object ReadInstance(Type t)
        {
            Console.WriteLine("insert " + t.Name + " data");
            var obj = Activator.CreateInstance(t);
            foreach (FieldInfo field in t.GetFields())
            {
                ReadField(field, obj);
            }
            return Convert.ChangeType(obj!, t);
        }

        private static void ReadField(FieldInfo field, object? obj)
        {
            Console.WriteLine("insert " + field.Name);
            string? input = null;


            if (field.FieldType == typeof(string))
            {
                input = Console.ReadLine();
                while (input == null || input == "")
                {
                    Console.WriteLine("insert value");
                    input = Console.ReadLine();
                }
                field.SetValue(obj, input);
                return;
            }

            if (field.FieldType == typeof(int) || field.FieldType == typeof(float) || field.FieldType == typeof(double))
            {
                ReadNum(field, input, obj!, field.FieldType);
                return;
            }

            if (field.FieldType.IsEnum)
            {
                var names = field.FieldType.GetEnumNames();
                Console.WriteLine("insert one of the numbers");
                for (int i = 0; i < names.Length; i++)
                {
                    Console.WriteLine(i + ": " + names[i]);
                }
                input = Console.ReadLine();
                int value;
                while (true)
                {
                    try
                    {
                        value = int.Parse(input!);
                        field.SetValue(obj, Convert.ChangeType(Enum.Parse(field.FieldType, names[value]), field.FieldType));
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("insert valid value");
                        input = Console.ReadLine();
                    }
                }
                return;
            }

            if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var value = Activator.CreateInstance(field.FieldType);
                var listType = field.FieldType.GenericTypeArguments.First();
                while (true)
                {
                    Console.WriteLine("add new value or insert s to exit");
                    input = Console.ReadLine();
                    if (input == "s") { field.SetValue(obj, value); return; };
                    if (listType.IsClass) { field.FieldType.GetMethod("Add")!.Invoke(value, new object[] { ReadInstance(listType)! }); }
                    else field.FieldType.GetMethod("Add")!.Invoke(value, new object[] { ReadField(listType)! });
                }
            }
        }

        public static object? ReadField(Type type)
        {
            Console.WriteLine("insert " + type.Name);
            string? input = null;


            if (type == typeof(string))
            {
                input = Console.ReadLine();
                while (input == null || input == "")
                {
                    Console.WriteLine("insert value");
                    input = Console.ReadLine();
                }

                return input;
            }

            if (type == typeof(int) || type == typeof(float) || type == typeof(double))
            {
                return ReadNum(type, input);

            }

            if (type.IsEnum)
            {
                var names = type.GetEnumNames();
                Console.WriteLine("insert one of the numbers");
                for (int i = 0; i < names.Length; i++)
                {
                    Console.WriteLine(i + ": " + names[i]);
                }
                input = Console.ReadLine();
                int value;
                while (true)
                {
                    try
                    {
                        value = int.Parse(input!);
                        return Convert.ChangeType(Enum.Parse(type, names[value]), type);
                    }
                    catch
                    {
                        Console.WriteLine("insert valid value");
                        input = Console.ReadLine();
                    }
                }
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                var value = Activator.CreateInstance(type);
                var listType = type.GenericTypeArguments.First();
                while (true)
                {
                    Console.WriteLine("add new value or insert s to exit");
                    input = Console.ReadLine();
                    if (input == "s") { return value; };
                    if (listType.IsClass) { type.GetMethod("Add")!.Invoke(value, new object[] { ReadInstance(listType)! }); }
                    else type.GetMethod("Add")!.Invoke(value, new object[] { ReadField(listType)! });
                }
            }

            return null;
        }

        private static object ReadNum(Type type, string? input)
        {
            var value = Activator.CreateInstance(type);
            while (true)
            {
                try
                {
                    value = type.GetMethod("Parse", new[] { typeof(string) })!.Invoke(null, new object[] { input! }); break;
                }
                catch
                {
                    Console.WriteLine("insert valid value");
                    input = Console.ReadLine();
                }
            }
            return value!;
        }

        private static void ReadNum(FieldInfo field, string? input, object obj, Type numType)
        {
            var value = Activator.CreateInstance(numType);
            while (true)
            {
                try
                {
                    value = numType.GetMethod("Parse", new[] { typeof(string) })!.Invoke(null, new object[] { input! }); break;
                }
                catch
                {
                    Console.WriteLine("insert valid value");
                    input = Console.ReadLine();
                }
            }
            field.SetValue(obj, value);
        }

        /// <summary>
        /// Faz uma request definida pelo method para o cliente do tipo T 
        /// </summary>
        /// <typeparam name="T">Tipo de entidade que o cliente usa</typeparam>
        /// <param name="method">O método de request do server que o cliente vai chamar</param>
        /// <returns>O resultado do request do server</returns>
        public string MakeRequest<T>(string method)
        {
            return clients[typeof(T)].MakeRequest(method);
        }

        /// <summary>
        /// Faz uma request definida pelo method para o cliente do tipo T 
        /// </summary>
        /// <typeparam name="T">Tipo de entidade que o cliente usa</typeparam>
        /// <param name="method">O método de request do server que o cliente vai chamar</param>
        /// <param name="msg">A mensagem a ser enviada para o server na request</param>
        /// <returns>O resultado do request do server</returns>
        public string MakeRequest<T>(string method, string msg)
        {
            return clients[typeof(T)].MakeRequest(method, msg);
        }
    }
}