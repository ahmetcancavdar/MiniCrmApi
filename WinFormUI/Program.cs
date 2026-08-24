namespace WinFormUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Beklenmedik bir hata (örn. try/catch ile sarılmamış bir olay
            // işleyicisindeki istisna) uygulamayı çökertmesin; kullanıcıya
            // bir mesaj kutusu gösterip çalışmaya devam etsin.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, e) => ShowUnhandledError(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
                ShowUnhandledError(e.ExceptionObject as Exception ?? new Exception(e.ExceptionObject?.ToString() ?? "Bilinmeyen hata"));

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Login());
        }

        private static void ShowUnhandledError(Exception ex)
        {
            MessageBox.Show(
                $"Beklenmeyen bir hata oluştu:\n{ex.Message}\n\nİşleme devam edebilirsiniz.",
                "Hata",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}