using System;

namespace Framework
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main(string[] args)
        {
            FrameworkCSM editor = new FrameworkCSM();
            editor.Show();
            editor.game = new Game1(editor.pb_editor.Handle, editor, editor.pb_editor);
            editor.game.Run();
        }
    }
#endif
}

