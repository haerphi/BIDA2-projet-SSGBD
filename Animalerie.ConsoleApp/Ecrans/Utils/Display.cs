namespace Animalerie.ConsoleApp.Ecrans.Utils
{
    public static class Display
    {
        public static void EnumOptions<TEnum>() where TEnum : struct, Enum
        {
            TEnum[] values = Enum.GetValues<TEnum>();

            foreach (TEnum value in values)
            {
                int key = Convert.ToInt32(value);
                string name = value.ToString();

                Console.WriteLine($"\t{key} - {name}");
            }
        }
    }
}