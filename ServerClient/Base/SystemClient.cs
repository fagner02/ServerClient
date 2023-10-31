using System.Reflection;

namespace SD
{
    public class SystemClient
    {
        public Dictionary<Type, Client> clients = new();

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

        public void Run()
        {
            List<MethodInfo> methods = new();
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

        public static T ReadInstance<T>()
        {
            Console.WriteLine("insert " + typeof(T).Name + " data");
            var obj = Activator.CreateInstance(typeof(T));
            foreach (FieldInfo field in typeof(T).GetFields())
            {
                Console.WriteLine("insert " + field.Name);
                string? input = Console.ReadLine();
                while (input == null)
                {
                    Console.WriteLine("insert value");
                    input = Console.ReadLine();
                }

                if (field.FieldType == typeof(string))
                {
                    field.SetValue(obj, input);
                }

                if (field.FieldType == typeof(int))
                {
                    int value;
                    while (true)
                    {
                        try { value = int.Parse(input!); break; }
                        catch
                        {
                            Console.WriteLine("insert valid value");
                            input = Console.ReadLine();
                        }
                    }
                    field.SetValue(obj, value);
                    continue;
                }

                if (field.FieldType == typeof(float))
                {
                    float value;
                    while (true)
                    {
                        try { value = float.Parse(input!); break; }
                        catch
                        {
                            Console.WriteLine("insert valid value");
                            input = Console.ReadLine();
                        }
                    }
                    field.SetValue(obj, value);
                    continue;
                }

                if (field.FieldType == typeof(double))
                {
                    double value;
                    while (true)
                    {
                        try { value = double.Parse(input!); break; }
                        catch
                        {
                            Console.WriteLine("insert valid value");
                            input = Console.ReadLine();
                        }
                    }
                    field.SetValue(obj, value);
                    continue;
                }
            }
            return (T)obj!;
        }

        public string MakeRequest<T>(string method)
        {
            return clients[typeof(T)].MakeRequest(method);
        }

        public string MakeRequest<T>(string method, string msg)
        {
            return clients[typeof(T)].MakeRequest(method, msg);
        }
    }
}