namespace LoftComputacion.WinForms
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            ApiClient apiClient = new ApiClient();
            frmLogin loginForm = new frmLogin(apiClient); // Le pasamos el ApiClient al Login

            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // Si el login fue exitoso, abrimos el principal
                // El ApiClient ya tiene el token guardado
                Application.Run(new frmPrincipal(apiClient));
            }
            // Si el login se cancela o se cierra, la aplicación termina.
        }
    }
}