// See https://aka.ms/new-console-template for more information
using LearningOpenTk4;
using LearnOpenTK;
using OpenTK.Windowing.Desktop;

using (Game game = new Game(GameWindowSettings.Default, NativeWindowSettings.Default))
{
    //run the game at 60 fps
    game.Run();
}
