using System.Reflection;

namespace UseAutoAlmostAllAssemblyType;
/// <summary>
/// Класс, что использует данные из сборки для создания объектов и использования методов, полуавтоматически...
/// </summary>
class UseAutoAlmostAllAssemblyType
{
    public static void Main()
    {
        //будет получать возвращаемые значения методов
        int value;

        //получить всю информацию о сборке
        Assembly assembly = Assembly.LoadFrom(@"//пусть к длл");

        //получть из всей информации только данные о типах
        Type[] alltype = assembly.GetTypes();

        //получить данные о первом типе
        Type onlytype = alltype[0];

        Console.WriteLine("Выбрать один тип: " + onlytype.Name);

        //получить данные конструкторов певрого типа
        ConstructorInfo[] constructors = onlytype.GetConstructors();

        //получить параметры первого конструктора первого типа
        ParameterInfo[] parameters = constructors[0].GetParameters();

        //будет присвиватся созданный из конструктора по отправленным парамметрам объект
        object reflectObj;

        //перебрать параметры
        if (parameters.Length > 0)
        {
            //создать аргументы для для выбранного конструктора
            object[] constrargs = new object[parameters.Length];

            for (int i = 0; i < constrargs.Length; i++)
            {
                constrargs[i] = 111 * i;
            }
            //создать объект из выбранного конструктора по отправленным параметрам
            reflectObj = constructors[0].Invoke(constrargs);
        }
        else
        {
            //создать "пустой" объект
            reflectObj = constructors[0].Invoke(null);
        }

        Console.WriteLine($"Вызов методов для объекта {nameof(reflectObj)}");

        //получить "все" методы находящиеся в типе. DeclaredOnly - только методы экземпляра, не наследуемые, открытые. 
        MethodInfo[] allmethodsfromtype = onlytype.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

        //вызвать подходящте методы по условию
        foreach (MethodInfo method in allmethodsfromtype)
        {
            //получть параметры что есть в выбранном циклом методе
            ParameterInfo[] param = method.GetParameters();

            switch (param.Length)
            {
                case 0:
                    //проверка совместимости типов
                    if (method.ReturnType == typeof(int))
                    {
                        //вызвать метод без аргументов и присвоить возвращаемое значение
                        value = (int)method.Invoke(reflectObj, null);
                    }
                    else if (method.ReturnType == typeof(void))
                    {
                        //просто вызвать метод
                        method.Invoke(reflectObj, null);
                    }
                    break;

                case 1:
                    //проверка совместимости типов
                    if (param[0].ParameterType == typeof(int))
                    {
                        //если есть метод с одним параметром, то создать для него аргумент
                        object[] arg = new object[1];

                        arg[0] = 15;

                        //вызвать метод
                        if ((bool)method.Invoke(reflectObj, arg))
                        {
                            Console.WriteLine("Вызов метода с одним параметром прошёл успешно");
                        }
                        else
                        {
                            Console.WriteLine("Вызов метода с одним параметром прошёл НЕуспешно");
                        }

                    }
                    break;

                case 2:
                    //проверка совместимости типов
                    if (param[0].ParameterType == typeof(int) && param[1].ParameterType == typeof(int))
                    {
                        //если есть метод с двумя параметром, то создать для него аргументы
                        object[] arg = new object[2];

                        arg[0] = 15;

                        arg[1] = 25;

                        //вызвать метод и передать в него параметры
                        method.Invoke(reflectObj, arg);
                    }
                    else if (param[0].ParameterType == typeof(double) && param[1].ParameterType == typeof(double))
                    {
                        object[] arg = new object[2];

                        arg[0] = 15.5635635;

                        arg[1] = 25.1255;

                        method.Invoke(reflectObj, arg);
                    }
                    break;
            }

            Console.WriteLine();
        }
    }
}
