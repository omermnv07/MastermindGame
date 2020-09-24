

namespace MastermindGame
{
    class Program
    {
        static void Main()
        {
            // TODO: Eger birden fazla oyun ekleyecek olursak, implementasyonu degistirip DI eklememiz grekiyor .
            IGame game = new MastermindGame();
            game.Start();
            game.Play();
            game.End();
        }
    }
}
