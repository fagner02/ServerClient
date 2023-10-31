using System.Reflection;

namespace SD
{
    public abstract class SystemBase
    {

        public void Run()
        {
            List<Task> tasks = new();
            foreach (FieldInfo field in GetType().GetFields())
            {
                CustomAttributeData? attribute = field.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(SystemServer));
                if (attribute != null)
                {
                    object value = field.GetValue(this)!;
                    field.FieldType.GetField(nameof(ServerBase.Timeout))!.SetValue(value, attribute.ConstructorArguments[1].Value);
                    int delay = (int)attribute.ConstructorArguments[2].Value!;
                    tasks.Add(Task.Run(() =>
                    {
                        if (delay > 0) { Console.WriteLine(field.FieldType.Name + " start delayed " + delay / 1000 + "s"); }
                        Task.Delay(delay).Wait();
                        field.FieldType.GetMethod(nameof(ServerBase.Setup))!.Invoke(value, new object[] { GetType() });
                    }));
                }
            }

            tasks.Add(Task.Run(() =>
             {
                 while (true)
                 {
                     Console.WriteLine("insert message to send to clients");
                     string input = Console.ReadLine()!;
                     MulticastSender.Send(input);
                 }
             }));

            Task.WaitAll(tasks.ToArray());
        }
    }
}