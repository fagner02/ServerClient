using System.Reflection;

namespace SD
{
    public abstract class SystemClass
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
                    field.FieldType.GetField("Timeout")!.SetValue(value, attribute.ConstructorArguments[1].Value);
                    int delay = (int)attribute.ConstructorArguments[2].Value!;
                    tasks.Add(Task.Run(() =>
                    {
                        if (delay > 0) { Console.WriteLine(field.FieldType.Name + " start delayed " + delay / 1000 + "s"); }
                        Task.Delay(delay).Wait();
                        field.FieldType.GetMethod("Setup")!.Invoke(value, new object[] { GetType() });
                    }));
                }
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}